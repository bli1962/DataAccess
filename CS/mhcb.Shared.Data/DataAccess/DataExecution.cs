//===============================================================================
// DataExcution.cs
//
// Object:	this is an universal data access/execution class for both SqlDB and OLEDB.  
//					It must inherit from DataConnection class.
//					It provides different interfaces to access database with / without a return object.
//					The return object could be DataSet or DataReader or None.
//
// Author: Ben Li
// Date Created:  12/05/2005
// Last Modified: 25/10/2014
// 
//===============================================================================
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
//==============================================================================

using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace mhcb.cs.Shared.DataAccess
{

    /// Summary description for ReturnICollection.
    public class DataExecution : DataConnection, IDataExecution, IDisposable
    {
        #region ** construction section **
        public DataExecution()
        {
            DBType = "SqlDb";                   // default data provider
        }


        public DataExecution(string dbType)
        {
            DBType = dbType;
        }
        #endregion


        //****************************************************************
        // implement abstract methods / properties in Abstract class
        // But you cannot change the way of access of the object, such as 'protected' 
        //****************************************************************
        protected override string DBType { get; set; }

        // *****************************************************************
        // implement common methods in Abstract class
        // *****************************************************************
        public new string getDefaultConnectionString()
        {
            // you have an option to implement by your way here, or by base  
            return base.getDefaultConnectionString();
        }

        // *****************************************************************
        // implement common methods in Abstract class
        // *****************************************************************
        public new string getConnectionString(Boolean blnIsLoading)
        {
            // you have an option to implement by your way here, or by base  
            return base.getConnectionString(blnIsLoading);
        }


        #region ** "ExecuteScalar" **
        public object ExecuteScalar(SqlCommand cmd)
        {
            object objRtn;
            try
            {
                using (SqlConnection conn = getSqlConnection())
                {
                    cmd.Connection = conn;
                    objRtn = cmd.ExecuteScalar();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return objRtn;
        }


        public object ExecuteScalar(SqlCommand cmd, SqlConnection conn)
        {
            object objRtn;
            try
            {
                cmd.Connection = conn;
                objRtn = cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw e;
            }
            return objRtn;

        }
        #endregion


        #region ** "ExecuteNonQuery" **

        private SqlConnection getSqlConnection()
        {
            //** declare connection string
            SqlConnection conn;
            try
            {
                conn = SetSqlConnection();
                conn.Open();
            }
            catch
            {
                //EntryToEventLog olog = new EntryToEventLog(ConfigurationSettings.AppSettings["EventLogSource"], ConfigurationSettings.AppSettings["EventLogName"]);
                //olog.WriteAEventLog(e.Message.ToString(), EventLogEntryType.Error);
                //TraceMaker.TraceException(e);
                conn = null;
            }
            return conn;
        }

        public void ExecuteNonQuery(SqlCommand cmd)
        {
            try
            {
                using (SqlConnection conn = getSqlConnection())
                {
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public void ExecuteNonQuery(SqlCommand cmd, SqlConnection conn)
        {
            try
            {
                //SqlExecuteNonQuery(cmd, conn); 
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion


        #region ** "ExecuteDataReader - SqlString" **
        public SqlDataReader ExecuteDataReader(string sqlString)
        {
            SqlDataReader dr;
            try
            {
                using (SqlConnection conn = getSqlConnection())
                {                   
                    var cmd = new SqlCommand(sqlString)
                    {
                        Connection = conn
                    };
                    dr = cmd.ExecuteReader(CommandBehavior.Default);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return dr;
        }


        public SqlDataReader ExecuteDataReader(string sqlString, SqlConnection conn)
        {
            SqlDataReader dr;
            try
            {
                var cmd = new SqlCommand(sqlString)
                {
                    Connection = conn
                };
                dr = cmd.ExecuteReader(CommandBehavior.Default);
            }
            catch (Exception e)
            {
                throw e;
            }

            return dr;
        }


        //private SqlDataReader SqlExecuteReader(string SqlString, SqlConnection conn) 
        //{ 
        //    SqlDataReader dr = null; 
        //    SqlCommand cmd = new SqlCommand(SqlString); 
        //    try 
        //    { 
        //        cmd.Connection = conn; 
        //        dr = cmd.ExecuteReader(System.Data.CommandBehavior.Default); 
        //    } 
        //    catch (Exception e) 
        //    { 
        //        dr = null; 
        //        throw e; 
        //    } 
        //    finally 
        //    { 
        //        cmd = null; 
        //        conn = null; 
        //    } 
        //    return dr; 
        //}
        #endregion


        #region ** "ExecuteDataReader - by command" **

        public SqlDataReader ExecuteDataReader(SqlCommand cmd)
        {
            SqlDataReader dr;
            try
            {
                using (SqlConnection conn = getSqlConnection())
                {
                    dr = SqlExecuteReader(cmd, conn);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return dr;
        }


        public SqlDataReader ExecuteDataReader(SqlCommand cmd, SqlConnection conn)
        {
            SqlDataReader dr;
            try
            {
                dr = SqlExecuteReader(cmd, conn);
            }
            catch (Exception e)
            {
                throw e;
            }

            return dr;
        }


        private SqlDataReader SqlExecuteReader(SqlCommand cmd, SqlConnection conn)
        {
            SqlDataReader dr;
            try
            {
                cmd.Connection = conn;
                dr = cmd.ExecuteReader(CommandBehavior.Default);
            }
            catch (Exception e)
            {
                throw e;
            }

            return dr;
        }

        #endregion


        #region ** "ExecuteDataSet" **

        public DataSet ExecuteDataSet(SqlCommand cmd)
        {
            DataSet ds;
            try
            {
                using (SqlConnection conn = getSqlConnection())
                {
                    ds = SqlExecuteDataSet(cmd, conn, "");
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return ds;
        }

        public DataSet ExecuteDataSet(SqlCommand cmd, SqlConnection conn)
        {
            DataSet ds;
            try
            {
                ds = SqlExecuteDataSet(cmd, conn, "");
            }
            catch (Exception e)
            {
                throw e;
            }

            return ds;
        }

        public DataSet ExecuteDataSet(SqlCommand cmd, string strTable)
        {
            var ds = new DataSet();

            try
            {
                using (SqlConnection conn = getSqlConnection())
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                        ds = SqlExecuteDataSet(cmd, conn, strTable);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return ds;
        }

        public DataSet ExecuteDataSet(SqlCommand cmd, SqlConnection conn, string strTable)
        {
            DataSet ds;
            try
            {
                ds = SqlExecuteDataSet(cmd, conn, strTable);
            }
            catch (Exception e)
            {
                throw e;
            }

            return ds;
        }

        private DataSet SqlExecuteDataSet(SqlCommand cmd, SqlConnection conn, string strTable)
        {
            DataSet ds = new DataSet();
            try
            {
                var da = new SqlDataAdapter(cmd);
                cmd.Connection = conn;
                if (strTable == "")
                {
                    da.Fill(ds);
                }
                else
                {
                    da.Fill(ds, strTable);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return ds;
        }

        #endregion

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        //		#region //** SQL members section
        //
        //		public SqlDataReader ExecuteDataReader(SqlCommand cmd)
        //		{
        //			return SqlExecuteReader(cmd);
        //		}
        //
        //		
        //		public void ExecuteNonQuery(SqlCommand cmd)
        //		{
        //			SqlExecuteNonQuery(cmd);
        //		}
        //
        //		
        //		public SqlDataReader ExecuteDataReader(string SqlString)
        //		{
        //			return SqlExecuteReader(SqlString);
        //		}
        //
        //		
        //		public DataSet ExecuteDataSet(SqlCommand cmd)
        //		{
        //			DataSet ds =  new DataSet();
        //			SqlDataAdapter da =  new SqlDataAdapter(cmd);
        //
        //			try
        //			{
        //				//TraceMaker.TraceStart();
        //				cmd.Connection = SetSqlConnection();
        //				da.Fill(ds);
        //			}
        //			catch (Exception e)
        //			{
        //				//EntryToEventLog olog = new EntryToEventLog(ConfigurationSettings.AppSettings["EventLogSource"], ConfigurationSettings.AppSettings["EventLogName"]);
        //				//olog.WriteAEventLog(e.Message.ToString(), EventLogEntryType.Error);
        //				//TraceMaker.TraceException(e);
        //				ds = null;
        //				throw;
        //			}
        //			finally 
        //			{
        //				da = null;
        //				cmd = null;
        //				//TraceMaker.TraceEnd();
        //			}
        //			
        //			return ds;
        //		}
        //
        //	
        //		public DataSet ExecuteDataSet(SqlCommand cmd, string strTable)
        //		{
        //			DataSet ds =  new DataSet();
        //			SqlDataAdapter da =  new SqlDataAdapter(cmd);
        //
        //			try
        //			{
        //				//TraceMaker.TraceStart();
        //				cmd.Connection = SetSqlConnection();
        //				da.Fill(ds, strTable);
        //			}
        //			catch (Exception e)
        //			{
        //				//EntryToEventLog olog = new EntryToEventLog(ConfigurationSettings.AppSettings["EventLogSource"], ConfigurationSettings.AppSettings["EventLogName"]);
        //				//olog.WriteAEventLog(e.Message.ToString(), EventLogEntryType.Error);
        //				//TraceMaker.TraceException(e);
        //				ds = null;
        //				throw;
        //			}
        //			finally 
        //			{
        //				da = null;
        //				cmd = null;
        //				//TraceMaker.TraceEnd();
        //			}
        //			
        //			return ds;
        //		}
        //
        //	
        //		public SqlDataReader SqlExecuteReader (SqlCommand cmd) 
        //		{
        //			SqlConnection conn = new SqlConnection();
        //			SqlDataReader dr = null;
        //			
        //			try
        //			{
        //				//TraceMaker.TraceStart();
        //				conn = SetSqlConnection();
        //				conn.Open();
        //				cmd.Connection = conn;
        //				dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        //			}
        //			catch (Exception e)
        //			{
        //				//EntryToEventLog olog = new EntryToEventLog(ConfigurationSettings.AppSettings["EventLogSource"], ConfigurationSettings.AppSettings["EventLogName"]);
        //				//olog.WriteAEventLog(e.Message.ToString(), EventLogEntryType.Error);
        //				//TraceMaker.TraceException(e);
        //				dr = null;
        //				throw;
        //			}
        //			finally 
        //			{
        //				cmd = null;
        //				conn = null;
        //				//TraceMaker.TraceEnd();
        //			}
        //			return dr;
        //		}
        //
        //		
        //		public SqlDataReader SqlExecuteReader (string SqlString) 
        //		{
        //			SqlConnection conn = new SqlConnection();
        //			SqlDataReader dr = null;
        //			SqlCommand cmd = new SqlCommand(SqlString);
        //
        //			try
        //			{
        //				//TraceMaker.TraceStart();
        //				conn = SetSqlConnection();
        //				conn.Open();
        //				cmd.Connection = conn;
        //				dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        //			}
        //			catch (Exception e)
        //			{
        //				//EntryToEventLog olog = new EntryToEventLog(ConfigurationSettings.AppSettings["EventLogSource"], ConfigurationSettings.AppSettings["EventLogName"]);
        //				//olog.WriteAEventLog(e.Message.ToString(), EventLogEntryType.Error);
        //				//TraceMaker.TraceException(e);
        //				dr = null;
        //				throw;
        //			}
        //			finally 
        //			{
        //				cmd = null;
        //				conn = null;
        //				//TraceMaker.TraceEnd();
        //			}
        //			return dr;
        //		}
        //		
        //		
        //		public void SqlExecuteNonQuery (SqlCommand cmd) 
        //		{
        //			SqlConnection conn = new SqlConnection();
        //
        //			try
        //			{
        //				//TraceMaker.TraceStart();
        //				conn = SetSqlConnection();
        //				conn.Open();
        //
        //				cmd.Connection = conn;
        //				cmd.ExecuteNonQuery();
        //			}
        //			catch (Exception e)
        //			{
        //				//EntryToEventLog olog = new EntryToEventLog(ConfigurationSettings.AppSettings["EventLogSource"], ConfigurationSettings.AppSettings["EventLogName"]);
        //				//olog.WriteAEventLog(e.Message.ToString(), EventLogEntryType.Error);
        //				//TraceMaker.TraceException(e);
        //				throw;
        //			}
        //			finally 
        //			{
        //				cmd = null;
        //				conn = null;
        //				//TraceMaker.TraceEnd();
        //			}
        //		}
        //
        //		//		public string ExecuteSqlXmlSelect(string CustomerID, string
        //		//			ConnectionString, bool ClientSide)
        //		//		{
        //		//			SqlXmlCommand cmd = new SqlXmlCommand(ConnectionString);
        //		//			cmd.RootTag = "Customers";
        //		//			cmd.ClientSideXml = ClientSide;
        //		//			cmd.CommandText = "SELECT * FROM Customers WHERE CustomerID =	'" + CustomerID + "' FOR XML RAW";
        //		//			XmlReader xr = cmd.ExecuteXmlReader();
        //		//
        //		//			// 
        //		//			XmlDocument xd = new XmlDocument();
        //		//			// 
        //		//			xd.Load(xr);
        //		//			return xd.OuterXml;
        //		//		}
        //
        //		#endregion

        //**********************

        #region //** OleDb members section

        public OleDbDataReader ExecuteDataReader(OleDbCommand cmd)
        {
            return OleDbExecuteReader(cmd);
        }

        public void ExecuteNonQuery(OleDbCommand cmd)
        {
            OleDbExecuteNonQuery(cmd);
        }

        //				public OleDbDataReader ExecuteDataReader(string SqlString)
        //				{
        //					return OleDbExecuteReader(SqlString);
        //				}

        public DataSet ExecuteDataSet(OleDbCommand cmd)
        {
            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);

            try
            {
                //TraceMaker.TraceStart();
                cmd.Connection = SetOleDbConnection();
                da.Fill(ds);
            }
            catch (Exception e)
            {
                //EntryToEventLog olog = new EntryToEventLog(ConfigurationSettings.AppSettings["EventLogSource"], ConfigurationSettings.AppSettings["EventLogName"]);
                //olog.WriteAEventLog(e.Message.ToString(), EventLogEntryType.Error);
                //TraceMaker.TraceException(e);
                throw;
            }
            finally
            {
                cmd = null;
                da = null;
                //TraceMaker.TraceEnd();
            }
            return ds;
        }


        public DataSet ExecuteDataSet(OleDbCommand cmd, string strTable)
        {
            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);

            try
            {
                //TraceMaker.TraceStart();
                cmd.Connection = SetOleDbConnection();
                da.Fill(ds, strTable);
            }
            catch (Exception e)
            {
                //EntryToEventLog olog = new EntryToEventLog(ConfigurationSettings.AppSettings["EventLogSource"], ConfigurationSettings.AppSettings["EventLogName"]);
                //olog.WriteAEventLog(e.Message.ToString(), EventLogEntryType.Error);
                //TraceMaker.TraceException(e);
                throw;
            }
            finally
            {
                cmd = null;
                da = null;
                //TraceMaker.TraceEnd();
            }
            return ds;
        }


        public void OleDbExecuteNonQuery(OleDbCommand cmd)
        {
            OleDbConnection conn = new OleDbConnection();

            try
            {
                //TraceMaker.TraceStart();
                conn = SetOleDbConnection();
                conn.Open();

                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                //EntryToEventLog olog = new EntryToEventLog(ConfigurationSettings.AppSettings["EventLogSource"], ConfigurationSettings.AppSettings["EventLogName"]);
                //olog.WriteAEventLog(e.Message.ToString(), EventLogEntryType.Error);
                //TraceMaker.TraceException(e);
                throw;
            }
            finally
            {
                cmd = null;
                conn = null;
                //TraceMaker.TraceEnd();
            }
        }


        public OleDbDataReader OleDbExecuteReader(OleDbCommand cmd)
        {
            OleDbConnection conn = new OleDbConnection();
            OleDbDataReader dr = null;
            try
            {
                //TraceMaker.TraceStart();
                conn = SetOleDbConnection();
                conn.Open();

                cmd.Connection = conn;
                dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                //EntryToEventLog olog = new EntryToEventLog(ConfigurationSettings.AppSettings["EventLogSource"], ConfigurationSettings.AppSettings["EventLogName"]);
                //olog.WriteAEventLog(e.Message.ToString(), EventLogEntryType.Error);
                //TraceMaker.TraceException(e);
                dr = null;
                throw;
            }
            finally
            {
                cmd = null;
                conn = null;
                //TraceMaker.TraceEnd();
            }
            return dr;
        }


        public OleDbDataReader OleDbExecuteReader(string sqlString)
        {
            OleDbConnection conn = new OleDbConnection();
            OleDbCommand cmd = new OleDbCommand(sqlString);
            OleDbDataReader dr = null;

            try
            {
                //TraceMaker.TraceStart();
                conn = SetOleDbConnection();
                conn.Open();
                cmd.Connection = conn;
                dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                //EntryToEventLog olog = new EntryToEventLog(ConfigurationSettings.AppSettings["EventLogSource"], ConfigurationSettings.AppSettings["EventLogName"]);
                //olog.WriteAEventLog(e.Message.ToString(), EventLogEntryType.Error);
                //TraceMaker.TraceException(e);
                dr = null;
                throw;
            }
            finally
            {
                cmd = null;
                conn = null;
                //TraceMaker.TraceEnd();
            }
            return dr;
        }


        //		public OleDbDataReader GetOleDbDataReader () 
        //		{
        //			OleDbConnection conn = SetOleDbConnection();
        //			conn.Open();
        //			OleDbCommand comm = new OleDbCommand(SqlString,conn);
        //			OleDbDataReader dr;
        //			dr = comm.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        //			return dr;
        //		}
        //	} //End ReturnDataReader Class

        #endregion



        //	public class ReturnICollection : ReturnDataReader
        //	{
        //		public string SortExpression;
        //		
        //		public ICollection GetCollection()
        //		{
        //			if(ReaderType=="OleDb")
        //			{
        //				return GetOleDbCollection();}
        //			else
        //			{
        //				return GetSqlCollection();}
        //		}

        //		public ICollection GetSqlCollection() 
        //		{
        //			SqlConnection conn = SetSqlConnection();
        //			conn.Open();
        //			SqlDataAdapter dbadaptor = new SqlDataAdapter(SqlString,conn);
        //			DataSet ds = new DataSet();
        //			dbadaptor.Fill(ds,"DataResult");
        //			DataView dv = ds.Tables["DataResult"].DefaultView;
        //			dv.Sort=SortExpression;
        //
        //			return dv;
        //		}

        //		public ICollection GetOleDbCollection () 
        //		{
        //			OleDbConnection conn = SetOleDbConnection();
        //			conn.Open();
        //			OleDbDataAdapter dbadaptor = new OleDbDataAdapter(SqlString,conn);
        //			DataSet ds = new DataSet();
        //			dbadaptor.Fill(ds,"DataResult");
        //			DataView dv = ds.Tables["DataResult"].DefaultView;
        //			dv.Sort=SortExpression;
        //			return dv;
        //		}


    } //DataExcution.cs

}
