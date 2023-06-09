using System;
using System.Text;

namespace Params.YWX
{
    public class Data_Signature_params<T> where T : class
    {
        /// <summary>
        /// 签名方式(按照示例入参即可)
        /// 0：推送签名（指定医生） 4：auth绑定签名（无需指定医生，推送后返回二维码医生进行扫码绑定）
        /// </summary>
        public int signType { get; set; }

        /// <summary>
        /// 内容体
        /// </summary>
        public Params<Data_Head, T> msg { get; set; }
    }

    /// <summary>
    /// 数据签名载荷
    /// </summary>
    public class Data_Body
    {
        /// <summary>
        /// 签名订单流水ID，字母、数字，或者字母数字组合，长度64位以内
        /// </summary>
        public string urId { get; set; }

        /// <summary>
        ///医网信医师唯一标识；signType=4 auth签名时可空
        /// </summary>
        public string openId { get; set; }

        /// <summary>
        /// 患者姓名
        /// </summary>
        public string patientName { get; set; }

        /// <summary>
        /// 患者年龄
        /// </summary>
        public string patientAge { get; set; }

        /// <summary>
        /// 患者性别
        /// </summary>
        public string patientSex { get; set; }

        /// <summary>
        /// 患者证件号，证件类型为QT时可以
        /// </summary>
        public string patientCard { get; set; }

        /// <summary>
        /// 证件类型 YB：患者医保号 SF：身份证 HZ：护照 QT：其它
        /// </summary>
        public string patientCardType { get; set; }

        /// <summary>
        /// 开具时间 格式：yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string recipeTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        /// <summary>
        /// Hash原文(从4.3.4 数据哈希计算接口结果中得到的)
        /// </summary>
        public string hashValue { get; set; }

        /// <summary>
        /// Hash算法(从4.3.4 数据哈希计算接口结果中得到的)
        /// </summary>
        public string hashType { get; set; }
    }

    /// <summary>
    /// 数据签名表头
    /// </summary>
    public class Data_Head
    {
        /// <summary>
        /// 第三方账号标识
        /// </summary>
        public string clientId { get; set; }

        /// <summary>
        /// personRolesPdf:个人pdf多盖章区域签名
        /// hash开头模板ID签名数据摘要
        /// </summary>
        public string templateId { get; set; }

        /// <summary>
        /// 第三方账号秘钥
        /// </summary>
        public string clientSecret { get; set; }

        /// <summary>
        /// 渠道ID
        /// </summary>
        public string channelID { get; set; }

        /// <summary>
        /// 渠道ID
        /// </summary>
        public string sysTag { get; set; }
    }

    #region PDF载荷数据

    /// <summary>
    /// PDF签名载荷
    /// </summary>
    public class Pdf_Body
    {
        /// <summary>
        /// 签名订单流水ID，字母、数字，或者字母数字组合，长度64位以内
        /// </summary>
        public string urId { get; set; }

        /// <summary>
        /// 医网信医师唯一标识 长度35位
        /// </summary>
        public string openId { get; set; }

        /// <summary>
        /// 患者姓名
        /// </summary>
        public string patientName { get; set; }

        /// <summary>
        /// 患者年龄
        /// </summary>
        public string patientAge { get; set; }

        /// <summary>
        /// 患者性别
        /// </summary>
        public string patientSex { get; set; }

        /// <summary>
        /// 患者医保号身份证护照（如果证件类型为QT，可为空字符串），最多20个字符
        /// </summary>
        public string patientCard { get; set; }

        /// <summary>
        /// 证件类型YB：患者医保号SF：身份证HZ：护照QT：其它
        /// </summary>
        public string patientCardType { get; set; }

        /// <summary>
        /// 开具时间，格式：yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string recipeTime { get; set; }

        /// <summary>
        /// 第三方签名信息主题，第三方提供后期在列表页展示（0~50位）
        /// </summary>
        public string subject { get; set; }

        /// <summary>
        /// 医生诊断
        /// </summary>
        public string diagnose { get; set; }

        /// <summary>
        /// pdf文件的base64格式
        /// </summary>
        public string pdfBase64 { get; set; }

        /// <summary>
        /// 盖章区域列表
        /// </summary>
        public PDF_Arae roles { get; set; }
    }

    /// <summary>
    /// 盖章区域
    /// </summary>
    public class PDF_Arae
    {
        /// <summary>
        /// 印章位置（相对于盖章关键字,默认居右）
        /// 1:居右
        /// 2:居下
        /// 3:重叠
        /// </summary>
        public string moveType { get; set; }

        /// <summary>
        /// 盖章区域定位关键字，连续字符，中间不能含有空格
        /// </summary>
        public string keyword { get; set; }

        /// <summary>
        /// 搜索顺序
        /// 1:正序
        /// 2:倒序
        /// 默认倒序
        /// </summary>
        public string searchOrder { get; set; }

        /// <summary>
        /// 指定页内关键字序号。默认值：1
        /// </summary>
        public int searchNum { get; set; }

        /// <summary>
        /// （坐标盖章）通过坐标定位时，需指定盖章位置在第几页，最小1，最大不超过pdf页数
        /// </summary>
        public int locationPage { get; set; }

        /// <summary>
        /// x坐标 大于50，小于pdf宽度
        /// </summary>
        public float x { get; set; }

        /// <summary>
        /// Y坐标 大于50，小于pdf高度
        /// </summary>
        public float y { get; set; }

        /// <summary>
        /// 签章图片缩放比例
        /// </summary>
        public float scale { get; set; }
    }

    #endregion PDF载荷数据

    #region EMR(电子病历)

    /// <summary>
    /// 电子病历载荷
    /// </summary>
    public class EMR_Data
    {
        /// <summary>
        /// 签名订单流水ID，字母、数字，或者字母数字组合，长度64位以内
        /// </summary>
        public string urId { get; set; }

        /// <summary>
        /// 医网信医师唯一标识；signType=4 auth签名时可空
        /// </summary>
        public string openId { get; set; }

        /// <summary>
        /// 患者姓名
        /// </summary>
        public string patientName { get; set; }

        /// <summary>
        /// 患者年龄
        /// </summary>
        public string patientAge { get; set; }

        /// <summary>
        /// 患者性别
        /// </summary>
        public string patientSex { get; set; }

        /// <summary>
        /// 证件类型 YB：患者医保号 SF：身份证 HZ：护照 QT：其它
        /// </summary>
        public string patientCardType { get; set; }

        /// <summary>
        /// 患者证件号，证件类型为QT时可以为空，最多20个字符
        /// </summary>
        public string patientCard { get; set; }

        /// <summary>
        /// 开具时间 格式：yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string recipeTime { get; set; }

        /// <summary>
        /// Hash原(从4.3.4 数据哈希计算接口结果中得到的)
        /// </summary>
        public string hashValue { get; set; }

        /// <summary>
        /// Hash算法(从4.3.4 数据哈希计算接口结果中得到的)
        /// </summary>
        public string hashType { get; set; }

        /// <summary>
        /// 主题 （业务系统可在此字段内上传仅可中文，数字，英文，标点符号，最大50字符）
        /// </summary>
        public string subject { get; set; }

        /// <summary>
        /// 数据标签
        ///（最多支持5个标签，多个用“,”分隔，每个标签最多5个字符；例：”tag1,tag2,tag3”）
        /// </summary>
        public string tag { get; set; }

        /// <summary>
        /// 签名备注
        /// </summary>
        public string remarks { get; set; }
    }

    #endregion EMR(电子病历)
}