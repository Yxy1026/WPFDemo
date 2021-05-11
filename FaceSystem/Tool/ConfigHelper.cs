using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace FaceSystem.Tool
{
    public class ConfigHelper
    {
        /// <summary>
        /// 获取AppSetting的值

        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 设置AppSetting的值

        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void SetAppSetting(string key, string value)
        {
            AppSettingsSection appSection = null; //AppSection对象  
            appSection = ConfigurationManager.GetSection("appSettings") as AppSettingsSection;
            if (appSection != null)
            {
                appSection.Settings[key].Value = value;

            }
        }

        public static void SetConnectionString(string value)
        {
            System.Configuration.Configuration config =
                             ConfigurationManager.OpenExeConfiguration(
                             ConfigurationUserLevel.None);
            ConnectionStringsSection con = (ConnectionStringsSection)config.GetSection("connectionStrings");
            con.ConnectionStrings["ConnectionString"].ConnectionString = value;
            config.Save();//ConfigurationSaveMode.Modified
            ConfigurationManager.RefreshSection("connectionStrings");

        }

        /// <summary>
        /// 修改配置文件
        /// </summary>
        /// <param name="key">配置的Key</param>
        /// <param name="value">配置的Value</param>
        public static void UpdateConfig(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            AppSettingsSection assecion = (AppSettingsSection)config.GetSection("appSettings");
            assecion.Settings[key].Value = value;
            //config.AppSettings.Settings[key].Value = value;
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
