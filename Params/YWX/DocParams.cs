namespace Params.YWX
{
    public class Doc_body
    {
        /// <summary>
        /// 用户开放标识
        /// </summary>
        public string employeeNumber { get; set; }

        /// <summary>
        /// 用户手机号
        /// </summary>
        public string openId { get; set; }

        /// <summary>
        /// 证件号
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 员工号
        /// </summary>
        public string userIdcardNum { get; set; }
    }

    public class Single_head
    {
        /// <summary>
        /// 第三方厂商标识
        /// </summary>
        public string clientId { get; set; }
    }
}