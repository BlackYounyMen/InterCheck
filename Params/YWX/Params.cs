namespace Params.YWX
{
    public class head
    {
        /// <summary>
        /// 第三方厂商标识
        /// </summary>
        public string clientId { get; set; }

        /// <summary>
        /// 第三方厂商秘钥
        /// </summary>
        public string clientSecret { get; set; }
    }

    /// <summary>
    /// 传递参数
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class Params<T1, T2> where T1 : class, new() where T2 : class
    {
        /// <summary>
        /// 载荷
        /// </summary>
        public T2 body { get; set; }

        /// <summary>
        /// 头部
        /// </summary>
        public T1 head { get; set; }
    }
}