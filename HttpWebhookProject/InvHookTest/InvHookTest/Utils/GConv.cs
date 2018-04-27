using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace InvHookTest.utils
{
    public static class GConv
    {
        #region // sql - net - string //
        public static string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public static string DATE_FORMAT = "yyyy-MM-dd";


        public static string DbToStr(object data)
        {
            return (data == System.DBNull.Value) ? string.Empty : Convert.ToString(data);
        }
        public static string DbToStr(object data, string default_value)
        {
            return (data == System.DBNull.Value) ? default_value : Convert.ToString(data);
        }
        public static string DbToStrNull(object data)
        {
            return (data == System.DBNull.Value) ? null : Convert.ToString(data);
        }
        public static object StrToDb(string data)
        {
            return "'" + data + "'";
        }
        public static object StrToDb(string data, string nullval)
        {
            return data == nullval ? System.DBNull.Value : (object)data;
        }
        #endregion
        #region // sql - net - bool //
        public static bool DbToBln(object data)
        {
            return (data == System.DBNull.Value) ? false : Convert.ToBoolean(data);
        }
        public static bool DbToBln(object data, bool default_value)
        {
            return (data == System.DBNull.Value) ? default_value : Convert.ToBoolean(data);
        }
        public static bool? DbToBlnNull(object data)
        {
            return (data == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(data);
        }
        public static object BlnToDb(bool data)
        {
            return data;
        }
        public static object BlnToDb(bool data, bool nullval)
        {
            return data == nullval ? System.DBNull.Value : (object)data;
        }
        #endregion
        #region // sql - net - byte //
        public static byte DbToByte(object data)
        {
            return (data == System.DBNull.Value) ? (byte)0 : Convert.ToByte(data);
        }
        public static byte DbToByte(object data, byte default_value)
        {
            return (data == System.DBNull.Value) ? default_value : Convert.ToByte(data);
        }
        public static byte? DbToByteNull(object data)
        {
            return (data == System.DBNull.Value) ? (byte?)null : Convert.ToByte(data);
        }
        public static object ByteToDb(byte data)
        {
            return data;
        }
        public static object ByteToDb(byte data, byte nullval)
        {
            return data == nullval ? System.DBNull.Value : (object)data;
        }
        public static object ByteToDb(byte? data)
        {
            return data;
        }
        public static object ByteToDb(byte? data, byte nullval)
        {
            return data == nullval ? System.DBNull.Value : (object)data;
        }
        #endregion
        #region // sql - net - short //
        public static short DbToShort(object data)
        {
            return (data == System.DBNull.Value) ? (short)0 : Convert.ToInt16(data);
        }
        public static short DbToShort(object data, short default_value)
        {
            return (data == System.DBNull.Value) ? default_value : Convert.ToInt16(data);
        }
        public static short? DbToShortNull(object data)
        {
            return (data == System.DBNull.Value) ? (short?)null : Convert.ToInt16(data);
        }
        public static object ShortToDb(short data)
        {
            return data;
        }
        public static object ShortToDb(short data, short nullval)
        {
            return data == nullval ? System.DBNull.Value : (object)data;
        }
        public static object ShortToDb(short? data)
        {
            return data;
        }
        public static object ShortToDb(short? data, short nullval)
        {
            return data == nullval ? System.DBNull.Value : (object)data;
        }
        #endregion
        #region // sql - net - int //
        public static int DbToInt(object data)
        {
            return (data == System.DBNull.Value) ? 0 : Convert.ToInt32(data);
        }
        public static int DbToInt(object data, int default_value)
        {
            return (data == System.DBNull.Value) ? default_value : Convert.ToInt32(data);
        }
        public static int? DbToIntNull(object data)
        {
            return (data == System.DBNull.Value) ? (int?)null : Convert.ToInt32(data);
        }
        public static object IntToDb(int data)
        {
            return data;
        }
        public static object IntToDb(int data, int nullval)
        {
            return data == nullval ? System.DBNull.Value : (object)data;
        }
        public static object IntToDb(int? data)
        {
            return data;
        }
        public static object IntToDb(int? data, int nullval)
        {
            return data == nullval ? System.DBNull.Value : (object)data;
        }
        #endregion
        #region // sql - net - long //
        public static long DbToLong(object data)
        {
            return (data == System.DBNull.Value) ? (long)0 : Convert.ToInt64(data);
        }
        public static long DbToLong(object data, long default_value)
        {
            return (data == System.DBNull.Value) ? default_value : Convert.ToInt64(data);
        }
        public static long? DbToLongNull(object data)
        {
            return (data == System.DBNull.Value) ? (long?)null : Convert.ToInt64(data);
        }
        public static object LongToDb(long data)
        {
            return data;
        }
        public static object LongToDb(long data, long nullval)
        {
            return data == nullval ? System.DBNull.Value : (object)data;
        }
        #endregion
        #region // sql - net - float //
        public static float DbToFloat(object data)
        {
            return (data == System.DBNull.Value) ? (float)0 : Convert.ToSingle(data);
        }
        public static float DbToFloat(object data, float default_value)
        {
            return (data == System.DBNull.Value) ? default_value : Convert.ToSingle(data);
        }
        public static float? DbToFloatNull(object data)
        {
            return (data == System.DBNull.Value) ? (float?)null : Convert.ToSingle(data);
        }
        #endregion
        #region // sql - net - double //
        public static double DbToDbl(object data)
        {
            return (data == System.DBNull.Value) ? (double)0 : Convert.ToDouble(data);
        }
        public static double DbToDbl(object data, double default_value)
        {
            return (data == System.DBNull.Value) ? default_value : Convert.ToDouble(data);
        }
        public static double? DbToDblNull(object data)
        {
            return (data == System.DBNull.Value) ? (double?)null : Convert.ToDouble(data);
        }
        #endregion
        #region // sql - net - decimal //
        public static decimal DbToDec(object data)
        {
            return (data == System.DBNull.Value) ? (decimal)0 : Convert.ToDecimal(data);
        }
        public static decimal DbToDec(object data, decimal default_value)
        {
            return (data == System.DBNull.Value) ? default_value : Convert.ToDecimal(data);
        }
        public static decimal? DbToDecNull(object data)
        {
            return (data == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(data);
        }
        public static object DecToDb(decimal data)
        {
            return data;
        }
        public static object DecToDb(decimal data, decimal nullval)
        {
            return data == nullval ? System.DBNull.Value : (object)data;
        }
        #endregion
        #region // sql - net - datetime //
        public static DateTime DbToDt(object data)
        {
            return (data == System.DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(data);
        }
        public static DateTime DbToDt(object data, DateTime default_value)
        {
            return (data == System.DBNull.Value) ? default_value : Convert.ToDateTime(data);
        }
        public static DateTime? DbToDtNull(object data)
        {
            return (data == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(data);
        }
        public static object DtToDb(DateTime data)
        {
            return data;
        }
        public static object DtToDb(DateTime data, DateTime nullval)
        {
            return data == nullval ? System.DBNull.Value : (object)data;
        }
        #endregion
        #region // sql - net - guid //
        public static Guid DbToGid(object data)
        {
            return (data == System.DBNull.Value) ? Guid.Empty : (Guid)data;
        }
        public static Guid DbToGid(object data, Guid default_value)
        {
            return (data == System.DBNull.Value) ? default_value : (Guid)data;
        }
        public static Guid? DbToGidNull(object data)
        {
            return (data == System.DBNull.Value) ? (Guid?)null : (Guid)data;
        }
        public static object GidToDb(Guid data)
        {
            return data;
        }
        public static object GidToDb(Guid data, Guid nullval)
        {
            return data == nullval ? System.DBNull.Value : (object)data;
        }
        #endregion
        #region // sql - net - timestamp //
        public static ulong DbTsToLong(object data)
        {
            if (data != System.DBNull.Value)
            {
                byte[] byteData = (byte[])data;
                Array.Reverse(byteData);
                return BitConverter.ToUInt64(byteData, 0);
            }
            else
            {
                return 0;
            }

        }
        #endregion

        #region // sql //
        public static SqlDbType SqlTypeToSqlDbType(int type_id)
        {
            SqlDbType typOut = SqlDbType.Variant;
            switch (type_id)
            {
                case 34: typOut = SqlDbType.Image; break;
                case 35: typOut = SqlDbType.Text; break;
                case 36: typOut = SqlDbType.UniqueIdentifier; break;
                case 40: typOut = SqlDbType.Date; break;
                case 41: typOut = SqlDbType.Time; break;
                case 42: typOut = SqlDbType.DateTime2; break;
                case 43: typOut = SqlDbType.DateTimeOffset; break;
                case 48: typOut = SqlDbType.TinyInt; break;
                case 52: typOut = SqlDbType.SmallInt; break;
                case 56: typOut = SqlDbType.Int; break;
                case 58: typOut = SqlDbType.SmallDateTime; break;
                case 59: typOut = SqlDbType.Real; break;
                case 60: typOut = SqlDbType.Money; break;
                case 61: typOut = SqlDbType.DateTime; break;
                case 62: typOut = SqlDbType.Float; break;
                case 98: typOut = SqlDbType.Variant; break;
                case 99: typOut = SqlDbType.NText; break;
                case 104: typOut = SqlDbType.Bit; break;
                case 106: typOut = SqlDbType.Decimal; break;
                case 108: typOut = SqlDbType.Decimal; break; //numeric
                case 122: typOut = SqlDbType.SmallMoney; break;
                case 127: typOut = SqlDbType.BigInt; break;
                case 128: typOut = SqlDbType.Variant; break; //hierarchyid
                case 129: typOut = SqlDbType.Variant; break; //geometry
                case 130: typOut = SqlDbType.Variant; break; //geography
                case 165: typOut = SqlDbType.VarBinary; break;
                case 167: typOut = SqlDbType.VarChar; break;
                case 173: typOut = SqlDbType.Binary; break;
                case 175: typOut = SqlDbType.Char; break;
                case 189: typOut = SqlDbType.Timestamp; break;
                case 231: typOut = SqlDbType.NVarChar; break;
                case 239: typOut = SqlDbType.NChar; break;
                case 241: typOut = SqlDbType.Xml; break;
            }
            return typOut;
        }
        public static string SqlTypeFullString(int type_id, int length, int precision, int scale)
        {
            string strData = string.Empty;
            switch (type_id)
            {
                case 34:
                    strData = "image";
                    break;
                case 35:
                    strData = "text";
                    break;
                case 36:
                    strData = "uniqueidentifier";
                    break;
                case 40:
                    strData = "date";
                    break;
                case 41:
                    strData = "time(" + scale.ToString() + ")";
                    break;
                case 42:
                    strData = "datetime2(" + scale.ToString() + ")";
                    break;
                case 43:
                    strData = "datetimeoffset(" + scale.ToString() + ")";
                    break;
                case 48:
                    strData = "tinyint";
                    break;
                case 52:
                    strData = "smallint";
                    break;
                case 56:
                    strData = "int";
                    break;
                case 58:
                    strData = "smalldatetime";
                    break;
                case 59:
                    strData = "real";
                    break;
                case 60:
                    strData = "money";
                    break;
                case 61:
                    strData = "datetime";
                    break;
                case 62:
                    strData = "float";
                    break;
                case 98:
                    strData = "sql_variant";
                    break;
                case 99:
                    strData = "ntext";
                    break;
                case 104:
                    strData = "bit";
                    break;
                case 106:
                    strData = "decimal(" + precision.ToString() + "," + scale.ToString() + ")";
                    break;
                case 108:
                    strData = "numeric(" + precision.ToString() + "," + scale.ToString() + ")";
                    break;
                case 122:
                    strData = "smallmoney";
                    break;
                case 127:
                    strData = "bigint";
                    break;
                case 128:
                    strData = "hierarchyid";
                    break;
                case 129:
                    strData = "geometry";
                    break;
                case 130:
                    strData = "geography";
                    break;
                case 165:
                    strData = "varbinary(" + length.ToString() + ")";
                    break;
                case 167:
                    strData = "varchar(" + length.ToString() + ")";
                    break;
                case 173:
                    strData = "binary(" + length.ToString() + ")";
                    break;
                case 175:
                    strData = "char(" + length.ToString() + ")";
                    break;
                case 189:
                    strData = "timestamp";
                    break;
                case 231:
                    if (length == -1)
                        strData = "nvarchar(MAX)";
                    else
                        strData = "nvarchar(" + (length / 2).ToString() + ")";
                    break;
                case 239:
                    strData = "nchar(" + (length / 2).ToString() + ")";
                    break;
                case 241:
                    strData = "xml";
                    break;
            }
            return strData;
        }
        #endregion

        #region // net type //
        public static int IntParse(String data, int default_value)
        {
            int result = 0;
            if (!int.TryParse(data, out result))
                result = default_value;
            return result;
        }
        public static long LngParse(String data, long default_value)
        {
            long result = 0;
            if (!long.TryParse(data, out result))
            {
                result = default_value;
            }
            return result;
        }
        #endregion

        #region // hex - string //
        public static string ByteArrayToHexString(byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", "");
        }
        #endregion

        #region // objects - datarow //
        public static bool ObjectFromDataRow(object data, DataRow dr, bool include_underscores = false)
        {
            bool result = true;
            Type dataType = data.GetType();
            PropertyInfo[] arrPropertyInfos = dataType.GetProperties();
            foreach (PropertyInfo dataPropertyInfo in arrPropertyInfos)
            {
                Type tmpType = dataPropertyInfo.PropertyType;
                string tmpName = dataPropertyInfo.Name;
                if (tmpName.StartsWith("_") && (!include_underscores))
                    continue;
                if (!dr.Table.Columns.Contains(tmpName))
                    continue;
                if (tmpName.StartsWith("sync_ts"))
                {
                    dataPropertyInfo.SetValue(data, GConv.DbTsToLong(dr[tmpName]), null);
                    continue;
                }
                if (tmpType == typeof(string))
                    dataPropertyInfo.SetValue(data, GConv.DbToStr(dr[tmpName]), null);
                else if (tmpType == typeof(bool))
                    dataPropertyInfo.SetValue(data, GConv.DbToBln(dr[tmpName]), null);
                else if (tmpType == typeof(Byte))
                    dataPropertyInfo.SetValue(data, GConv.DbToByte(dr[tmpName]), null);
                else if (tmpType == typeof(Byte?))
                    dataPropertyInfo.SetValue(data, GConv.DbToByteNull(dr[tmpName]), null);
                else if (tmpType == typeof(Int16))
                    dataPropertyInfo.SetValue(data, GConv.DbToShort(dr[tmpName]), null);
                else if (tmpType == typeof(Int16?))
                    dataPropertyInfo.SetValue(data, GConv.DbToShortNull(dr[tmpName]), null);
                else if (tmpType == typeof(Int32))
                    dataPropertyInfo.SetValue(data, GConv.DbToInt(dr[tmpName]), null);
                else if (tmpType == typeof(Int32?))
                    dataPropertyInfo.SetValue(data, GConv.DbToIntNull(dr[tmpName]), null);
                else if (tmpType == typeof(Int64))
                    dataPropertyInfo.SetValue(data, GConv.DbToLong(dr[tmpName]), null);
                else if (tmpType == typeof(Int64?))
                    dataPropertyInfo.SetValue(data, GConv.DbToLongNull(dr[tmpName]), null);
                else if (tmpType == typeof(Single))
                    dataPropertyInfo.SetValue(data, GConv.DbToFloat(dr[tmpName]), null);
                else if (tmpType == typeof(Single?))
                    dataPropertyInfo.SetValue(data, GConv.DbToFloatNull(dr[tmpName]), null);
                else if (tmpType == typeof(Double))
                    dataPropertyInfo.SetValue(data, GConv.DbToDbl(dr[tmpName]), null);
                else if (tmpType == typeof(Double?))
                    dataPropertyInfo.SetValue(data, GConv.DbToDblNull(dr[tmpName]), null);
                else if (tmpType == typeof(Decimal))
                    dataPropertyInfo.SetValue(data, GConv.DbToDec(dr[tmpName]), null);
                else if (tmpType == typeof(Decimal?))
                    dataPropertyInfo.SetValue(data, GConv.DbToDecNull(dr[tmpName]), null);
                else if (tmpType == typeof(DateTime))
                    dataPropertyInfo.SetValue(data, GConv.DbToDt(dr[tmpName]), null);
                else if (tmpType == typeof(DateTime?))
                    dataPropertyInfo.SetValue(data, GConv.DbToDtNull(dr[tmpName]), null);
                else if (tmpType == typeof(Guid))
                    dataPropertyInfo.SetValue(data, GConv.DbToGid(dr[tmpName]), null);
                else
                {
                    throw new Exception("ObjectFromDataRow type not supported!");
                }
            }
            return result;
        }
        #endregion
        #region // expando - datarow //
        public static dynamic ExpandoFromDataRow(DataRow dr)
        {
            var result = new ExpandoObject() as IDictionary<string, object>;
            foreach (DataColumn col in dr.Table.Columns)
            {
                result.Add(col.ColumnName, dr[col.ColumnName]);
            }
            return result;
        }
        #endregion
    }
}
