using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InvHookTest.Utils
{
    public class GSqlUtils
    {

        public static string GetConnectionString(string database, string username, string password, string initialcatalog, bool integrated = false)
        {
            if (!integrated)
            {
                // Initial Catalog really set up properly?
                return String.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}", database, initialcatalog, username, password);
            }
            else
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = database;
                builder.InitialCatalog = initialcatalog;
                builder.IntegratedSecurity = true;
                return builder.ConnectionString;
            }
        }
    }
}
