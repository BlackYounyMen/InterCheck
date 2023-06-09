using System;

namespace Params.YWX
{
    /// <summary>
    /// 表头
    /// </summary>
    public class Signature_State_head
    {
        public string clientId { get; set; }
    }

    /// <summary>
    /// 载荷
    /// </summary>
    public class Signature_State_body
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime startDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime endDate { get; set; }

        /// <summary>
        /// 页码 默认1
        /// </summary>
        public int pageNum { get; set; }

        /// <summary>
        /// 页条数 默认显示10条
        /// </summary>
        public int pageSize { get; set; }
    }
}