using CRM.Database;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace WhatsAppConversationApp.DataModel
{
    public class UserAuthenticationModel
    {
        public string tableName = "user_authenication";

        public UserAuthenticationModel() { }
        string error = " ";

        public int Login(string username, string pass)
        {

            List<SqlParameter> procedureParameters = new List<SqlParameter>(){
                new SqlParameter("@uname", username),
                new SqlParameter("@pass", pass)

            };
            try
            {
                var a = new Db().getConnection();

                DBResultSet r = new Db().getConnection().ExecuteReaderParameterized("proc_user_authentication_login", System.Data.CommandType.StoredProcedure, procedureParameters);
                DbDataReader _r =  r.dr;
     
                if (_r.Read())
                { return (int)Db.getNamedValue(_r, "user_authenication_id"); }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return -1;
        }
    }
}