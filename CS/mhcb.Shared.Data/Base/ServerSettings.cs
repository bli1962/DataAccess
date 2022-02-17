//===============================================================================
// ServerSettings.cs
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
using System.Text;
using System.Configuration;

namespace mhcb.cs.Shared
{

    //**************************************************************
    //  public struct
    //**************************************************************
    /// Summary description for ServerSetting.
    [Serializable()]
    public struct ServerSettings
    {
        public string SqlServer { get; set; }
        public string Database { get; set; }
        public bool IntegratedSecurity { get; set; }
        public bool TrustedConnection { get; set; }
        public string SqlUser { get; set; }
        public string SqlPass { get; set; }
        public bool IsLoading { get; set; }

        public string getConnStringSQL
        {
            get
            {
                StringBuilder strConnectionString = new StringBuilder();

                strConnectionString.Append("data source=");
                strConnectionString.Append(SqlServer);
                strConnectionString.Append(";");
                strConnectionString.Append("initial catalog=");
                strConnectionString.Append(Database);
                strConnectionString.Append(";");

                if (TrustedConnection != true)
                {
                    if (IntegratedSecurity)
                    {
                        strConnectionString.Append("Integrated Security=true;");
                    }
                    else
                    {
                        strConnectionString.Append("User ID=");
                        strConnectionString.Append(SqlUser);
                        strConnectionString.Append(";");
                        strConnectionString.Append("Pwd=");
                        strConnectionString.Append(SqlPass);
                        strConnectionString.Append(";");
                    }
                }
                else
                {
                    strConnectionString.Append("Trusted_Connection=");
                    strConnectionString.Append(TrustedConnection);
                    strConnectionString.Append(";");
                }

                return strConnectionString.ToString();
            }
        }

        public string getConnStringOleDb
        {
            get
            {
                StringBuilder strConnectionString = new StringBuilder();

                //This one's from Microsoft
                strConnectionString.Append("Provider=msdaora;");

                //This one's from Oracle
                //strConnectionString.Append("Provider=OraOLEDB.Oracle;"); 
                strConnectionString.Append("data source=");
                strConnectionString.Append(SqlServer);
                strConnectionString.Append(";");

                strConnectionString.Append("User ID=");
                strConnectionString.Append(SqlUser);
                strConnectionString.Append(";");
                strConnectionString.Append("Pwd=");
                strConnectionString.Append(SqlPass);
                strConnectionString.Append(";");

                return strConnectionString.ToString();
            }
        }


        //private string _SQLServer; 
        //private string _Database; 
        //private bool _IntegratedSecurity; 
        //private bool _Trusted_Connection;
        //private string _SQLUser; 
        //private string _SQLPass; 
        //private bool _IsLoading;

        //public string SQLServer
        //{
        //    get
        //    {
        //        return _SQLServer;
        //    }
        //    set
        //    {
        //        _SQLServer = value;
        //    }
        //}

        //public string Database
        //{
        //    get
        //    {
        //        return _Database;
        //    }
        //    set
        //    {
        //        _Database = value;
        //    }
        //}

        //public bool IntegratedSecurity
        //{
        //    get
        //    {
        //        return _IntegratedSecurity;
        //    }
        //    set
        //    {
        //        _IntegratedSecurity = value;
        //    }
        //}

        //public bool Trusted_Connection
        //{
        //    get
        //    {
        //        return _Trusted_Connection;
        //    }
        //    set
        //    {
        //        _Trusted_Connection = value;
        //    }
        //}

        //public string SQLUser
        //{
        //    get
        //    {
        //        return _SQLUser;
        //    }
        //    set
        //    {
        //        _SQLUser = value;
        //    }
        //}

        //public string SQLPass
        //{
        //    get
        //    {
        //        return _SQLPass;
        //    }
        //    set
        //    {
        //        _SQLPass = value;
        //    }
        //}

        //public bool IsLoading
        //{
        //    get
        //    {
        //        return _IsLoading;
        //    }
        //    set
        //    {
        //        _IsLoading = value;
        //    }
        //} 
    }


    //**************************************************************
    //  Static Class
    //**************************************************************
    public class ServerSettingsHelper
    {
        public static ServerSettings ServerSettingsCache;

        // default constructor
        public ServerSettingsHelper()
        {
            ServerSettingsCache.IsLoading = false;
        }


        public static ServerSettings getSettings()
        {
            if (ServerSettingsCache.IsLoading != true)
            {
                ServerSettingsCache = LoadSettingFromConfig();
                ServerSettingsCache.IsLoading = true;
            }
            return ServerSettingsCache;
        }


        private static ServerSettings LoadSettingFromConfig()
        {
            var oSettings = new ServerSettings()
            {
                SqlServer = ConfigurationManager.AppSettings["SQLServer"],
                Database = ConfigurationManager.AppSettings["Database"],
                IntegratedSecurity = bool.Parse(ConfigurationManager.AppSettings["IntegratedSecurity"]),
                TrustedConnection = bool.Parse(ConfigurationManager.AppSettings["Trusted_Connection"]),
                SqlUser = ConfigurationManager.AppSettings["SQLUser"],
                SqlPass = ConfigurationManager.AppSettings["SQLPass"],
            };

            return oSettings;
        }

    }
}

