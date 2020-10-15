using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Collections;
using System.Data.Common;

using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;

namespace CRM.Database
{
    public abstract class DBConnection
    {
        private IDbConnection connection;
        private IDbCommand command;
        //private IDbTransaction transaction;
        //private IDataReader reader;
        private IDataAdapter adapter;
        private string lastError;
        private int connectionNum = -1;

        public int ConnectionNumber
        {
            get { return connectionNum; }
            set { connectionNum = value; }
        }
/*
        protected string ConnectionString
        {
            set { lastError = value; }
        }
*/
        public string GetLastError
        {
            get
            {
                if (lastError == string.Empty || lastError.Length == 0)
                    return "";
                return lastError;
            }
            set { lastError = value; }
        }

        public ConnectionState ConnectionState
        {
            get { return connection.State == null ? ConnectionState.Closed : connection.State ; }
        }

        protected abstract IDbConnection GetDataProviderConnection();
        protected abstract IDbCommand GetDataProviderCommand();
/*
        public abstract IDbDataAdapter GetDataProviderDataAdapter();
*/
/*
        public abstract IDbDataAdapter GetDataProviderDataAdapter(string commandText, IDbConnection connObject);
*/
        protected abstract IDbDataAdapter GetDataProviderDataAdapter(IDbCommand connObject);
/*
        public abstract IDataReader GetDataProviderDataReader();
*/
        //public abstract IDbDataAdapter GetDataProviderDataAdapterwithSp(IDbCommand CommandObject);

        #region Database Transaction

        public bool OpenConnection(string pConnectionString)
        {
            var returnValue = false;
            //string Response = string.Empty;
            try
            {
                connection = GetDataProviderConnection(); // instantiate a connection object
                connection.ConnectionString = pConnectionString;
                connection.Open(); // open connection
                returnValue = true;
                lastError = connection.GetType().Name + " Open Successfully";
            }
            catch (Exception ex)
            {
                connection.Close();
                lastError = "Unable to Open " +" Ex caught "+ex+ connection.GetType().Name;
            }
            return returnValue;
        }
     
        //public bool CloseConnection()
        //{
        //    var returnValue = false;
        //    try
        //    {
        //        connection = GetDataProviderConnection(); // instantiate a connection object
        //        connection.Close();
        //        lastError = connection.GetType().Name + " Closed Successfully";
        //        returnValue = true;
        //    }
        //    catch
        //    {
        //        connection.Close();
        //        lastError = "Unable to Close " + connection.GetType().Name;
        //    }
        //     return returnValue;
        //}

        #region Old DB functions
        //public int ExecuteNonQuery()
        //{
        //    int returnValue = 0;
        //    try
        //    {                 
        //        command.Connection = connection;
        //        returnValue = command.ExecuteNonQuery();                
        //        command.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        lastError = ex.Message;
        //        //throw;
        //    }
            
        //    return returnValue;
        //}

        //public int ExecuteNonQuery(string query)
        //{
        //    int returnValue = 0;
        //    try
        //    {
        //        using (command = GetDataProviderCommand())
        //        {
        //            command.Connection = connection;
        //            command.CommandType = CommandType.Text;
        //            command.CommandText = query;
        //            returnValue = command.ExecuteNonQuery();
        //        }
        //        //command.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        lastError = ex.Message;
        //        //throw;
        //    }           
        //    return returnValue;
        //}
        //public Object ExecuteScalar(string query)
        //{
        //    //DBresultset result = new DBresultset();
        //    Object _obj = null;
        //    try
        //    {
        //        command = GetDataProviderCommand();
        //        command.Connection = connection;
        //        command.CommandType = CommandType.Text;
        //        command.CommandText = query;
        //        _obj = (object)command.ExecuteScalar();
        //        command.Dispose();

        //        //result.RowsAffected = 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        lastError = ex.Message;
        //        //throw;
        //    }
        //    return _obj;
        //}

        //public DBresultset ExecuteScalarResultSet(string query)
        //{
        //    DBresultset result = new DBresultset();
        //    try
        //    {
        //        command = GetDataProviderCommand();
        //        command.Connection = connection;
        //        command.CommandType = CommandType.Text;
        //        command.CommandText = query;
        //        result.obj = (object)command.ExecuteScalar();
        //        command.Dispose();

