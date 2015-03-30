using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using StackExchange.Redis;

namespace SimpleTwitter.ReadSide.Data
{
    
    public class ConfigUtils
    {
        const string ErrorAppsettingNotFound = "Unable to find App Setting: {0}";

      /// <summary>
        /// Gets the app setting.
        /// </summary>
        public static string GetAppSetting(string key)
        {
            string value = ConfigurationManager.AppSettings[key];

            if (value == null)
            {
                throw new ConfigurationErrorsException(String.Format(ErrorAppsettingNotFound, key));
            }

            return value;
        }

        public static int GetDatabaseNumber(string appSettingsKey = "RedisDatabaseNumber")
        {
            return int.Parse(GetAppSetting(appSettingsKey,"0"));
        }

        public static ConfigurationOptions GetConfiguration(string appSettingsKey = "RedisConfig",bool allowAdmin = false)
        {
            var connection = GetAppSetting(appSettingsKey);
            if (allowAdmin && !connection.Contains("allowAdmin"))
            {
                connection += ",allowAdmin=true";
            }
            return ConfigurationOptions.Parse(connection);
        }


        /// <summary>
        /// Returns AppSetting[key] if exists otherwise defaultValue
        /// </summary>
        public static string GetAppSetting(string key, string defaultValue)
        {
            return ConfigurationManager.AppSettings[key] ?? defaultValue;
        }

       


      

      
       
    }
}