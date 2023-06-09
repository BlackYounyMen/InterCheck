using Inter.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace RestSharpDemo.Web
{
    /// <summary>
    /// Http请求类型:Post/Put/Get/Delete
    /// </summary>
    public enum HttpType
    {
        /// <summary>
        /// HttpGet 请求方式
        /// </summary>
        HttpGet,

        /// <summary>
        /// HttpDelete 请求方式
        /// </summary>
        HttpPost,

        /// <summary>
        /// HttpDelete 请求方式
        /// </summary>
        HttpPut,

        /// <summary>
        /// HttpDelete 请求方式
        /// </summary>
        HttpDelete
    }

    /// <summary>
    /// 直接通过Http协议调用api，不会产生跨域问题
    /// </summary>
    public static class HttpClientHelper
    {
        /// <summary>
        /// 发送Http请求
        /// </summary>
        /// <param name="httpType">请求类型：post、put、get、delete</param>
        /// <param name="url">类似地址： /api/values/get/2</param>
        /// <param name="obj">对象参数</param>
        /// <returns></returns>
        public static string Execute(HttpType httpType, string url, string token = "", object obj = null, string paramsName = null)
        {
            try
            {
                HttpClient hc = new HttpClient();
                if (obj != null)
                {
                    LogHelper.Loging("Response", obj, paramsName);
                }

                //注意事项： 1、端口号跟Api项目的端口号保持一致；2、看清楚是Http请求还是  https 请求
                hc.BaseAddress = new Uri("http://10.168.1.165:8003/");

                //添加token验证
                hc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);
                hc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //将接收到的实体对象序列化为json字符串
                var jsonString = JsonConvert.SerializeObject(obj);
                HttpContent content = new StringContent(jsonString);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                Task<HttpResponseMessage> task = null;
                switch (httpType)
                {
                    case HttpType.HttpGet:
                        task = hc.GetAsync(url);
                        break;

                    case HttpType.HttpPost:
                        task = hc.PostAsync(url, content);
                        break;

                    case HttpType.HttpPut:
                        task = hc.PutAsync(url, content);
                        break;

                    case HttpType.HttpDelete:
                        task = hc.DeleteAsync(url);
                        break;
                }
                task.Wait();
                if (task.Result.IsSuccessStatusCode)
                {
                    var getresultstringTask = task.Result.Content.ReadAsStringAsync();
                    LogHelper.Loging("Request", getresultstringTask, paramsName);

                    getresultstringTask.Wait();
                    return getresultstringTask.Result;
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.Loging("Error", ex, paramsName);

                throw;
            }
        }
    }
}