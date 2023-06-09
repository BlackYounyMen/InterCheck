using Inter.Util;
using Inter.Params.XTQM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SignParse;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Params.XTQM;

namespace Inter.Controllers
{
    public class XTQMController : Controller
    {
        ////////////////////////////////////////需要像调用方返回3个参数（必须）“signResult”、“signCert”、 “tsResp”////////////////////////////////////////
        ////////////////////////////////////////分别是 签名结果、签名证书、时间戳////////////////////////////////////////

        #region 注入公共配置

        private readonly XTQMJsonParse interJsonParse;

        public XTQMController()
        {
            string jsonfile = System.IO.Directory.GetCurrentDirectory() + "\\Json" + "\\XTQMInter.json";
            using (System.IO.StreamReader file = System.IO.File.OpenText(jsonfile))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o = (JObject)JToken.ReadFrom(reader);
                    string data = o.ToString();
                    interJsonParse = JsonConvert.DeserializeObject<XTQMJsonParse>(data);
                }
            }
            DataConfig.httpdata = ConfigHelper.GetSection("XTQM", "Http");
            DataConfig.domainName = ConfigHelper.GetSection("XTQM", "DomainName");
        }

        #endregion 注入公共配置

        #region 视图

        // GET: QRCodeController
        public ActionResult Login()
        { return View(); }

        public ActionResult Sign()
        { return View(); }

        public ActionResult addCert()
        { return View(); }

        #endregion 视图

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> add_pad_ca_user(string databody)
        {
            var api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.addUser;
            var data = JsonConvert.DeserializeObject<UserData>(databody);

            SortedDictionary<string, string> paramdata = Param.GetConfigValue();

            #region 在此放参数

            paramdata.Add("userName", data.userName);
            paramdata.Add("idNumber", data.idNumber);
            paramdata.Add("idType", data.idType);
            paramdata.Add("mobile", data.mobile);
            paramdata.Add("department", data.department);

            #endregion 在此放参数

            paramdata.Add("signature", Param.GetsignatureValue(paramdata));
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, paramdata, "添加用户");
        }

        /// <summary>
        /// 删除用户（测试专用）
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> updateUser(string userId)
        {
            var api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.updateUser;

            SortedDictionary<string, string> paramdata = Param.GetConfigValue();

            #region 在此放参数

            paramdata.Add("userId", userId);
            paramdata.Add("status", "DELETE");

            #endregion 在此放参数

            paramdata.Add("signature", Param.GetsignatureValue(paramdata));
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, paramdata, "添加用户");
        }

        /// <summary>
        /// 产生激活码
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> getAuthCode(string userId)
        {
            var api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.getAuthCode;

            SortedDictionary<string, string> paramdata = Param.GetConfigValue();

            #region 在此放参数

            paramdata.Add("userId", userId);

            #endregion 在此放参数

            paramdata.Add("signature", Param.GetsignatureValue(paramdata));

            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, paramdata, "产生激活码");
        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> queryUserInfo(string databody)
        {
            if (databody == null)
            {
                return "必须要有信息";
            }
            var api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.addUser;
            var data = JsonConvert.DeserializeObject<QueryUserData>(databody);

            SortedDictionary<string, string> paramdata = Param.GetConfigValue();

            #region 在此放参数

            if (data.userId != null)
            {
                paramdata.Add("userId", data.userId);
            }
            if (data.idNumber != null)
            {
                paramdata.Add("idNumber", data.idNumber);
            }
            if (data.idType != null)
            {
                paramdata.Add("idType", data.idType);
            }
            if (data.uniqueId != null)
            {
                paramdata.Add("uniqueId", data.uniqueId);
            }

            #endregion 在此放参数

            paramdata.Add("signature", Param.GetsignatureValue(paramdata));
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, paramdata, "查询用户");
        }

        #region 用户信息查看

        /// <summary>
        /// 3.3.4查询用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> DocRequestSelect(string msspId)
        {
            var api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.DocRequestSelect;

            SortedDictionary<string, string> paramdata = Param.GetConfigValue();

            #region 在此放参数

            paramdata.Add("userId", msspId);

            #endregion 在此放参数

            paramdata.Add("signature", Param.GetsignatureValue(paramdata));
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, paramdata, "开启登录二维码");
        }

        #endregion 用户信息查看

        #region 登录操作

        /// <summary>
        /// 3.5.1开启登录
        /// </summary>
        /// <param name="responseType"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Auth_Request()
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.Auth_Request;

            SortedDictionary<string, string> paramdata = Param.GetConfigValue();

            #region 在此放参数

            paramdata.Add("timeRegion", (24 * 60 * 60).ToString());

            #endregion 在此放参数

            paramdata.Add("signature", Param.GetsignatureValue(paramdata));

            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, paramdata, "开启登录二维码");
        }

        /// <summary>
        /// 3.4.3获取登录结果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetUser(string requestId)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.GetUser;

            SortedDictionary<string, string> paramdata = Param.GetConfigValue();

            #region 在此放参数

            paramdata.Add("signDataId", requestId);

            #endregion 在此放参数

            paramdata.Add("signature", Param.GetsignatureValue(paramdata));
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, paramdata, "获取用户信息");
        }

        #region 登录需要验证证书

        /// <summary>
        /// 3.7.3 证书验证
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CertVail(string cert)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.CertVail;

            SortedDictionary<string, string> paramdata = Param.GetConfigValue();

            #region 在此放参数

            paramdata.Add("cert", cert);

            #endregion 在此放参数

            paramdata.Add("signature", Param.GetsignatureValue(paramdata));
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, paramdata, " 证书验证");
        }

        #endregion 登录需要验证证书

        #endregion 登录操作

        #region 签名操作等流程

        /// <summary>
        /// 3.5.1开启自动签名授权接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Auto_Sign_Auth(string openid)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.Auto_Sign_Auth;

            SortedDictionary<string, string> paramdata = Param.GetConfigValue();

            #region 在此放参数

            paramdata.Add("userId", openid);

            #endregion 在此放参数

            paramdata.Add("signature", Param.GetsignatureValue(paramdata));
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, paramdata, "开启自动签");
        }

        /// <summary>
        ///  3.4.3获取签名授权结果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Acq_Auth_Res(string openid)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.Acq_Auth_Res;

            SortedDictionary<string, string> paramdata = Param.GetConfigValue();

            #region 在此放参数

            paramdata.Add("signDataId", openid);

            #endregion 在此放参数

            paramdata.Add("signature", Param.GetsignatureValue(paramdata));
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, paramdata, "获取用户信息");
        }

        /// <summary>
        /// 3.5.2关闭自动签
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> LogOut_Auth(string openid)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.LogOut_Auth;

            SortedDictionary<string, string> paramdata = Param.GetConfigValue();

            #region 在此放参数

            paramdata.Add("userId", openid);

            #endregion 在此放参数

            paramdata.Add("signature", Param.GetsignatureValue(paramdata));
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, paramdata, "退出授权接口");
        }

        #endregion 签名操作等流程

        #region 签名任务的三种方式

        /// <summary>
        /// 3.4.1添加签名任务
        /// </summary>

        [HttpPost]
        public async Task<string> EMR_Signature(string databody)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.EMR_Signature;

            SignData data = JsonConvert.DeserializeObject<SignData>(databody);
            SortedDictionary<string, string> paramdata = Param.GetConfigValue();

            #region 在此放参数

            paramdata.Add("userId", data.appid);
            paramdata.Add("title", string.IsNullOrWhiteSpace(data.title) ? ConfigHelper.GetSection("DefaultTitle") : data.title);
            paramdata.Add("dataType", "DATA");
            paramdata.Add("algo", "SM3withSM2");
            paramdata.Add("expiryDate", (24 * 3 * 60).ToString());

            paramdata.Add("data", data.oridata);

            #endregion 在此放参数

            paramdata.Add("signature", Param.GetsignatureValue(paramdata));
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, paramdata, " 添加签名任务");
        }

        /// <summary>
        /// 3.4.1添加签名任务  hash数据类型
        /// </summary>

        [HttpPost]
        public async Task<string> hash_Signature(string databody)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.EMR_Signature;

            SignData data = JsonConvert.DeserializeObject<SignData>(databody);
            SortedDictionary<string, string> paramdata = Param.GetConfigValue();

            #region 在此放参数

            paramdata.Add("userId", data.appid);
            paramdata.Add("title", string.IsNullOrWhiteSpace(data.title) ? ConfigHelper.GetSection("DefaultTitle") : data.title);
            paramdata.Add("dataType", "HASH");
            paramdata.Add("algo", "SM3withSM2");
            paramdata.Add("expiryDate", (24 * 3 * 60).ToString());

            paramdata.Add("data", data.oridata);

            #endregion 在此放参数

            paramdata.Add("signature", Param.GetsignatureValue(paramdata));
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, paramdata, " 添加签名任务");
        }

        /// <summary>
        /// 3.4.1添加签名任务  网页签章类型
        /// </summary>

        [HttpPost]
        public async Task<string> WEB_SEAL_Signature(string databody)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.EMR_Signature;

            SignData data = JsonConvert.DeserializeObject<SignData>(databody);
            SortedDictionary<string, string> paramdata = Param.GetConfigValue();

            #region 在此放参数

            paramdata.Add("userId", data.appid);
            paramdata.Add("title", string.IsNullOrWhiteSpace(data.title) ? ConfigHelper.GetSection("DefaultTitle") : data.title);
            paramdata.Add("dataType", "WEB_SEAL");
            paramdata.Add("algo", "SM3withSM2");
            paramdata.Add("expiryDate", (24 * 3 * 60).ToString());

            paramdata.Add("data", data.oridata);

            #endregion 在此放参数

            paramdata.Add("signature", Param.GetsignatureValue(paramdata));
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, paramdata, " 添加签名任务");
        }

        #endregion 签名任务的三种方式

        #region 自动签名的三种方式

        /// <summary>
        /// 3.5.3自动签名DATA类型
        /// </summary>
        /// <param name="databody"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> DATA_autoSign(string databody)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.autoSign;

            SignData data = JsonConvert.DeserializeObject<SignData>(databody);
            SortedDictionary<string, string> paramdata = Param.GetConfigValue();

            #region 在此放参数

            paramdata.Add("userId", data.appid);
            paramdata.Add("title", string.IsNullOrWhiteSpace(data.title) ? ConfigHelper.GetSection("XTQM", "DefaultTitle") : data.title);
            paramdata.Add("dataType", "DATA");
            paramdata.Add("algo", "SM3withSM2");
            paramdata.Add("expiryDate", (24 * 3 * 60).ToString());

            paramdata.Add("data", data.oridata);

            paramdata.Add("signToken", data.pad_signtoken);

            #endregion 在此放参数

            paramdata.Add("signature", Param.GetsignatureValue(paramdata));
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, paramdata, "自动签名");
        }

        /// <summary>
        /// 3.5.3自动签名HASH类型
        /// </summary>
        /// <param name="databody"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> HASH_autoSign(string databody)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.autoSign;

            SignData data = JsonConvert.DeserializeObject<SignData>(databody);
            SortedDictionary<string, string> paramdata = Param.GetConfigValue();

            #region 在此放参数

            paramdata.Add("userId", data.appid);
            paramdata.Add("title", string.IsNullOrWhiteSpace(data.title) ? ConfigHelper.GetSection("DefaultTitle") : data.title);
            paramdata.Add("dataType", "HASH");
            paramdata.Add("algo", "SM3withSM2");
            paramdata.Add("expiryDate", (24 * 3 * 60).ToString());

            paramdata.Add("data", data.oridata);

            paramdata.Add("signToken", data.pad_signtoken);

            #endregion 在此放参数

            paramdata.Add("signature", Param.GetsignatureValue(paramdata));
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, paramdata, "自动签名");
        }

        /// <summary>
        /// 3.5.3自动签名WEB_SEAL类型
        /// </summary>
        /// <param name="databody"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> WEB_SEAL_autoSign(string databody)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.autoSign;

            SignData data = JsonConvert.DeserializeObject<SignData>(databody);
            SortedDictionary<string, string> paramdata = Param.GetConfigValue();

            #region 在此放参数

            paramdata.Add("userId", data.appid);
            paramdata.Add("title", string.IsNullOrWhiteSpace(data.title) ? ConfigHelper.GetSection("DefaultTitle") : data.title);
            paramdata.Add("dataType", "WEB_SEAL");
            paramdata.Add("algo", "SM3withSM2");
            paramdata.Add("expiryDate", (24 * 3 * 60).ToString());

            paramdata.Add("data", data.oridata);

            paramdata.Add("signToken", data.pad_signtoken);

            #endregion 在此放参数

            paramdata.Add("signature", Param.GetsignatureValue(paramdata));
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, paramdata, "自动签名");
        }

        #endregion 自动签名的三种方式

        /// <summary>
        /// 3.4.3获取签名结果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Get_Signature(string signDataId)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.Get_Signature;

            SortedDictionary<string, string> paramdata = Param.GetConfigValue();

            #region 在此放参数

            paramdata.Add("signDataId", signDataId);

            #endregion 在此放参数

            paramdata.Add("signature", Param.GetsignatureValue(paramdata));
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, paramdata, "获取签名结果");
        }

        /// <summary>
        /// 3.8  集成时间戳接口调用
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> createAndGetTssInfo(string oriData)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.createAndGetTssInfo;

            SortedDictionary<string, string> paramdata = Param.GetConfigValue();

            #region 在此放参数

            paramdata.Add("oriData", oriData);
            paramdata.Add("attachCert", "true");  //最终产生的时间戳是否带证书(true / false)

            #endregion 在此放参数

            paramdata.Add("signature", Param.GetsignatureValue(paramdata));
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, paramdata, " 集成时间戳接口调用");
        }

        /// <summary>
        /// 验签  数据
        /// </summary>
        /// <param name="oriData"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ValiSignature(string databody)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.verifySignData;
            VailSign data = JsonConvert.DeserializeObject<VailSign>(databody);
            SortedDictionary<string, string> paramdata = Param.GetConfigValue();

            #region 在此放参数

            paramdata.Add("inDataType", data.inDataType);
            paramdata.Add("inData", data.inData);
            paramdata.Add("signAlg", data.signAlg);
            paramdata.Add("signValue", data.signValue);
            paramdata.Add("cert", data.cert);

            #endregion 在此放参数

            paramdata.Add("signature", Param.GetsignatureValue(paramdata));
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, paramdata, " 验签");
        }

        /// <summary>
        /// 计算PDF文件hash值
        /// </summary>
        /// <param name="oriData"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> getPdfHash(string databody)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.getPdfHash;
            PdfClass data = JsonConvert.DeserializeObject<PdfClass>(databody);
            ConfigureSealInfo Configsea = JsonConvert.DeserializeObject<ConfigureSealInfo>(databody);
            SortedDictionary<string, string> paramdata = Param.GetConfigValue();

            #region 在此放参数

            paramdata.Add("userId", data.userId);
            paramdata.Add("signSealType", string.IsNullOrWhiteSpace(data.signSealType) ? ConfigHelper.GetSection("DefaulesignSealType") : data.signSealType);

            paramdata.Add("pdfFileStr", data.pdfFileStr);
            paramdata.Add("configureSealInfo", JsonConvert.SerializeObject(Configsea));
            paramdata["version"] = "2.0.7";

            #endregion 在此放参数

            paramdata.Add("signature", Param.GetsignatureValue(paramdata));
            ///签完以后返回数据  ，之后走hash 签， hash签完之后走合章签名
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, paramdata, "计算PDF文件hash值");
        }

        /// <summary>
        /// 合并 PDF 签章
        /// </summary>
        /// <param name="oriData"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> composePdfSeal(string databody)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.composePdfSeal;
            composePdfSeal data = JsonConvert.DeserializeObject<composePdfSeal>(databody);
            SortedDictionary<string, string> paramdata = Param.GetConfigValue();

            #region 在此放参数

            paramdata.Add("userId", data.userId);
            paramdata.Add("signSealType", string.IsNullOrWhiteSpace(data.signSealType) ? ConfigHelper.GetSection("DefaulesignSealType") : data.signSealType);
            paramdata.Add("signValue", data.signValue);
            paramdata.Add("pdfBytesStr", data.pdfBytesStr);
            paramdata.Add("hashSetBytesStr", data.hashSetBytesStr);

            #endregion 在此放参数

            paramdata.Add("signature", Param.GetsignatureValue(paramdata));
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, paramdata, " 验签");
        }

        /// <summary>
        /// 验签  数据
        /// </summary>
        /// <param name="oriData"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> verifyPdfSeal(string databody)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.verifySignData;
            VailSign data = JsonConvert.DeserializeObject<VailSign>(databody);
            SortedDictionary<string, string> paramdata = Param.GetConfigValue();

            #region 在此放参数

            paramdata.Add("inDataType", data.inDataType);
            paramdata.Add("inData", data.inData);
            paramdata.Add("signAlg", data.signAlg);
            paramdata.Add("signValue", data.signValue);
            paramdata.Add("cert", data.cert);

            #endregion 在此放参数

            paramdata.Add("signature", Param.GetsignatureValue(paramdata));
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, paramdata, " 验签");
        }
    }
}