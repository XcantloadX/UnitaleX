using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnitaleX.Network
{
	public class HttpUtil
	{
        /// <summary>
        /// 发送 HTTP GET 请求
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>返回的字符串</returns>
        public static string Get(string url)
        {
            WWW www = new WWW(url);
            while (!www.isDone) { }
            return System.Text.Encoding.UTF8.GetString(www.bytes);
        }

	}
}
