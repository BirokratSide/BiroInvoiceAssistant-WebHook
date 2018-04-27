using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

using InvHookTest.Utils;
using InvHookTest.Structs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InvHookTest
{
    public class BazureInvoiceBufferAPI
    {

        CMsSqlConnection SqlConnection;
        string InitialCatalog;

        #region [constructor]
        public BazureInvoiceBufferAPI(IConfiguration config) {
            string username = config.GetValue<string>("DatabaseConnection:Username");
            string password = config.GetValue<string>("DatabaseConnection:Password");
            string database = config.GetValue<string>("DatabaseConnection:ServerAddress");
            InitialCatalog = config.GetValue<string>("DatabaseConnection:InitialCatalog");
            bool integrated_security = config.GetValue<bool>("DatabaseConnection:IntegratedSecurity");
            SqlConnection = new CMsSqlConnection(GSqlUtils.GetConnectionString(database, username, password, InitialCatalog));
        }
        #endregion

        #region [public]
        public bool CreateDatabaseIfNotExists(string db_name) {
            string query = CreateDatabaseIfNotExistsSql(db_name);
            IDbCommand cmd = SqlConnection.GenerateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            int success = SqlConnection.ExecNonQuery(cmd);
            return success > 0; // are you sure?
        }

        public int CreateTableIfNotExists(string db_name)
        {
            string query = CreateTableIfNotExistsSql(db_name);
            IDbCommand cmd = SqlConnection.GenerateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            int success = SqlConnection.ExecNonQuery(cmd);
            return success;
        }

        public bool SaveRecord(SInvoiceRecord record) {

            if (!record.IsValid()) throw new Exception("The record passed was invalid");

            string valuenames = "(";
            string valuevalues = "(";
            valuenames += "[company_id], ";
            valuenames += "[company_year_id], ";
            valuenames += "[oznaka], ";
            valuenames += "[invoice_assistant_filename], ";
            valuenames += "[invoice_assistant_content], ";
            valuevalues += GConv.StrToDb(record.CompanyId) + ", ";
            valuevalues += GConv.StrToDb(record.CompanyYearId) + ", ";
            valuevalues += GConv.StrToDb(record.Oznaka) + ", ";
            valuevalues += GConv.StrToDb(record.InvoiceAssistantFilename) + ", ";
            valuevalues += GConv.StrToDb(record.InvoiceAssistantContent) + ", ";

            string query = SaveRecordSql(valuenames, valuevalues);

            IDbCommand cmd = SqlConnection.GenerateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            int id = SqlConnection.ExecScalarInt(cmd);
            return id > -1; // be sure which int gets returned if it's wrong!
        }
        #endregion

        #region [private]
        private string CreateDatabaseIfNotExistsSql(string db_name) {
            string query = String.Format(@"
            if (SELECT count(name) FROM sys.databases WHERE name LIKE '%{0}%') = 0 
            BEGIN
                CREATE DATABASE [{0}]
            END;
            ", db_name);
            return query;
        }

        private string CreateTableIfNotExistsSql(string db_name) {
            string query = String.Format("" +
                "use [" + db_name + "] " +
                "IF NOT (EXISTS(SELECT * " +
                                 "FROM INFORMATION_SCHEMA.TABLES " +
                                 "WHERE TABLE_NAME = '{0}')) " +
                "BEGIN " +
                    "CREATE TABLE {0}( " +
                        "id int not null identity(1,1) primary key, " +
                        "company_id varchar(15) not null, " +
                        "company_year_id varchar(15) not null, " +
                        "oznaka varchar(20) not null, " +
                        "additional_params varchar(100), " +
                        "invoice_assistant_filename varchar(80) not null, " +
                        "invoice_assistant_content varchar(MAX) not null " +
                    ") " +
                "END", INVOICE_BUFFER_TABLE_NAME);
            return query;
        }

        private string SaveRecordSql(string valuenames, string valuevalues) {

            return  String.Format(@"
                    
                    use [{0}]
                    INSERT INTO [dbo].[{1}]
                    ({2}} VALUES ({3});
                    SELECT Scope_Identity();

                ", InitialCatalog, INVOICE_BUFFER_TABLE_NAME, valuenames, valuevalues);
        }
        #endregion

        #region [constants]
        private const string INVOICE_BUFFER_TABLE_NAME = "InvoiceBuffer";
        #endregion
    }
}
