using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inter.Util
{
    public class BackData
    {
        /// <summary>
        ///  签名数据唯一标识
        /// </summary>
        public string uniqueid { get; set; }

        /// <summary>
        /// 签名值
        /// </summary>
        public string sign_value { get; set; }

        /// <summary>
        /// 签名图
        /// </summary>
        public string stamp { get; set; }

        /// <summary>
        /// 证书序列号
        /// </summary>
        public string cret { get; set; }

        /// <summary>
        /// 签名时间
        /// </summary>
        public string signtime { get; set; }

        /// <summary>
        /// 签名状态
        /// </summary>

        public string sign_state { get; set; }
    }
}