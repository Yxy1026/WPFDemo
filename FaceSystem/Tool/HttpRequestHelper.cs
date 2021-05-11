using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using RestSharp;
using System.Text;
using FaceSystem.UserModel;

namespace FaceSystem.Tool
{
    class HttpRequestHelper
    {
        static public StudentInfo studentinfo;
        static public Token token;
        static public DepartmentInfo departmentInfo;
        public static List<T> GetList<T>(string url, string XH)
        {
            var resultRpt = Test(url, "XH=" + XH);
            return JsonConvert.DeserializeObject<IEnumerable<T>>(resultRpt).ToList();
        }

        public static Token GetToken(string url, string corpid, string corpsecret)
        {
            var resultRpt = Test2(url, "corpid=" + corpid, "corpsecret=" + corpsecret);
            return JsonConvert.DeserializeObject<Token>(resultRpt);
        }

        public static DepartmentInfo GetDepart(string url, string access_token)
        {
            var resultRpt = Test2(url, "access_token=" + access_token, "");
            return JsonConvert.DeserializeObject<DepartmentInfo>(resultRpt);
        }

        public static StudentInfo GetInfo(string url, string sfzh)
        {
            var resultRpt = Test(url, "sfzh=" + sfzh);
            return JsonConvert.DeserializeObject<StudentInfo>(resultRpt);
        }
        public static string Test2(string url, string para1, string para2)
        {
            var client = new RestClient(url + "?" + para1 + "&" + para2);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return response.Content;
        }

        public static string Test(string url, string para)
        {
            var client = new RestClient(url + "?" + para);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            IRestResponse response = client.Execute(request);
            return response.Content;
        }

    }

    public class StudentInfo
    {
        public string XM { get; set; }
        public string XH { get; set; }
        public string XB { get; set; }
        public string CSRQ { get; set; }
        public string XSM { get; set; }
        public string ZYM { get; set; }
        public string NJDM { get; set; }
        public string BYDM { get; set; }
        public string SJ { get; set; }
        public string BJH { get; set; }
        public string DH { get; set; }
        public string EMAIL { get; set; }

    }

    public class StudentScore
    {
        public string course_credit { get; set; }
        public string course_name { get; set; }
        public string course_num { get; set; }
        public string course_order { get; set; }
        public string course_score { get; set; }
        public string course_type { get; set; }

    }

    public class Token
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }

    public class DepartmentInfo
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public List<Department> department { get; set; }
    }
}
