using System.ComponentModel.DataAnnotations;

namespace Params.XTQM
{
    public class VailSign
    {
        /// <summary>
        /// 数据类型 DATA|HASH|WEB_SEAL
        /// </summary>
        public string inDataType { get; set; }

        /// <summary>
        /// Base64 格式的原文数据
        /// </summary>
        public string inData { get; set; }

        /// <summary>
        ///签名算法：
        /// SHA1withRSA|SHA256withRSA|SM3withSM2
        /// </summary>
        public string signAlg { get; set; }

        /// <summary>
        /// 签名值
        /// </summary>
        public string signValue { get; set; }

        /// <summary>
        /// 签名证书
        /// </summary>
        public string cert { get; set; }
    }

    public class SignData
    {
        public string table_name { get; set; }

        /// <summary>
        ///   用户id  （是否必须） 否
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 标题（最长不超过100个字符） （是否必须）是
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 签名算法(SM3 withSM2)       （是否必须） 是
        /// </summary>
        public string algo { get; set; }

        /// <summary>
        /// 描述（最长不超过200个字符）（是否必须）否
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 签名任务有效期（截止时间，单位为分钟），不能
        //  大于4320分钟，即3天有效期（该字段为空则款认
        //  有效期为1天)   （是否必须）  否
        /// </summary>
        public string expiryDate { get; set; }

        /// <summary>
        /// 待签数据（必须是base64编码，最大不超过1M)      （是否必须） 是
        /// </summary>
        public string oridata { get; set; }

        /// <summary>
        /// 回调地址      （是否必须）  否
        /// </summary>
        public string callBackUrl { get; set; }

        /// <summary>
        /// 开启自动签名的开启自动签返回的 signDataId  非自动签，不需要使用，只有自动签名的时候才会使用
        /// </summary>
        public string pad_signtoken { get; set; }
    }

    public class UserData
    {
        public string userName { get; set; }

        public string idNumber { get; set; }

        public string idType { get; set; }

        public string mobile { get; set; }

        public string department { get; set; }
    }

    public class QueryUserData
    {
        public string userId { get; set; }

        public string idNumber { get; set; }

        public string idType { get; set; }

        public string uniqueId { get; set; }
    }
}