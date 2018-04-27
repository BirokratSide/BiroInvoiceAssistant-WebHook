using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;

namespace InvHookTest.Utils
{
    public class CMsSqlConnection
    {
        #region // locals //
        string _connection_string = null;
        SqlConnection _connection = null;
        bool _auto_open_close = true;
        int _connection_timeout = 10;
        int _command_timeout = 60;
        #endregion
        #region // constructor //
        public CMsSqlConnection(string connection_string)
        {
            _connection_string = connection_string;
        }
        #endregion
        #region // properties //
        public IDbConnection connection
        {
            get { return _connection; }
        }
        public bool autoOpenClose
        {
            get { return _auto_open_close; }
            set { _auto_open_close = value; }
        }
        public int commandTimeout
        {
            get { return _command_timeout; }
            set { _command_timeout = value; }
        }
        public int connectionTimeout
        {
            get { return _connection_timeout; }
            set { _connection_timeout = value; }
        }
        #endregion
        #region // public - generate //
        public IDbCommand GenerateCommand()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = commandTimeout;
            return cmd;
        }
        public IDbDataParameter GenerateParameter(string name, object value)
        {
            return new SqlParameter(name, value ?? System.DBNull.Value);
        }
        public IDbDataAdapter GenerateDataAdapter()
        {
            return new SqlDataAdapter();
        }
        public IDbDataAdapter GenerateDataAdapter(IDbCommand command)
        {
            return new SqlDataAdapter((SqlCommand)command);
        }
        public IDbDataAdapter GenerateDataAdapter(string query, IDbConnection connection)
        {
            return new SqlDataAdapter(query, (SqlConnection)connection);
        }
        public IDbDataAdapter GenerateDataAdapter(IDbCommand command_delete, IDbCommand command_insert, IDbCommand command_update)
        {
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.DeleteCommand = (SqlCommand)command_delete;
            sda.InsertCommand = (SqlCommand)command_insert;
            sda.UpdateCommand = (SqlCommand)command_update;
            return sda;
        }
        #endregion
        #region // public - control //
        public void Close()
        {
            int close_max_timeout = 200;
            if (_connection != null)
                _connection.Close();
            while ((close_max_timeout--) > 0)
            {
                if (_connection.State == ConnectionState.Closed)
                    break;
                Thread.Sleep(10);
            }
            _connection = null;
        }
        public void Open()
        {
            if (_connection == null)
            {
                _connection = new SqlConnection();
                _connection.ConnectionString = _connection_string;
                _connection.Open();
            }
        }
        #endregion
        #region // public - execute //
        public DataSet ExecDataSet(IDbCommand command)
        {
            if (autoOpenClose) Open();
            SqlCommand cmd = (SqlCommand)command;
            cmd.Connection = (SqlConnection)_connection;
            DataSet result = new DataSet();
            using (SqlDataAdapter data_adapter = new SqlDataAdapter(cmd))
            {
                data_adapter.Fill(result);
            }
            if (autoOpenClose) Close();
            return result;
        }
        public DataTable ExecDataTable(IDbCommand command)
        {
            if (autoOpenClose) Open();
            SqlCommand cmd = (SqlCommand)command;
            cmd.Connection = _connection;
            SqlDataAdapter data_adapter = new SqlDataAdapter(cmd);
            DataTable result = new DataTable();
            data_adapter.Fill(result);
            data_adapter.Dispose();
            if (autoOpenClose) Close();
            return result;
        }
        public bool ExecDataTableBln(IDbCommand command)
        {
            DataTable dtt = ExecDataTable(command);
            if ((dtt == null) || (dtt.Rows.Count < 1)) return false;
            else return GConv.DbToBln(dtt.Rows[0][0]);
        }
        public decimal ExecDataTableDec(IDbCommand command)
        {
            DataTable dtt = ExecDataTable(command);
            if ((dtt == null) || (dtt.Rows.Count < 1)) return 0;
            else return GConv.DbToDec(dtt.Rows[0][0]);
        }
        public DateTime ExecDataTableDt(IDbCommand command)
        {
            DataTable dtt = ExecDataTable(command);
            if ((dtt == null) || (dtt.Rows.Count < 1)) return DateTime.MinValue;
            else return GConv.DbToDt(dtt.Rows[0][0]);
        }
        public Guid ExecDataTableGid(IDbCommand command)
        {
            DataTable dtt = ExecDataTable(command);
            if ((dtt == null) || (dtt.Rows.Count < 1)) return Guid.Empty;
            else return GConv.DbToGid(dtt.Rows[0][0]);
        }
        public int ExecDataTableInt(IDbCommand command)
        {
            DataTable dtt = ExecDataTable(command);
            if ((dtt == null) || (dtt.Rows.Count < 1)) return 0;
            else return GConv.DbToInt(dtt.Rows[0][0]);
        }
        public long ExecDataTableLng(IDbCommand command)
        {
            DataTable dtt = ExecDataTable(command);
            if ((dtt == null) || (dtt.Rows.Count < 1)) return 0;
            else return GConv.DbToLong(dtt.Rows[0][0]);
        }
        public string ExecDataTableStr(IDbCommand command)
        {
            DataTable dtt = ExecDataTable(command);
            if ((dtt == null) || (dtt.Rows.Count < 1)) return string.Empty;
            else return GConv.DbToStr(dtt.Rows[0][0]);
        }
        public T ExecDatatable<T>(IDbCommand command)
        {
            T result = default(T);
            DataTable dtt = ExecDataTable(command);
            if ((dtt != null) && (dtt.Rows.Count > 0))
                result = ((T)dtt.Rows[0][0]);
            return result;
        }
        public int ExecNonQuery(IDbCommand command)
        {
            if (autoOpenClose) Open();
            SqlCommand cmd = (SqlCommand)command;
            cmd.Connection = _connection;
            int result = 0;
            result = cmd.ExecuteNonQuery();
            if (autoOpenClose) Close();
            return result;
        }
        public bool ExecScalarBln(IDbCommand command)
        {
            bool result = false;
            if (_auto_open_close) Open();
            command.Connection = _connection;
            command.CommandTimeout = _command_timeout;
            result = GConv.DbToBln(command.ExecuteScalar());
            if (_auto_open_close) Close();
            return result;
        }
        public decimal ExecScalarDec(IDbCommand command)
        {
            Decimal result = 0;
            if (_auto_open_close) Open();
            command.Connection = _connection;
            command.CommandTimeout = _command_timeout;
            result = GConv.DbToDec(command.ExecuteScalar());
            if (_auto_open_close) Close();
            return result;
        }
        public Guid ExecScalarGid(IDbCommand command)
        {
            Guid result = Guid.Empty;
            if (_auto_open_close) Open();
            command.Connection = _connection;
            command.CommandTimeout = _command_timeout;
            result = GConv.DbToGid(command.ExecuteScalar());
            if (_auto_open_close) Close();
            return result;
        }
        public int ExecScalarInt(IDbCommand command)
        {
            int result = 0;
            if (_auto_open_close) Open();
            command.Connection = _connection;
            command.CommandTimeout = _command_timeout;
            result = GConv.DbToInt(command.ExecuteScalar());
            if (_auto_open_close) Close();
            return result;
        }
        public long ExecScalarLng(IDbCommand command)
        {
            long result = 0;
            if (_auto_open_close) Open();
            command.Connection = _connection;
            command.CommandTimeout = _command_timeout;
            result = GConv.DbToLong(command.ExecuteScalar());
            if (_auto_open_close) Close();
            return result;
        }
        public string ExecScalarStr(IDbCommand command)
        {
            string result = string.Empty;
            if (_auto_open_close) Open();
            command.Connection = _connection;
            command.CommandTimeout = _command_timeout;
            result = GConv.DbToStr(command.ExecuteScalar());
            if (_auto_open_close) Close();
            return result;
        }
        #endregion
        #region !! IDisposable !!
        public void Dispose()
        {
            Close();
        }
        #endregion
    }
}
