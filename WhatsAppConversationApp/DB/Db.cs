using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace CRM.Database
{
    
    public class Db
    {
        DBConnection connection = null;
        string connectionString = "";
        public Db()
        {
            connection = DBFactory.GetDataAccessLayer(DataProviderType.Sql);
            if (ConfigurationManager.AppSettings.Get("environment") == "dev")
            {
                connectionString = ConfigurationManager.ConnectionStrings["development"].ConnectionString;
            }
            else if (ConfigurationManager.AppSettings.Get("environment") == "prod")
            {
                connectionString = ConfigurationManager.ConnectionStrings["development"].ConnectionString;
            }
            connectionString = ConfigurationManager.ConnectionStrings["development"].ConnectionString;
        }

        public DBConnection getConnection()
        {
                connection.OpenConnection(connectionString);
            if (connection.ConnectionState != System.Data.ConnectionState.Open)
            {
            }
            return connection;
        }

        //Returns sql output parameter(helper function)
        public static SqlParameter getOutParameter(string name, SqlDbType type)
        {
            SqlParameter p = new SqlParameter(name, type);
            p.Direction = ParameterDirection.Output;
            return p;
        }

        public static object getNamedValue(DbDataReader rdr, string name)
        {
            for (int n=0;n<rdr.FieldCount;n++)
            {
                if (rdr.GetName(n) == name)
                    return rdr.GetValue(n);
            }
            return new object();
        }
    }
}