        //        //result.RowsAffected = 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        lastError = ex.Message;
        //        //throw;
        //    }

        //    return result;
        //}
        //public DBresultset ExecuteReaderwithparam(string spname, object[] parameters)
        //{
        //    DBresultset result = new DBresultset();
        //    try
        //    {

        //        using (command = GetDataProviderCommand())
        //        {
        //            //command = GetDataProviderCommand();
        //            command.Connection = connection;

        //            command.CommandType = CommandType.StoredProcedure;
        //            command.CommandText = spname;
        //            result.dr = command.ExecuteReader();

        //            //result.dr.Read();
        //            result.RowsAffected = result.dr.RecordsAffected;
        //            //command.Dispose();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        lastError = "DBConnection ::" + ex.Message;
        //        throw;
        //    }
        //    return result;
        //}
        #endregion
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public int ExecuteNonQuery(string commandText, CommandType commandType)
        {
            var returnValue = 0;
            lastError = "";

            try
            {
                using (connection)
                {
                    using (command = GetDataProviderCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = commandType;
                        command.CommandText = commandText;
                        returnValue = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
            }
            return returnValue;
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public int ExecuteNonQueryParameterized(string commandText, CommandType commandType, Dictionary<string, object> paramList)
        {
            var returnValue = 0;
            lastError = "";

            try
            {
                using (command = GetDataProviderCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = commandText;

                    foreach (var item in paramList)
                    {
                        var p = command.CreateParameter();
                        p.ParameterName = item.Key;
                        p.Value = item.Value ?? DBNull.Value;
                        command.Parameters.Add(p);
                    }
                    returnValue = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
            }

            return returnValue;
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public int ExecuteNonQueryParameterized(string commandText, CommandType commandType, object[] paramName, object[] paramValues)
        {
            var returnValue = 0;
            try
            {
                using (command = GetDataProviderCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = commandText;

                    var paramCount = paramName.Length;
                    for (var i = 0; i < paramCount; i++)
                    {
                        var p = command.CreateParameter();
                        p.ParameterName = (string)paramName[i];
                        p.Value = paramValues[i] ?? DBNull.Value;
                        command.Parameters.Add(p);
                    }

                    returnValue = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
            }

            return returnValue;
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public int ExecuteNonQueryParameterized(string commandText, CommandType commandType, Hashtable HtParamList)
        {
            var returnValue = 0;
            lastError = "";

            try
            {
                using (command = GetDataProviderCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = commandText;

                    foreach (string Key in HtParamList.Keys)
                    {
                        var p = command.CreateParameter();
                        p.Direction = ParameterDirection.Input;
                        p.ParameterName = Key;
                        p.Value = HtParamList[Key] ?? DBNull.Value;
                        command.Parameters.Add(p);
                    }

                    returnValue = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
            }

            return returnValue;
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public object ExecuteScalar(string commandText, CommandType commandType)
        {
            object obj = null;
            lastError = "";
            try
            {
                using (command = GetDataProviderCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = commandText;
                    obj = command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
            }
            return obj;
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public object ExecuteScalarParameterized(string commandText, CommandType commandType, Dictionary<string, object> paramList)
        {
            //int returnValue = 0;
            object returnValue = null;
            lastError = "";

            try
            {
                using (command = GetDataProviderCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = commandText;

                    foreach (var item in paramList)
                    {
                        var p = command.CreateParameter();
                        p.ParameterName = item.Key;
                        p.Value = item.Value ?? DBNull.Value;
                        command.Parameters.Add(p);
                    }
                    //returnValue = command.ExecuteNonQuery();
                    returnValue = command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
            }

            return returnValue;
        }
/*
        public object ExecuteScalarParameterized(string commandText, CommandType commandType, object[] paramName, object[] paramValues)
        {
            object obj = null;
            lastError = "";
            try
            {
                using (command = GetDataProviderCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = commandText;

                    int paramCount = paramName.Length;
                    for (int i = 0; i < paramCount; i++)
                    {
                        var p = command.CreateParameter();
                        p.ParameterName = (string)paramName[i];
                        p.Value = paramValues[i] ?? DBNull.Value;
                        command.Parameters.Add(p);
                    }

                    obj = command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
            }

            return obj;
        }
*/
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public object ExecuteScalarParameterized(string commandText, CommandType commandType, Hashtable HtParamList)
        {
            object obj = null;
            lastError = "";
            try
            {
                using (command = GetDataProviderCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = commandText;

                    foreach (string Key in HtParamList.Keys)
                    {
                        var p = command.CreateParameter();
                        p.Direction = ParameterDirection.Input;
                        p.ParameterName = Key;
                        p.Value = HtParamList[Key] ?? DBNull.Value;
                        command.Parameters.Add(p);
                    }

                    obj = command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
            }

            return obj;
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public DBResultSet ExecuteReader(string commandText, CommandType commandType)
        {
            var result = new DBResultSet();
            lastError = "";

            try
            {
                using (command = GetDataProviderCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = commandText;
                    result.dr = (DbDataReader)command.ExecuteReader();
                    //result.dr.Read();
                }
            }
            catch (Exception ex)
            {
                lastError = "DBConnection ::" + ex.Message;
                //throw;
            }
            return result;
        }


/*
        private DBresultset ExecuteReaderPa `rameterized(string commandText, CommandType commandType, object[] paramName, object[] paramValues)
        {
            DBresultset returnValue = new DBresultset();
            lastError = "";

            try
            {
                using (command = GetDataProviderCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = commandText;

                    int paramCount = paramName.Length;
                    for (int i = 0; i < paramCount; i++)
                    {
                        var p = command.CreateParameter();
                        p.ParameterName = (string)paramName[i];
                        p.Value = paramValues[i] ?? DBNull.Value;
                        command.Parameters.Add(p);
                    }
                    returnValue.dr = (DbDataReader)command.ExecuteReader();
                    returnValue.dr.Read();
                }
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
            }

            return returnValue;
        }
*/
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public DBResultSet ExecuteReaderParameterized(string commandText, CommandType commandType, Dictionary<string, object> paramList)
        {
            var returnValue = new DBResultSet();
            lastError = "";

            try
            {
                using (command = GetDataProviderCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = commandText;

                    foreach (var item in paramList)
                    {
                        var p = command.CreateParameter();
                        p.ParameterName = item.Key;
                        p.Value = item.Value ?? DBNull.Value;
                        command.Parameters.Add(p);
                    }

                    returnValue.dr = (DbDataReader)command.ExecuteReader();
                    returnValue.dr.Read();
                }
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
            }

            return returnValue;
        }

        public DBResultSet ExecuteReaderParameterized(string commandText, CommandType commandType, List<SqlParameter> paramList)
        {
            var returnValue = new DBResultSet();
            lastError = "";

            try
            {
                using (command = GetDataProviderCommand())
                {
                    command.Connection  = connection;
                    command.CommandType = commandType;
                    command.CommandText = commandText;

                 

                    foreach (SqlParameter p in paramList)
                    {
                        command.Parameters.Add(p);
                    }

                    IDataReader r = command.ExecuteReader();
                    returnValue.dr = (DbDataReader)r;
                    //returnValue.dr.Read();
                }
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
            }

            return returnValue;
        }

     
/*
        public DBresultset ExecuteReaderParameterized(string commandText, CommandType commandType, Hashtable HtParamList)
        {
            DBresultset returnValue = new DBresultset();
            lastError = "";
            try
            {
                using (command = GetDataProviderCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = commandText;

                    foreach (string Key in HtParamList.Keys)
                    {
                        var p = command.CreateParameter();
                        p.Direction = ParameterDirection.Input;
                        p.ParameterName = Key;
                        p.Value = HtParamList[Key] ?? DBNull.Value;
                        command.Parameters.Add(p);
                    }

                    returnValue.dr = (DbDataReader)command.ExecuteReader();
                    returnValue.dr.Read();
                }
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
            }

            return returnValue;
        }        
*/
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public DBResultSet ExecuteDataTable(string commandText, CommandType commandType)
        {
            var result = new DBResultSet();
            //{
            //    ds = new DataSet(),
            //    dt = new DataTable()
            //};
            lastError = "";

            try
            {

                using (command = GetDataProviderCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = commandText;
                    
                    adapter = GetDataProviderDataAdapter(command);

                    IDataReader r = command.ExecuteReader();
                    result.dr = (DbDataReader)r;
                }
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
            }

            return result;
        }
/*
        public DBresultset ExecuteDataTableParameterized(string commandText, CommandType commandType, object[] paramName, object[] paramValues)
        {
            DBresultset result = new DBresultset
            {
                ds = new DataSet(),
                dt = new DataTable()
            };

            lastError = "";

            try
            {
                using (command = GetDataProviderCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = commandText;

                    int paramCount = paramName.Length;
                    for (int i = 0; i < paramCount; i++)
                    {
                        var p = command.CreateParameter();
                        p.ParameterName = (string)paramName[i];
                        p.Value = paramValues[i] ?? DBNull.Value;
                        command.Parameters.Add(p);
                    }

                    adapter = GetDataProviderDataAdapter(command);
                    adapter.Fill(result.ds);
                    result.dt = result.ds.Tables[0];
                    result.RowsAffected = result.dt.Rows.Count;
                    

                }
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
            }

            return result;
           
        }
*/
/*
        public DBresultset ExecuteDataTableParameterized(string commandText, CommandType commandType, Dictionary<string, object> paramList)
        {
            DBresultset result = new DBresultset
            {
                ds = new DataSet(),
                dt = new DataTable()
            };

            lastError = "";

            try
            {
                using (command = GetDataProviderCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = commandText;

                    foreach (var item in paramList)
                    {
                        var p = command.CreateParameter();
                        p.ParameterName = item.Key;
                        p.Value = item.Value ?? DBNull.Value;
                        command.Parameters.Add(p);
                    }

                    adapter = GetDataProviderDataAdapter(command);
                    adapter.Fill(result.ds);
                    result.dt = result.ds.Tables[0];
                    result.RowsAffected = result.dt.Rows.Count;
                    //result.dtRowIndex = 0;
                }
                
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
            }

            return result;
        }
*/
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public DBResultSet ExecuteDataTableParameterized(string commandText, CommandType commandType, Hashtable HtParamList)
        {
            var result = new DBResultSet
            {
                ds = new DataSet(),
                dt = new DataTable()
            };

            lastError = "";

            try
            {
                using (command = GetDataProviderCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = commandText;

                    foreach (string Key in HtParamList.Keys)
                    {
                        var p = command.CreateParameter();
                        p.Direction = ParameterDirection.Input;
                        p.ParameterName = Key;
                        p.Value = HtParamList[Key] ?? DBNull.Value;
                        command.Parameters.Add(p);
                    }

                    adapter = GetDataProviderDataAdapter(command);
                    adapter.Fill(result.ds);
                    result.dt = result.ds.Tables[0];
                    result.RowsAffected = result.dt.Rows.Count;
                    //result.dtRowIndex = 0;
                }

            }
            catch (Exception ex)
            {
                lastError = ex.Message;
            }

            return result;
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public DBResultSet GetDataTableFromSP(string spname, string[] param_name, object[] param_values)
        {
            var result = new DBResultSet
            {
                ds = new DataSet(),
                dt = new DataTable()
            };

            try
            {
                 command = GetDataProviderCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = spname;
                
                var paramCount = param_name.Length;

                if (param_name.Length > 0 && param_values.Length > 0)
                {


                    for (var i = 0; i < paramCount; i++)
                    {
                        var param = command.CreateParameter();
                        if (param_values[i] == null)
                        {
                            param_values[i] = DBNull.Value;
                        }
                        param.ParameterName = param_name[i];
                        param.Value = param_values[i];
                        command.Parameters.Add(param);
                    }
                }
                command.Connection = connection;
                result.dr = (DbDataReader)command.ExecuteReader();
                result.dr.Read();
                result.RowsAffected = result.dr.FieldCount;
                //using (command = GetDataProviderCommand())
                //{
                //    command.Connection = connection;
                //    command.CommandType = CommandType.Text;
                //    command.CommandText = query;
                //    result.dr = command.ExecuteReader();
                //    result.dr.Read();
                //    result.RowsAffected = result.dr.FieldCount;

                //}
            }
            catch (Exception ex)
            {
                lastError = "DBConnection ::" + ex.Message;
                //throw;
            }
            return result;
        }
/*
        public DBresultset PerformTransaction(string spname, string[] param_name, object[] param_values)
        {
            DBresultset result = new DBresultset
            {
                ds = new DataSet(),
                dt = new DataTable()
            };

            IDbTransaction dbTxn = null;

            try
            {
                dbTxn = connection.BeginTransaction();

                // TO DO
                // Perform DB extensive task

                dbTxn.Commit();
            }
            catch (Exception ex)
            {
                lastError = "DBConnection ::" + ex.Message;
                //throw;

                if (dbTxn != null) dbTxn.Rollback();
            }
            return result;
        }       
*/
        
        #endregion

        
    }

    public class OleDbDataBase : DBConnection
    {
        // Provide class constructors

/*
        public OleDbDataBase(string connectionString)
        {
            ConnectionString = connectionString;
        }
*/
        // DBBaseClass Members
        protected override IDbConnection GetDataProviderConnection()
        {
            return new OleDbConnection();
        }
        protected override IDbCommand GetDataProviderCommand()
        {
            return new OleDbCommand();
        }
        //public override IDbDataAdapter GetDataProviderDataAdapter()
        //{
        //    return new OleDbDataAdapter();
        //}
        //public override IDbDataAdapter GetDataProviderDataAdapter(string commandText, IDbConnection connObject)
        //{
        //    return new OleDbDataAdapter(commandText, (OleDbConnection)connObject);
        //}
        protected override IDbDataAdapter GetDataProviderDataAdapter(IDbCommand commandObject)
        {
            return new OleDbDataAdapter((OleDbCommand)commandObject);
        }        
        //public override IDataReader GetDataProviderDataReader()
        //{
        //    using (GetDataProviderConnection())
        //    {
        //        using (GetDataProviderCommand())
        //        {
        //            return null;
        //        }
        //    }
        //}

    }

    public class SqlDataBase : DBConnection
    {
        // Provide class constructors

/*
        public SqlDataBase(string connectionString)
        {
            ConnectionString = connectionString;
        }
*/

        // DBBaseClass Members
        protected override IDbConnection GetDataProviderConnection()
        {
            return new SqlConnection();
        }

        protected override IDbCommand GetDataProviderCommand()
        {
            return new SqlCommand();
        }
        //public override IDbDataAdapter GetDataProviderDataAdapter()
        //{
        //    return new SqlDataAdapter();
        //}

        //public override IDbDataAdapter GetDataProviderDataAdapter(string commandText, IDbConnection connObject)
        //{
        //    return new SqlDataAdapter(commandText, (SqlConnection)connObject);
        //}
        protected override IDbDataAdapter GetDataProviderDataAdapter(IDbCommand commandObject)
        {
            return new SqlDataAdapter((SqlCommand)commandObject);
        }


        //public override IDataReader GetDataProviderDataReader()
        //{
        //    return null;
        //}
    }

    public class OracleDataBase : DBConnection
    {
        // Provide class constructors

/*
        public OracleDataBase(string connectionString)
        {
            ConnectionString = connectionString;
        }
*/

        // DALBaseClass Members
        protected override IDbConnection GetDataProviderConnection()
        {
#pragma warning disable 618
            return new OracleConnection();
#pragma warning restore 618
        }
        protected override IDbCommand GetDataProviderCommand()
        {
#pragma warning disable 618
            return new OracleCommand();
#pragma warning restore 618
        }

//        public override IDbDataAdapter GetDataProviderDataAdapter()
//        {
//#pragma warning disable 618
//            return new OracleDataAdapter();
//#pragma warning restore 618
//        }

//        public override IDbDataAdapter GetDataProviderDataAdapter(string commandText, IDbConnection connObject)
//        {
//#pragma warning disable 618
//            return new OracleDataAdapter(commandText, (OracleConnection)connObject);
//#pragma warning restore 618
//        }

        protected override IDbDataAdapter GetDataProviderDataAdapter(IDbCommand commandObject)
        {
#pragma warning disable 618
            return new OracleDataAdapter((OracleCommand)commandObject);
#pragma warning restore 618
        }

        //public override IDataReader GetDataProviderDataReader()
        //{
        //    using (GetDataProviderConnection())
        //    {
        //        using (GetDataProviderCommand())
        //        {
        //            return null;
        //        }
        //    }
        //}


       
    }
}
