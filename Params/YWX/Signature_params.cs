namespace Params.YWX
{
    /// <summary>
    /// 表头
    /// </summary>
    public class Signature_head
    {
        /// <summary>
        /// 第三方厂商标识
        /// </summary>
        public string clientId { get; set; }
    }

    /// <summary>
    /// 载荷
    /// </summary>
    public class Signature_body
    {
        /// <summary>
        /// 签名数据唯一标识
        /// </summary>
        public string uniqueId { get; set; }

        /// <summary>
        /// 用户标识openId
        /// </summary>
        public string openId { get; set; }
    }
}