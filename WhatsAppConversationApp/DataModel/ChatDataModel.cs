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
    public class ChatDataModel
    {
        string error = "";
        public int Chat(Chat model)
        {
            List<SqlParameter> procedureParameters = new List<SqlParameter>(){
                new SqlParameter("@userid", SqlDbType.Int){ Value = model.user_id},
                new SqlParameter("@message_content", SqlDbType.NVarChar){ Value = model.message_content },
                new SqlParameter("@number", SqlDbType.VarChar){ Value = model.number},
                new SqlParameter("@reciver_name", SqlDbType.VarChar){ Value = model.reciever_name},
                new SqlParameter("@msgTypeID", SqlDbType.Int){ Value = 1},
                new SqlParameter("@statusID", SqlDbType.Int){ Value = 1}
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

        public int Delete_User_Chat(int userid, int wa_chat_id)
        {
            List<SqlParameter> procedureParameters = new List<SqlParameter>(){
                new SqlParameter("@userid", SqlDbType.Int){ Value = userid},
                new SqlParameter("@chatid", SqlDbType.Int){ Value = wa_chat_id}
            };

            try
            {
                new Db().getConnection().ExecuteReaderParameterized("Delete_user_Chat", System.Data.CommandType.StoredProcedure, procedureParameters);

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