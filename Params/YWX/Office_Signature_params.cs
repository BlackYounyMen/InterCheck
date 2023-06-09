using System;

namespace Params.YWX
{
    public class Office_Signature_params
    {
        /// <summary>
        /// 签名方式
        /// 0：推送签名（仅限推送到医网信APP）
        /// 1：SDK集成签名
        /// 2：PC二维码签名
        /// </summary>
        public int signType { get; set; }

        public Params<Office_head, Office_body> msg { get; set; }
    }

    /// <summary>
    /// 头部
    /// </summary>
    public class Office_head
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
        /// 合同类||行政办公类
        /// </summary>
        public string templateId { get; set; }

        /// <summary>
        /// 系统标识（进行自动签名业务系统隔离及）
        /// </summary>
        public string sysTag { get; set; }

        /// <summary>
        /// 是否自动签，“true”or "false"
        /// </summary>
        public string selfSign { get; set; }
    }

    /// <summary>
    /// 载荷
    /// </summary>
    public class Office_body
    {
        /// <summary>
        /// 签名数据id，字母、数字，或者字母数字组合，长度32位以内
        /// </summary>
        public string urId { get; set; }

        /// <summary>
        /// 医网信医师唯一标识，长度35位
        /// </summary>
        public string openId { get; set; }

        /// <summary>
        /// 展示姓名
        /// </summary>
        public string viewName { get; set; }

        /// <summary>
        /// 展示时间，yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string viewTime { get; set; }

        /// <summary>
        /// pdf文件的base64
        /// </summary>
        public string pdfBase64 { get; set; }

        /// <summary>
        /// 展示主题，限制长度50
        /// </summary>
        public string viewSubject { get; set; }

        /// <summary>
        /// 签名备注，限制长度300
        /// </summary>
        public string viewRemark { get; set; }
    }
}