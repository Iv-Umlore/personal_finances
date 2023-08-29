using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInteraction
{
    public class DbProxy
    {
        public DbProxy(string connectionString) {
            var connection = new SqliteConnection(connectionString);
            connection.Open();
        }
    }
}
