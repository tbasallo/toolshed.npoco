using NPoco.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolshed.Npoco
{
    public class SqlBaseRepository : IDisposable
    {
        public SqlBaseRepository(IRepositoryConfig repositoryConfig)
        {
            Db = new SqlServerDatabase(repositoryConfig.ConnectionString);
        }

        public SqlServerDatabase Db { get; set; }

        public void Dispose()
        {
            if (Db != null)
            {
                Db.Dispose();
            }
        }
    }
}
