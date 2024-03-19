using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PruebaUsuarios.Data
{
    public class MySqlConfig
    {
        public string ConnectionString { get; set; }
        public MySqlConfig(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
