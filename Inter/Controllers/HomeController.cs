using Inter.Util;
using InterFace;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace Inter.Controllers
{
    public class HomeController : Controller
    {
        // GET: HomeController
        public ActionResult Sign()
        {
            return View();
        }

        /// <summary>
        /// 获取数据文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetJson(string id, string SignatureType)
        {
            switch (SignatureType)
            {
                case "1": return GetData(id, AppSettingsHelper.Configuration["YWX:ResponseDataUrl"]); //医网信
                case "2": return GetData(id, AppSettingsHelper.Configuration["XTQM:ResponseDataUrl"]); // 协同
                default: return "";
            }
        }

        public string GetData(string id, string api_url)
        {
            //get 请求参数方法
            api_url = api_url + "?operation=getsigndata&id=" + id;

            using (HttpClient httpClient = new HttpClient())
            {
                // 创建HTTP请求
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, api_url);

                // 发送HTTP请求，并等待响应
                HttpResponseMessage response = httpClient.SendAsync(request).Result;

                // 检查响应状态码
                if (response.IsSuccessStatusCode)
                {
                    // 读取响应内容
                    string responseContent = response.Content.ReadAsStringAsync().Result;
                    LogHelper.Loging("Request", responseContent, "前端返回签名的数据");
                    return responseContent;
                }
                else
                {
                    // 处理请求失败的情况
                }
                return null;
            }
        }
    }
}