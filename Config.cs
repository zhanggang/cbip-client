using System;
using System.Configuration;
using System.Collections.Generic;

namespace Console
{
    public class Config
    {
        public static readonly string Server;
        public static readonly int Port;
        public static readonly string IsRespon;
        public static readonly string To;
        public static readonly string FilePath;

        public static readonly List<UserInfo> Users;

        private Config() { }

        static Config() 
        {
            Users = ConfigurationManager.GetSection("users") as List<UserInfo>;

            Server = ConfigurationManager.AppSettings.Get("Server");
            Port = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Port"));
            IsRespon = ConfigurationManager.AppSettings.Get("IsRespon");
            To = ConfigurationManager.AppSettings.Get("To");
            FilePath = ConfigurationManager.AppSettings.Get("FilePath");
        }
    }
}
