
using System;
using System.Configuration;
using System.Threading;

namespace CRM.Database
{
    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public  class DBConnectionPool:IDisposable
    {
        public static readonly string ApplogLevel = "1";
        private readonly int m_PoolSize;
        private readonly string m_ConnectionString;
        private readonly DataProviderType m_dbType;
        private string m_LastError = "";

        private readonly DBConnection[] dbConnPool;
        private readonly bool[] isConnFree;
        private int freeConnectionsCount;
        private readonly Mutex mConnPool;

        public string GetLastError
        {
            get { return m_LastError; }
            
        }

        public DBConnectionPool(DataProviderType dbType, string connString, int poolSize)
        {
            m_PoolSize = poolSize;
            m_ConnectionString = connString;
            m_dbType = dbType;

            mConnPool = new Mutex(false, "DBConnPool");
            dbConnPool = new DBConnection[m_PoolSize];
            isConnFree = new bool[m_PoolSize];

            //Initialize(dbType, connString);
        }

        ~DBConnectionPool()
        {
            Dispose(false);
           
        }

        public bool Initialize()
        {
          //  new DBFactory();
            var bReturnValue = true;
            
            try
            {
                //mConnPool.WaitOne();

                for (var i = 0; i < m_PoolSize; i++)
                {
                    dbConnPool[i] = DBFactory.GetDataAccessLayer(m_dbType);
                    m_LastError = "Decrypting the Connection string";
                    string output = Utility.Decrypt(m_ConnectionString);
                    m_LastError = "connection string decrypted Successfully";
                    if (dbConnPool[i].OpenConnection(output))
                    {
                        isConnFree[i] = true;
                        freeConnectionsCount++;
                        //Logger.LogMessage(logObject, Thread.CurrentThread.Name, LogLevel.INFO, dbConnPool[i].GetLastError, DBConnectionPool.ApplogLevel);
                    }
                    else
                    {
                        m_LastError = dbConnPool[i].GetLastError;
                        isConnFree[i] = false;
                        dbConnPool[i] = null;
                        bReturnValue = false;
                        //Logger.LogMessage(logObject, Thread.CurrentThread.Name, LogLevel.ERROR, dbConnPool[i].GetLastError, DBConnectionPool.ApplogLevel);
                    }
                }
            }
            catch (Exception ex)
            {
                m_LastError = ex.Message;
                //Logger.LogMessage(logObject, Thread.CurrentThread.Name, LogLevel.ERROR, m_LastError, DBConnectionPool.ApplogLevel);
                bReturnValue= false;
            }

            //mConnPool.ReleaseMutex();

            return bReturnValue;
        }

        public DBConnection GetConnectionWait()
        {
            DBConnection obj = null;

            try
            {
                while (freeConnectionsCount <= 0)
                    Thread.Sleep(100);

            while ((obj = GetConnection()) == null)
            {
                Thread.Sleep(100);                
                }
            }
            catch (Exception ex)
            {
                m_LastError = ex.Message;
                //Logger.LogMessage(logObject, Thread.CurrentThread.Name, LogLevel.ERROR, m_LastError);
            }

            return obj;
        }

        private DBConnection GetConnection()
        {
           
            try
            {
                mConnPool.WaitOne();
                for (var i = 0; i < m_PoolSize; i++)
                {
                    if (!isConnFree[i]) continue;
                    //Logger.LogMessage(logObject, Thread.CurrentThread.Name, LogLevel.INFO, "connection going for use "+i);
                    isConnFree[i] = false;
                    freeConnectionsCount--;
                    var dbConn = dbConnPool[i];
                    dbConn.ConnectionNumber = i+1;
                    //Logger.LogMessage(logObject, Thread.CurrentThread.Name, LogLevel.INFO, "ReleaseMutex ");
                    mConnPool.ReleaseMutex();
                    return dbConn;
                }
                mConnPool.ReleaseMutex();
            }
            catch (ApplicationException ex)
            {
                mConnPool.ReleaseMutex();
                m_LastError = ex.Message;
                //Logger.LogMessage(logObject, Thread.CurrentThread.Name, LogLevel.FATAL, m_LastError);
            }

           catch(Exception ex)
            {
                mConnPool.ReleaseMutex();
                m_LastError = ex.Message;
                //Logger.LogMessage(logObject, Thread.CurrentThread.Name, LogLevel.FATAL, m_LastError);
            }          
            return null;
        }

        public void PutBack(DBConnection dbConn)
        {
            try
            {
                mConnPool.WaitOne();
                for (var i = 0; i < m_PoolSize; i++)
                {
                    if (dbConnPool[i] != dbConn) continue;
                    if (dbConn.ConnectionState != System.Data.ConnectionState.Open)
                    {
                        dbConnPool[i].OpenConnection(m_ConnectionString);
                    }

                    //dbConn.ConnectionNumber = -1;
                    dbConnPool[i] = dbConn;
                    freeConnectionsCount++;
                    isConnFree[i] = true;
                    mConnPool.ReleaseMutex();
                    return;
                }
                mConnPool.ReleaseMutex();
            }

            catch (Exception ex)
            {
                mConnPool.ReleaseMutex();
                m_LastError = ex.Message;
                //Logger.LogMessage(logObject, Thread.CurrentThread.Name, LogLevel.FATAL, m_LastError);
            }
        }

        private static void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        private void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (!disposing) return;
            if (mConnPool != null) mConnPool.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            
            GC.SuppressFinalize(this);
        }
    }

/*
    public class DBSmartConnection
    {
        readonly DBConnectionPool mPool = null;
        readonly DBConnection dbConn = null;

        public DBSmartConnection(DBConnectionPool _pool, ref DBConnection _conn)
        {
            mPool = _pool;
            _conn = mPool.GetConnectionWait();
            //return dbConn;
        }

        ~DBSmartConnection()
        {
            mPool.PutBack(dbConn);
        }
    }
*/
}
