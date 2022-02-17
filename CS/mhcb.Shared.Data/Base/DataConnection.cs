//===============================================================================
// DataConnection.cs
//
//  Object:	this is an universal data access/execution class for SqlDB and OLEDB.  
// 					It must inherit from DataConnection class.
// 					It provides different interfaces to access database with / without a return object.
// 					The return object could be DataSet or DataReader or None.
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
using System.Data.SqlClient;
using System.Data.OleDb;

namespace mhcb.cs.Shared.DataAccess
{

    //*********************************************
    // this class is for connection purpose. 
    // It can be used independent object.
    //*********************************************
    public abstract class DataConnection
    {
        //protected string DBType;

        //****************************************************************
        // this is abstract property, it must be implemented in sub class.
        // you can not change the way of access of the object, such as 'protected' 
        //****************************************************************
        protected abstract string DBType { get; set; }

        public DataConnection()
        {
            DBType = "SqlDb";
        }

        public DataConnection(string dbType)
        {
            DBType = dbType;    // for example : "SqlDb";
        }

        // public interface if you prefer to connect SQL server
        public SqlConnection SetSqlConnection()
        {
            var conn = new SqlConnection()
            {
                ConnectionString = getConnectionString(false)
            };
            return conn;
        }

        // public interface if you prefer to connect non-SQL server
        public OleDbConnection SetOleDbConnection()
        {
            var conn = new OleDbConnection()
            {
                ConnectionString = getConnectionString(false)
            };

            return conn;
        }


        // public interface if you prefer to get default SQL server connection
        protected string getDefaultConnectionString()
        {
            return getConnectionString(false);
        }


        //  if you would like to force to load setting from Configure file, call it with parameter 'false'
        protected string getConnectionString(Boolean blnIsLoading)
        {
            ServerSettingsHelper.ServerSettingsCache.IsLoading = blnIsLoading;

            // construct connection string by class field of ConnectionString
            var oServerSettings = ServerSettingsHelper.getSettings();

            switch (DBType)
            {
                case "SqlDb":
                    //return "data source=Mizweb01;initial catalog=pubs;integrated security=SSPI;persist security info=False";
                    return oServerSettings.getConnStringSQL;
                case "OleDb":
                    return oServerSettings.getConnStringOleDb;
                default:
                    return oServerSettings.getConnStringSQL;
            }
        }


    } //End DataConnection Class

}
