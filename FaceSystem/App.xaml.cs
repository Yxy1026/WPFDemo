using FaceSystem.Tool;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace FaceSystem
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            YuZhi = ConfigHelper.GetAppSetting("YuZhi");
        }
        public static string YuZhi = "0.7";
    }
}
