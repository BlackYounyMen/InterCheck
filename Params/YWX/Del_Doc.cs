namespace Params.YWX
{
    /// <summary>
    /// 删除待签表头
    /// </summary>
    public class Del_Doc_head
    {
        /// <summary>
        ///  第三方厂商标识
        /// </summary>
        public string clientId { get; set; }
    }

    /// <summary>
    /// 删除代签载荷
    /// </summary>
    public class Del_Doc_body
    {
        /// <summary>
        /// 签名数据唯一标识
        /// </summary>
        public string uniqueId { get; set; }
    }
}