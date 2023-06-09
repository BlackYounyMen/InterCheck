using InterFace;
using Newtonsoft.Json;
using System;

namespace Inter.Util
{
    public class LogHelper
    {
        public static string path = System.IO.Directory.GetCurrentDirectory();
        public static string IsDebug = ConfigHelper.GetSection("IsDebug");

        public static string ResponseUrl = path + "\\wwwroot\\Responselog.txt";
        public static string RequestUrl = path + "\\wwwroot\\Requestlog.txt";
        public static string ErrorUrl = path + "\\wwwroot\\Errorlog.txt";

        //select FName,F_Birthday from TAthlete  where FName like '%真%' order by   F_Birthday asc

        public static string Loging(string type, object msg, string url)
        {
            if (IsDebug == "1")
            {
                switch (type)
                {
                    case "Response": System.IO.File.AppendAllText(ResponseUrl, $"发送数据——{DateTime.Now.ToString("HH:mm:ss")}:{url}方法底下 -- 【response】{JsonConvert.SerializeObject(msg)} \n\n"); break;
                    case "Request": System.IO.File.AppendAllText(RequestUrl, $"返回数据——{DateTime.Now.ToString("HH:mm:ss")}:{url}方法底下 -- 【Request】{JsonConvert.SerializeObject(msg)} \n\n"); break;
                    case "Error": System.IO.File.AppendAllText(ErrorUrl, $"报错数据——{DateTime.Now.ToString("HH:mm:ss")}:{url}方法底下 -- 【Error】{JsonConvert.SerializeObject(msg)} \n\n"); break;
                }
            }
            return "";
        }
    }
}