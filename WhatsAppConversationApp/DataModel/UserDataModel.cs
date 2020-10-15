using CRM.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WhatsAppConversationApp.Model;

namespace WhatsAppConversationApp.DataModel
{
    public class UserDataModel
    {
        public static string error;
        public int UpdateUser(User model)
        {
            List<SqlParameter> procedureParameters = new List<SqlParameter>(){
                new SqlParameter("@name", SqlDbType.VarChar){ Value = model.name},
                new SqlParameter("@uname", SqlDbType.VarChar){ Value = model.username },
                new SqlParameter("@pass", SqlDbType.VarChar){ Value = model.password},
                new SqlParameter("@id", SqlDbType.Int){ Value = model.user_id}

            };

            try
            {
                DBResultSet r = new Db().getConnection().ExecuteReaderParameterized("Update_User", System.Data.CommandType.StoredProcedure, procedureParameters);

                if (r.dr.Read())
                {
                    return r.dr.GetInt32(0);
                }

                return -1;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return -1;
            }
        }

        public int Crete_Usre(User model)
        {
            List<SqlParameter> procedureParameters = new List<SqlParameter>(){
                new SqlParameter("@name", SqlDbType.VarChar){ Value = model.name},
                new SqlParameter("@uname", SqlDbType.VarChar){ Value = model.username },
                new SqlParameter("@pass", SqlDbType.VarChar){ Value = model.password}

            };

            try
            {
                DBResultSet r = new Db().getConnection().ExecuteReaderParameterized("Create_User", System.Data.CommandType.StoredProcedure, procedureParameters);

                if (r.dr.Read())
                {
                    return r.dr.GetInt32(0);
                }

                return -1;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return -1;
            }
        }

        public int Delete_User(int id)
        {
            List<SqlParameter> procedureParameters = new List<SqlParameter>(){
                new SqlParameter("@id", SqlDbType.Int){ Value = id}
            };

            try
            {
                DBResultSet r = new Db().getConnection().ExecuteReaderParameterized("Delete_User", System.Data.CommandType.StoredProcedure, procedureParameters);

                if (r.dr.Read())
                {
                    return -1;
                }

                return 1;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return -1;
            }
        }
    }
}