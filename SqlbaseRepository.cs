using Microsoft.Extensions.Caching.Distributed;
using NPoco;
using NPoco.SqlServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Toolshed.Npoco
{
    public class SqlCachedBaseRepository : IDisposable
    {
        public SqlCachedBaseRepository(IRepositoryConfig repositoryConfig, IDistributedCache distributedCache)
        {
            Db = new SqlServerDatabase(repositoryConfig.ConnectionString);
            Caching = distributedCache;
        }

        public SqlServerDatabase Db { get; set; }
        public IDistributedCache Caching { get; set; }
        public bool IsCacheDisabled { get; set; }


        //CACHING

        /// <summary>
        /// Checks the cache for the specified key and if found returns it. If not found, invokes the functions and adds it to the cache and then returns it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key that is search for and used to store the object. The key is case-sensitive.</param>
        /// <param name="minutes">How many sliding minutes should this object be valid for in the cache.</param>
        /// <param name="exp">The function to invoke if the item is not found</param>
        /// <returns></returns>
        public async Task<T> GetSetItemAsync<T>(string key, int minutes, Func<Task<T>> exp)
        {
            if (Caching == null || IsCacheDisabled)
            {
                return await exp.Invoke();
            }

            var str = await Caching.GetStringAsync(key);
            if (str != null)
            {
                if (!string.IsNullOrWhiteSpace(str))
                {
                    if (typeof(T) == typeof(String))
                    {
                        TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

                        return (T)converter.ConvertFromString(null, CultureInfo.InvariantCulture, str);
                    }

                    return JsonSerializer.Deserialize<T>(str, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }

            T obj = await exp.Invoke();
            if (obj != null)
            {
                await Caching.SetStringAsync(key, JsonSerializer.Serialize(obj), new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(minutes) });
            }

            return obj;
        }

        /// <summary>
        /// Checks the cache for the specified key and if found returns it. If not found, invokes the functions and adds it to the cache and then returns it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key that is search for and used to store the object. The key is case-sensitive.</param>
        /// <param name="minutes">How many sliding minutes should this object be valid for in the cache.</param>
        /// <param name="exp">The function to invoke if the item is not found</param>
        /// <returns></returns>
        public T GetSetItem<T>(string key, int minutes, Func<T> exp)
        {
            if (Caching == null || IsCacheDisabled)
            {
                return exp.Invoke();
            }

            var str = Caching.GetString(key);
            if (str != null)
            {
                if (!string.IsNullOrWhiteSpace(str))
                {
                    if (typeof(T) == typeof(String))
                    {
                        TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

                        return (T)converter.ConvertFromString(null, CultureInfo.InvariantCulture, str);
                    }

                    return JsonSerializer.Deserialize<T>(str, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }

            T obj = exp.Invoke();
            if (obj != null)
            {
                Caching.SetString(key, JsonSerializer.Serialize(obj), new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(minutes) });
            }

            return obj;
        }


        public void Dispose()
        {
            if (Db != null)
            {
                Db.Dispose();
            }
        }
    }
}
