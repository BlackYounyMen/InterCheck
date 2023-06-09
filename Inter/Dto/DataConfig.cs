using InterFace;

namespace Inter
{
    public class DataConfig
    {
        /// <summary>
        /// 协议
        /// </summary>
        public static string httpdata { get; set; } =
        ConfigHelper.GetSection("http");

        /// <summary>
        /// 域名
        /// </summary>

        public static string domainName { get; set; } = ConfigHelper.GetSection("DomainName");

        /// <summary>
        /// 第三方厂商标识
        /// </summary>

        public static string clientId { get; set; } = ConfigHelper.GetSection("ClientId");

        /// <summary>
        /// 第三方厂商秘钥
        /// </summary>

        public static string clientSecret { get; set; } = ConfigHelper.GetSection("ClientSecret");
    }
}