namespace Params.YWX
{
    /// <summary>
    /// 验证签名表头
    /// </summary>
    public class Vali_Params
    {
        /// <summary>
        /// 第三方厂商标识
        /// </summary>
        public string clientId { get; set; }

        /// <summary>
        /// 第三方厂商秘钥
        /// </summary>
        public string clientSecret { get; set; }

        /// <summary>
        /// 签名数据唯一标识
        /// </summary>
        public string uniqueId { get; set; }

        /// <summary>
        /// 模板
        /// </summary>
        public string templateId { get; set; }
    }
}