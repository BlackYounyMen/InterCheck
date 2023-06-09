using System;

namespace Params.YWX
{
    /// <summary>
    /// 公共类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T> : ApiRes where T : class
    {
        /// <summary>
        /// 返回结果
        /// </summary>
        public T Data { get; set; }
    }

    /// <summary>
    /// 返回单独值
    /// </summary>
    public class ApiResponseCode : ApiRes
    {
        /// <summary>
        /// 返回结果
        /// </summary>
        public string Data { get; set; }
    }

    /// <summary>
    /// 返回Bool值
    /// </summary>
    public class ApiResponseState : ApiRes
    {
        /// <summary>
        /// 返回结果
        /// </summary>
        public bool Data { get; set; }
    }

    /// <summary>
    /// 公共返回
    /// </summary>
    public class ApiRes
    {
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 状态 0表示成功
        /// </summary>
        public string Status { get; set; }
    }

    /// <summary>
    /// 公共返回
    /// </summary>
    public class SysRes
    {
        /// <summary>
        /// 消息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 状态 0表示成功
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public string data { get; set; }
    }
}