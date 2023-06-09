namespace Params.YWX
{
    /// <summary>
    /// 数据哈希计算接口搜需参数
    /// </summary>
    public class Hash_Params
    {
        /// <summary>
        /// 要计算摘要的原文
        /// </summary>
        public string originData { get; set; }

        /// <summary>
        /// 加密算法，支持”SHA1”、”SHA256”、”SM3”，默认”SM3”
        /// </summary>
        public string hashAlg { get; set; }
    }
}