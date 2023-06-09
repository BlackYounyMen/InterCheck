using Inter.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using SignParse;
using Params.YWX;

namespace Inter.Controllers
{
    public class YWXController : Controller
    {
        private readonly YWXJsonParse interJsonParse;

        public YWXController()
        {
            string jsonfile = System.IO.Directory.GetCurrentDirectory() + "\\Json" + "\\YWXInter.json";
            using (System.IO.StreamReader file = System.IO.File.OpenText(jsonfile))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o = (JObject)JToken.ReadFrom(reader);
                    string data = o.ToString();
                    interJsonParse = JsonConvert.DeserializeObject<YWXJsonParse>(data);
                }
            }

            DataConfig.httpdata = ConfigHelper.GetSection("YWX", "Http");
            DataConfig.domainName = ConfigHelper.GetSection("YWX", "DomainName");
            DataConfig.clientId = ConfigHelper.GetSection("YWX", "ClientId");
            DataConfig.clientSecret = ConfigHelper.GetSection("YWX", "ClientSecret");
        }

        /// <summary>
        /// 返回到前个服务器(代签数据已经签完名后的结果)
        /// </summary>
        /// <param name="datainfo"></param>
        /// <returns></returns>

        public string SignBackData(string datainfo)
        {
            string api_url = ConfigHelper.GetSection("RequestDataUrl");

            Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(datainfo);
            //get 请求参数方法
            api_url = api_url + "?operation=savesignresult";

            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, data, "返回前端调用方");
        }

        /// <summary>
        /// 2.3.2 同步医师结果查询接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> DocRequestSelect(string openid)
        {
            var api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.DocRequestSelect;

            var requestParams = new Params<Single_head, Doc_body>
            {
                head = new Single_head
                {
                    clientId = DataConfig.clientId,
                },
                body = new Doc_body
                {
                    openId = openid,
                    phone = "",
                    userIdcardNum = "",
                    employeeNumber = "",
                }
            };

            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, requestParams, "同步医师结果查询接口");
        }

        /// <summary>
        /// 3.3.1 OAuth认证请求接口
        /// </summary>
        /// <param name="responseType"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Auth_Request(string responseType)
        {
            if (responseType == null)
            {
                responseType = "code";
            }

            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.Auth_Request;

            //get 请求参数方法
            api_url = api_url + "?responseType=" + responseType + "&clientId=" + DataConfig.clientId + "&sessionTime=16&selfSign=true";

            return HttpClientHelper.Execute(HttpType.HttpGet, api_url, null, null, "OAuth认证请求接口");
        }

        /// <summary>
        /// 3.3.2 OAuth登陆-获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetUser(string requestId)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.GetUser;

            //get 请求参数方法
            api_url = api_url + "?clientId=" + DataConfig.clientId + "&requestId=" + requestId;

            return HttpClientHelper.Execute(HttpType.HttpGet, api_url, null, null, "获取用户信息");
        }

        /// <summary>
        /// 4.3.1自动签名授权-请求自动签名授权接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Auto_Sign_Auth(string openid)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.Auto_Sign_Auth;

            var requestParams = new Params<head, Auto_body>
            {
                head = new head
                {
                    clientId = DataConfig.clientId,
                    clientSecret = DataConfig.clientSecret
                },
                body = new Auto_body
                {
                    openId = openid,
                    sysTag = "his"
                }
            };

            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, requestParams, "请求自动签名授权接口");
        }

        /// <summary>
        /// 4.3.2 自动签名授权-获取授权结果接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Acq_Auth_Res(string openid)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.Acq_Auth_Res;

            var requestParams = new Params<head, Auto_body>
            {
                head = new head
                {
                    clientId = DataConfig.clientId,
                    clientSecret = DataConfig.clientSecret
                },
                body = new Auto_body
                {
                    openId = openid,
                    sysTag = "his"
                }
            };
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, requestParams, "获取授权结果接口");
        }

        /// <summary>
        /// 4.3.3 自动签名授权 - 退出授权接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> LogOut_Auth(string openid)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.LogOut_Auth;

            var requestParams = new Params<head, Auto_body>
            {
                head = new head
                {
                    clientId = DataConfig.clientId,
                    clientSecret = DataConfig.clientSecret
                },
                body = new Auto_body
                {
                    openId = openid,
                    sysTag = "his"
                }
            };
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, requestParams, "退出授权接口");
        }

        /// <summary>
        /// 4.3.4 数据哈希计算接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Data_Hash(string originData)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.Data_Hash;
            var requestParams = new Hash_Params
            {
                originData = originData,
                hashAlg = "SM3"
            };

            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, requestParams, "数据哈希计算接口");
        }

        /// <summary>
        /// 4.3.5 数据签名接口（纯数据）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Data_Signature(string databody)
        {
            var data_body = JsonConvert.DeserializeObject<Data_Body>(databody);

            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + "/gateway/recipe/synRecipeInfo";

            switch (data_body.patientCardType)
            {
                case "1": data_body.patientCardType = "HZ"; break;
                case "2": data_body.patientCardType = "SF"; break;
                case "3": data_body.patientCardType = "QT"; break;
            }

            //1 男  2女
            if (data_body.patientSex == "1")
            {
                data_body.patientSex = "男";
            }
            else
            {
                data_body.patientSex = "女";
            }

            string inputFormat = "MM dd yyyy h:mmtt";
            string outputFormat = "yyyy-MM-dd HH:mm:ss";

            string cleanedInput = data_body.recipeTime.Replace("  ", " ");
            DateTime dateTime;
            if (DateTime.TryParseExact(cleanedInput, inputFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                string output = dateTime.ToString(outputFormat);
                //yyyy-MM-dd HH:mm:ss
                data_body.recipeTime = output;
            }
            else
            {
                Console.WriteLine("Failed to parse input date");
            }

            var dataParams = new Params<Data_Head, Data_Body>
            {
                head = new Data_Head
                {
                    clientId = DataConfig.clientId,
                    templateId = "hash",
                    clientSecret = DataConfig.clientSecret,
                    channelID = "",
                    sysTag = "his"
                },
                body = data_body,
            };

            var requestParams = new Data_Signature_params<Data_Body>
            {
                signType = 0,
                msg = dataParams
            };

            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, requestParams, "数据签名接口");
        }

        /// <summary>
        /// 4.3.5 数据签名接口(PDF文件签名接口)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PDF_Signature(string databody)
        {
            var base64file = JsonConvert.DeserializeObject<FileParse>(databody);
            var data_body = JsonConvert.DeserializeObject<Pdf_Body>(databody);
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + "/gateway/recipe/synRecipeInfo";

            switch (data_body.patientCardType)
            {
                case "1": data_body.patientCardType = "HZ"; break;
                case "2": data_body.patientCardType = "SF"; break;
                case "3": data_body.patientCardType = "QT"; break;
            }

            //1 男  2女
            if (data_body.patientSex == "1")
            {
                data_body.patientSex = "男";
            }
            else
            {
                data_body.patientSex = "女";
            }

            data_body.pdfBase64 = base64file.oridata;

            string inputFormat = "MM dd yyyy h:mmtt";
            string outputFormat = "yyyy-MM-dd HH:mm:ss";

            string cleanedInput = data_body.recipeTime.Replace("  ", " ");

            DateTime dateTime;
            if (DateTime.TryParseExact(cleanedInput, inputFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                string output = dateTime.ToString(outputFormat);
                //yyyy-MM-dd HH:mm:ss
                data_body.recipeTime = output;
            }
            else
            {
                Console.WriteLine("Failed to parse input date");
            }

            var dataParams = new Params<Data_Head, Pdf_Body>
            {
                head = new Data_Head
                {
                    clientId = DataConfig.clientId,
                    templateId = "hash",
                    clientSecret = DataConfig.clientSecret,
                    channelID = "",
                    sysTag = "his"
                },
                body = data_body
            };

            var requestParams = new Data_Signature_params<Pdf_Body>
            {
                signType = 0,
                msg = dataParams
            };

            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, requestParams, "数据签名接口（纯数据）");
        }

        /// <summary>
        /// 4.3.5 数据签名接口(EMR(电子病历)签名接口)
        /// </summary>

        [HttpPost]
        public async Task<string> EMR_Signature(string databody)
        {
            var base64file = JsonConvert.DeserializeObject<FileParse>(databody);
            var data_body = JsonConvert.DeserializeObject<EMR_Data>(databody);

            //data_body.urId = Guid.NewGuid().ToString();
            ApiResponse<HashData> hash = JsonConvert.DeserializeObject<ApiResponse<HashData>>(await Data_Hash(base64file.oridata));

            data_body.hashType = hash.Data.hashType;
            data_body.hashValue = hash.Data.hashValue;
            switch (data_body.patientCardType)
            {
                case "1": data_body.patientCardType = "HZ"; break;
                case "2": data_body.patientCardType = "SF"; break;
                case "3": data_body.patientCardType = "QT"; break;
            }

            switch (data_body.patientSex)
            {
                case "1": data_body.patientSex = "男"; break;
                case "2": data_body.patientSex = "女"; break;
                default: data_body.patientSex = "未知"; break;
            }

            string inputFormat = "MM dd yyyy h:mmtt";
            string outputFormat = "yyyy-MM-dd HH:mm:ss";

            string cleanedInput = data_body.recipeTime.Replace("  ", " ");

            DateTime dateTime;
            if (DateTime.TryParseExact(cleanedInput, inputFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                string output = dateTime.ToString(outputFormat);
                //yyyy-MM-dd HH:mm:ss
                data_body.recipeTime = output;
            }

            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.EMR_Signature;

            var dataParams = new Params<Data_Head, EMR_Data>
            {
                head = new Data_Head
                {
                    clientId = DataConfig.clientId,
                    templateId = "hash",
                    clientSecret = DataConfig.clientSecret,
                    channelID = "",
                    sysTag = "his"
                },
                body = data_body
            };

            var requestParams = new Data_Signature_params<EMR_Data>
            {
                signType = 0,
                msg = dataParams
            };
            var d = JsonConvert.SerializeObject(requestParams);
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, requestParams, "数据签名接口(EMR(电子病历)签名接口");
        }

        /// <summary>
        /// 4.3.7 签名状态查询接口(按订单查询)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Signature_State(string uniqueId, string openid)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.Signature_State;

            var requestParams = new Params<head, Signature_body>
            {
                head = new head
                {
                    clientId = DataConfig.clientId,
                    clientSecret = DataConfig.clientSecret,
                },
                body = new Signature_body
                {
                    uniqueId = uniqueId,
                    openId = openid,
                }
            };

            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, requestParams, "签名状态查询接口(按订单查询)");
        }

        /// <summary>
        /// 5.1.1 签名状态查询接口(按时间查询)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Signature_State_Select(DateTime startDate, DateTime endDate, int pageNum, int pageSize)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.Signature_State_Select;
            var requestParams = new Params<Signature_State_head, Signature_State_body>
            {
                head = new Signature_State_head
                {
                    clientId = DataConfig.clientId,
                },
                body = new Signature_State_body
                {
                    startDate = startDate,
                    endDate = endDate,
                    pageNum = pageNum,
                    pageSize = pageSize,
                }
            };
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, requestParams, "签名状态查询接口");
        }

        /// <summary>
        /// 5.1.2获取签名结果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Get_Signature(string uniqueId)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.Get_Signature;

            var requestParams = new Params<head, Get_Signature>
            {
                head = new head
                {
                    clientId = DataConfig.clientId,
                    clientSecret = DataConfig.clientSecret
                },
                body = new Get_Signature
                {
                    isP1 = true,
                    uniqueId = uniqueId,
                }
            };
            var d = JsonConvert.SerializeObject(requestParams);
            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, requestParams, "获取签名结果");
        }

        /// <summary>
        /// 5.1.3 查询医生是否有待签数据接口
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Get_Doc_State(string openid)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.Get_Doc_State;

            //get 请求参数方法
            api_url = api_url + "?openId=" + openid;

            return HttpClientHelper.Execute(HttpType.HttpGet, api_url, null, null, "查询医生是否有待签数据接口");
        }

        /// <summary>
        /// 5.1.4 删除待签数据接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Doc_Delete(string uniqueId)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.Doc_Delete;

            var requestParams = new Params<Del_Doc_head, Del_Doc_body>
            {
                head = new Del_Doc_head
                {
                    clientId = DataConfig.clientId,
                },
                body = new Del_Doc_body
                {
                    uniqueId = uniqueId,
                }
            };

            return HttpClientHelper.Execute(HttpType.HttpGet, api_url, null, requestParams, "删除待签数据接口");
        }

        /// <summary>
        /// 6.1.1 验证签名结果服务接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ValiSignature(string databody)
        {
            var base64file = JsonConvert.DeserializeObject<FileParse>(databody);

            var data_body = JsonConvert.DeserializeObject<EMR_Data>(databody);

            ApiResponse<HashData> hash = JsonConvert.DeserializeObject<ApiResponse<HashData>>(await Data_Hash(base64file.oridata));

            data_body.hashType = hash.Data.hashType;
            data_body.hashValue = hash.Data.hashValue;
            switch (data_body.patientCardType)
            {
                case "1": data_body.patientCardType = "HZ"; break;
                case "2": data_body.patientCardType = "SF"; break;
                case "3": data_body.patientCardType = "QT"; break;
            }
            //1 男  2女
            if (data_body.patientSex == "1")
            {
                data_body.patientSex = "男";
            }
            else
            {
                data_body.patientSex = "女";
            }

            string inputFormat = "MM dd yyyy h:mmtt";
            string outputFormat = "yyyy-MM-dd HH:mm:ss";

            string cleanedInput = data_body.recipeTime.Replace("  ", " ");

            DateTime dateTime;
            if (DateTime.TryParseExact(cleanedInput, inputFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                string output = dateTime.ToString(outputFormat);
                //yyyy-MM-dd HH:mm:ss
                data_body.recipeTime = output;
            }

            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.ValiSignature;

            var requestParams = new Params<Vali_Params, EMR_Data>
            {
                head = new Vali_Params
                {
                    clientId = DataConfig.clientId,
                    clientSecret = DataConfig.clientSecret,
                    uniqueId = base64file.uniqueId,
                    templateId = "hash",
                },
                body = data_body
            };

            var data = JsonConvert.DeserializeObject<ApiResponse<Vali_State>>(HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, requestParams, "验证签名结果服务接口"));
            Inter.Dto.Message m = new Inter.Dto.Message();
            if (data.Status == "0" && data.Data.verifyResult == true)
            {
                m.msg = "成功";
                m.status = "0";
                return JsonConvert.SerializeObject(m);
            }
            if (data.Status == "0" && data.Data.verifyResult == false)
            {
                m.msg = "用户信息不一致";
                m.status = "1";
                return JsonConvert.SerializeObject(m);
            }
            else
            {
                m.msg = data.Message;
                m.status = "1";
                return JsonConvert.SerializeObject(m);
            }
        }

        /// <summary>
        /// 6.1.2 验证本地时间戳接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ValiTime(string uniqueId)
        {
            // 定义要调用的API接口的URL
            string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.ValiTime;

            var requestParams = new Params<head, Del_Doc_body>
            {
                head = new head
                {
                    clientId = DataConfig.clientId,
                    clientSecret = DataConfig.clientSecret,
                },
                body = new Del_Doc_body
                {
                    uniqueId = uniqueId,
                }
            };

            return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, requestParams, "验证本地时间戳接口");
        }

        /// <summary>
        /// 循环查询签名结果
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Select_state(string ids)
        {
            try
            {
                var data = ids.Split(",");
                foreach (var item in data)
                {
                    ApiResponse<Get_Signature_Res> a = JsonConvert.DeserializeObject<ApiResponse<Get_Signature_Res>>(await Get_Signature(item));
                    if (a.Status == "1")
                    {
                        //拒签
                        BackData backData = new BackData();
                        backData.uniqueid = item;
                        backData.sign_value = a.Data.failReason;   // 拒签的原因
                        backData.stamp = a.Data.stamp;
                        backData.cret = a.Data.cert;
                        backData.signtime = a.Data.signTime;
                        backData.sign_state = "3";
                        SignBackData(JsonConvert.SerializeObject(backData));//需要放值
                    }
                    else if (a.Status == "0" && a.Data.signTime != null)
                    {
                        //自动签名成功
                        BackData backData = new BackData();
                        backData.uniqueid = item;
                        backData.sign_value = a.Data.signedData;
                        backData.stamp = a.Data.stamp;
                        backData.cret = a.Data.cert;
                        backData.signtime = a.Data.signTime;
                        backData.sign_state = "2";
                        SignBackData(JsonConvert.SerializeObject(backData));//需要放值
                    }
                    else
                    {
                        //没有改变
                    }
                }
                return "true";
            }
            catch (Exception)
            {
                return "false";
                throw;
            }
        }

        #region 废弃代码

        /// <summary>
        /// 4.3.6 商务办公类数据签名接口
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //public async Task<string> Office_Signature(string url)
        //{
        //    var office_body = HttpClientHelper.Execute<Office_body>(HttpType.HttpPost, url, null, null, "获取商务办公类数据签名接口");
        //    // 定义要调用的API接口的URL
        //    string api_url = DataConfig.httpdata + DataConfig.domainName + interJsonParse.Office_Signature;
        //    var dataParams = new Params<Office_head, Office_body>
        //    {
        //        head = new Office_head
        //        {
        //            clientId = DataConfig.clientId,
        //            clientSecret = DataConfig.clientSecret,
        //            templateId = "personPdf",
        //            sysTag = "his",
        //            selfSign = ""
        //        },
        //        body = office_body
        //    };

        //    var requestParams = new Office_Signature_params
        //    {
        //        signType = 0,
        //        msg = dataParams
        //    };

        //    return HttpClientHelper.Execute(HttpType.HttpPost, api_url, null, requestParams, "商务办公类数据签名接口结果");
        //}

        #endregion 废弃代码
    }
}