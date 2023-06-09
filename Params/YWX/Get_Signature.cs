using System;

namespace Params.YWX
{
    /// <summary>
    /// 获取签名结果载荷
    /// </summary>
    public class Get_Signature
    {
        /// <summary>
        /// 签名数据唯一标识
        /// </summary>
        public string uniqueId { get; set; }

        /// <summary>
        /// 是否需要获取p1签名值；true：返回用户证书和p1签名值，false：返回p7签名值
        /// </summary>
        public bool isP1 { get; set; }
    }
}