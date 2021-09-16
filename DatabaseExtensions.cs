using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolshed.Npoco
{
    public static class DatabaseExtensions
    {
        public static string GetTableName(this NPoco.Database database, Type type)
        {
            return database.PocoDataFactory.ForType(type).TableInfo.TableName;
        }
        public static string GetTableName<T>(this T obj, Database database) where T : class, INpocoTableInfo
        {
            return database.PocoDataFactory.ForType(obj.GetType()).TableInfo.TableName;
        }
    }
}
