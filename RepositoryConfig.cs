using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolshed.Npoco
{
    public class RepositoryConfig : IRepositoryConfig
    {
        public string ConnectionString { get; set; }
        public string RedisHost { get; set; }
        public string RedisPort { get; set; }
        public string RedisDatabase { get; set; }
    }
}
