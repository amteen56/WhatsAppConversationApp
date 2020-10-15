using System;

namespace CRM.Database
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DBFactory : DBFactoryBaseClass
    {
        public static DBConnection GetDataAccessLayer(DataProviderType dataProviderType)
        {
            switch (dataProviderType)
            {   
                case DataProviderType.Oracle:
                    return new OracleDataBase();
                case DataProviderType.Sql:
                    return new SqlDataBase();
                case DataProviderType.OleDb:
                    return new OleDbDataBase();
                default:
                    throw new ArgumentException("Invalid DAL provider type.");
            }
        }
    }

   
}