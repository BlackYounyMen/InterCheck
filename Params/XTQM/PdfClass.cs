using System.Runtime.InteropServices;

namespace Inter.Params.XTQM
{
    public class PdfClass
    {
        public string userId { get; set; }
        public string signSealType { get; set; }

        public string pdfFileStr { get; set; }

        public ConfigureSealInfo configureSealInfo { get; set; }

        public float signWidth { get; set; }

        public float signHeight { get; set; }
    }

    public class ConfigureSealInfo
    {
        /// <summary>
        /// 类型，个人签章或企业签章
        /// </summary>
        public string signType { get; set; }

        /// <summary>
        /// 地理位置
        /// </summary>
        public string signProvince { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string signName { get; set; }
    }




    public class composePdfSeal
    {
        public string userId { get; set; }
        public string signSealType { get; set; }

        public string signValue { get; set; }

        public string pdfBytesStr { get; set; }

        public string hashSetBytesStr { get; set; }

    }
}