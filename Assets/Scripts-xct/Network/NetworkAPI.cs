using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;

namespace UnitaleX.Network
{
    public class NetworkAPI
    {
        public const string CHECK_UPDATE = "https://cdn.jsdelivr.net/gh/XcantloadX/unitalex_files@master/latest_version.json";
        public const string DEFAULT_DATA_BACKUP2 = "https://cdn.jsdelivr.net/gh/XcantloadX/unitalex_files@master/default/default_files.json";
        public const string DEFAULT_DATA_BACKUP1 = "https://cdn.jsdelivr.net/gh/XcantloadX/unitalex_files@latest/default/default_files.json";
        public const string DEFAULT_DATA = "https://raw.githubusercontent.com/XcantloadX/unitalex_files/master/default/default_files.json";

        /// <summary>
        /// 获取当前版本的 Default 文件下载地址
        /// </summary>
        public static string GetDefaultDataUrl()
        {
            string str = HttpUtil.Get(DEFAULT_DATA);
            string url, directUrl;
            
            if (string.IsNullOrEmpty(str))
            {
                Debug.LogWarning("Could not connect to raw.githubusercontent.com. Used cdn.jsdelivr.net instead.");
                str = HttpUtil.Get(DEFAULT_DATA_BACKUP1);
            }

            
            DefaultFiles defaultFile = JsonMapper.ToObject<DefaultFiles>(str);
            DefaultFiles.File[] files = defaultFile.file;

            foreach (DefaultFiles.File f in files)
            {
                if (f.version.Contains(Application.version))
                {
                    url = defaultFile.baseUrl + f.url;
                    WWW www = new WWW(url);
                    while (!www.isDone) { }

                    if (www.responseHeaders["STATUS"].Contains("500") && (int)JsonMapper.ToObject(www.text)["code"] != 200)
                    {
                        throw new RemoteServerException("远程服务器出错。\n" + www.responseHeaders["STATUS"]);
                    }

                    //检查 location 重定向
                    if (www.responseHeaders.ContainsKey("LOCATION"))
                    {
                        directUrl = www.responseHeaders["LOCATION"];
                        if (!string.IsNullOrEmpty(directUrl))
                            return directUrl;
                    }
                }
            }

            Debug.LogWarning("未找到 " + Application.version + " 的 Default 文件！使用备用链接代替");
            return "https://note.youdao.com/yws/api/personal/file/WEBbf6fb1628fd1f80c245180ff0d69577b?method=download&shareKey=fa96ed041ec60e4457cee81982f76209";
        }

        public class RemoteServerException : Exception
        {
            private string statusCode;
            private string message;

            public RemoteServerException(){ }

            public RemoteServerException(string message)
            {
                this.message = message;
            }

            public override string Message
            {
                get
                {
                    return "远程服务器返回错误：" + message;
                }
            }
        }

        class DefaultFiles
        {
            public class File
            {
                public string version;
                public string name;
                public string url;
            }

            public string baseUrl;
            public File[] file;
        }

        class LatestVersion
        {
            public string version;
            public string description;
            public string url;
        }
    }
}

