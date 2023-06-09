using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using InterFace;

namespace Inter.Util.Params.XTQM
{
    public class Param
    {
        public static SortedDictionary<string, string> GetConfigValue()
        {
            SortedDictionary<string, string> ParamsDic = new SortedDictionary<string, string>();
            ParamsDic.Add("appId", ConfigHelper.GetSection("appId"));
            ParamsDic.Add("signAlgo", ConfigHelper.GetSection("signAlgo"));
            ParamsDic.Add("version", ConfigHelper.GetSection("version"));
            //ParamsDic.Add("secretKey", ConfigHelper.GetSection("secretKey"));
            return ParamsDic;
        }

        /// <summary>
        /// 查询条件排序
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string SortJson(SortedDictionary<string, string> parameters, string key)
        {
            if (parameters.Count == 0)
                return string.Empty;

            var list = parameters.Select(r => $"{r.Key}={r.Value}").ToList();
            var toSignString = string.Join("&", list.Select(r => r));

            return toSignString;
        }

        /// <summary>
        /// 转换为SHA256 编码模式
        /// </summary>
        /// <param name="secret"></param>
        /// <param name="signKey"></param>
        /// <returns></returns>
        public static string HmacSHA256(string secret, string signKey)
        {
            string signRet = string.Empty;
            using (HMACSHA256 mac = new HMACSHA256(Encoding.UTF8.GetBytes(signKey)))
            {
                byte[] hash = mac.ComputeHash(Encoding.UTF8.GetBytes(secret));
                signRet = Convert.ToBase64String(hash);
            }
            return signRet;
        }

        /// <summary>
        /// 把值转换成编码模式
        /// </summary>
        /// <param name="paramdata"></param>
        /// <returns></returns>
        public static string GetsignatureValue(SortedDictionary<string, string> paramdata)
        {
            string secretKey = ConfigHelper.GetSection("secretKey");
            string sortStr = SortJson(paramdata, secretKey);
            string SignTemp = HmacSHA256(sortStr, secretKey);
            return SignTemp;
        }
    }
}