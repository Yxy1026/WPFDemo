using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace FaceSystem.Tool
{
    public class Utility
    {
        public static string GetPath(string FileName)
        {
            string now = DateTime.Now.ToString("yyMMddHHmmss");
            if (!Directory.Exists(ConfigHelper.GetAppSetting("FileSavePath") + FileName))
            {
                Directory.CreateDirectory(ConfigHelper.GetAppSetting("FileSavePath") + FileName); //新建文件夹  
            }
            return ConfigHelper.GetAppSetting("FileSavePath") + FileName + "\\" + now + "_";
        }

    }
}
