using System;

namespace Params.YWX
{
    public class Get_Signature_Res
    {
        /// <summary>
        /// 签名流水号
        /// </summary>
        public string urId { get; set; }

        /// <summary>
        /// 签名数据唯一标识
        /// </summary>
        public string uniqueId { get; set; }

        /// <summary>
        /// p7签名结果（只有院内签名订单状态是已签名回调，并且isP1为false才有值）
        /// </summary>
        public string signedData { get; set; }

        /// <summary>
        /// pdf文件base64编码（仅在pdf签名成功状态时回调）
        /// </summary>
        public string signedPdfBase64 { get; set; }

        /// <summary>
        /// p1签名结果（只有院内签名订单状态是已签名回调，并且isP1为true才有值）
        /// </summary>
        public string p1 { get; set; }

        /// <summary>
        /// 用户证书（只有院内签名订单状态是已签名回调，并且isP1为true才有值）
        /// </summary>
        public string cert { get; set; }

        /// <summary>
        /// 签名图章的base64字符串
        /// </summary>
        public string stamp { get; set; }

        /// <summary>
        /// 签名时间(格式yyyy-MM-dd HH:mm:ss)
        /// </summary>
        public string signTime { get; set; }

        /// <summary>
        /// 失败或拒签原因
        /// </summary>
        public string failReason { get; set; }
    }
}