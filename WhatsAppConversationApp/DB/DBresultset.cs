using System;
using System.Data;
using System.Data.Common;

namespace CRM.Database
{
    public sealed class DBResultSet : IDisposable
    {
        public DbDataReader dr;
        public DataSet ds;
        public DataTable dt;

        //private int dtRowIndex;
        private int rowsAffected;

        ~DBResultSet()
        {
            Dispose();
        }

       
        public void Dispose()
        {
            if (dr != null)
            {
                
                dr.Close();
                dr = null;
            }

            if (dt != null)
            {
                dt.Clear();
                dt = null;
            }

            if (ds == null) return;
            ds.Clear();
            ds = null;
        }
        // ReSharper disable once ConvertToAutoProperty
        public int RowsAffected
        {
            get { return rowsAffected; }
            set { rowsAffected = value; }
        }        
        public string GetStringFromReader(string columnName)
        {
          
            string val = null;
            var index = dr.GetOrdinal(columnName);

            if (!dr.IsDBNull(index))
            {
                val = dr.GetString(index);
            }
            return val;
        }
        public double GetFloatFromReader(string columnName)
        {
            double val = 0;
            try
            {
                var index = dr.GetOrdinal(columnName);

                if (!dr.IsDBNull(index))
                {
                    return val = dr.GetDouble(index);
                }
            }
            catch (Exception Ex)
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                Ex.ToString();

            }
            return val;
        }
        public int GetIntegerFromReader(string columnName)
        {
            var val = 0;
            try
            {
                var index = dr.GetOrdinal(columnName);
                if (!dr.IsDBNull(index))
                {
                    val = dr.GetInt32(index);
                }
            }
            catch(Exception ex)
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
               ex.ToString();
            }
            return val;
        }

/*
        public object GetobjectFromReader(string colname)
        {
            object val = null;
            int index = dr.GetOrdinal(colname);
            if (!dr.IsDBNull(index))
            {
                val = dr.GetValue(index);
            }
            return val;
        }
*/


/*
        public bool NextRecordReader()
        {
            return dr.Read();

        }
*/

/*
        public bool NextRecordTable()
        {
            dtRowIndex++;

            if (dt.Rows.Count - 1 < dtRowIndex)
                return false;

            return true;

        }
*/
/*
        public string GetStringFromDatatable(string columnName)
        {
            return (string)dt.Rows[dtRowIndex][columnName];
        }
*/
/*
        public string GetStringFromDatatable(string columnName, int rowindex)
        {
            return (string)dt.Rows[rowindex][columnName];
        }
*/
/*
        public int GetIntegerFromDatatable(string columnName)
        {
            return (int)dt.Rows[dtRowIndex][columnName];
        }
*/
/*
        public int GetIntegerFromDatatable(string columnName, int rowindex)
        {
            return (int)dt.Rows[rowindex][columnName];
        }
*/

/*
        public object GetobjectFromDatatable(string columnName)
        {
            return dt.Rows[dtRowIndex][columnName];
        }
*/

        
    }
	   
}
 