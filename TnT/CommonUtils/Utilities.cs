using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using TnT.Models;

namespace TnT
{
    public class Utilities
    {
        public static bool compare(string str1, string str2)
        {
            bool hassimilar = false;

            if (str1 != null && str2 != null)
            {
                hassimilar = (String.Compare(str1.Trim(), str2.Trim(), StringComparison.CurrentCultureIgnoreCase) == 0);
            }

            return hassimilar;
        }


        public static string getAppSettings(string settingName)
        {
            return getAppSetingsFromDB(settingName);
            //return ConfigurationManager.AppSettings[settingName];
            
        }

        public static string getConnectionString(string connectionName)
        {
            //return ConfigurationManager.AppSettings[settingName];
            return getDBConnectionString(connectionName);
        }


        public static List<string> getNullFieldNames(Object obj)
        {
            List<string> unavailableFields = new List<string>();

            foreach (PropertyInfo item in obj.GetType().GetProperties())
            {
                if (item.PropertyType == typeof(string))
                {
                    string value = (string)item.GetValue(obj);
                    if (string.IsNullOrEmpty(value))
                    {
                        unavailableFields.Add(item.Name);
                    }
                }
                else if (item.PropertyType == typeof(int?) || item.PropertyType == typeof(int))
                {
                    int? value = (int?)item.GetValue(obj);
                    if (value == 0 || value == null)
                    {
                        unavailableFields.Add(item.Name);
                    }
                }
                else if (item.PropertyType == typeof(bool?))
                {
                    bool? value = (bool?)item.GetValue(obj);
                    if (value == null)
                    {
                        unavailableFields.Add(item.Name);
                    }
                }
                else if (item.PropertyType == typeof(decimal?) || item.PropertyType == typeof(decimal))
                {
                    decimal? value = (decimal?)item.GetValue(obj);
                    if (value == 0 || value == null)
                    {
                        unavailableFields.Add(item.Name);
                    }
                }
            }
            return unavailableFields;
        }


        #region utils
        private static string getAppSetingsFromDB(string settingName)
        {

            ApplicationDbContext db = new ApplicationDbContext();
            var data = db.AppSettings.Where(x => x.Key == settingName).FirstOrDefault();
            if (data != null)
            {
                return data.Value;
            }
            return "";
        }





        private static string getDBConnectionString(string connectionName)
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ToString();           
        }

        #endregion
    }
}