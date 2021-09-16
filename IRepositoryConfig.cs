using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolshed.Npoco
{
    public interface IRepositoryConfig
    {
        string ConnectionString { get; set; }

        string RedisHost { get; set; }
        string RedisPort { get; set; }
        string RedisDatabase { get; set; }
    }
}
