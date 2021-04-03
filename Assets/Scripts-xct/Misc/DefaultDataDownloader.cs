using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

namespace UnitaleX.Network
{
    /// <summary>
    /// 检查并下载 Default 数据
    /// </summary>
    public class DefaultDataDownloader : MonoBehaviour
    {
        public Text tip; //提示
        public bool downloadWithoutChecking = false;
        private float downloadPercent = 0;
        private WaitingScreen screen;

        void Start()
        {
            DontDestroyOnLoad(gameObject);
#if UNITY_EDITOR
            if (!CheckDefaultFiles() || downloadWithoutChecking)
#else
            if (!CheckDefaultFiles())
#endif
                WaitingScreen.LoadWaitingScreen(delegate()
                {
                    try
                    {
                        StartCoroutine(DownloadData(Path.Combine(FileLoader.DataRoot, "temp.zip")));
                    }
                    catch (System.Exception ex)
                    {
                        WaitingScreen.Instance.SetText("<color=red>" + ex.ToString() + "</color>");
                        throw ex;
                    }

                });
            else
                Destroy(gameObject);
        }

        /// <summary>
        /// 检查 Default 数据是否已下载
        /// </summary>
        /// <returns></returns>
        private bool CheckDefaultFiles()
        {
            bool flag = Directory.Exists(FileLoader.DefaultDataPath) && Directory.Exists(Path.Combine(FileLoader.DefaultDataPath, "Sprites"));
            if (!flag)
                Debug.Log("DefaultData not found at " + FileLoader.DefaultDataPath + ". Try to download.");
            return flag;
        }

        private IEnumerator DownloadData(string downloadTo)
        {
            //获取下载连接
            WaitingScreen.Instance.SetText("正在连接服务器...");
            yield return null; //等待一帧，让提示显示出来
            string url = NetworkAPI.GetDefaultDataUrl();
            if (string.IsNullOrEmpty(url))
                throw new Exception("链接为空");

            screen = WaitingScreen.Instance;
            Debug.Log("Started file downloading from " + url);
            WWW www = new WWW(url);
            while (!www.isDone)
            {
                downloadPercent = www.progress;
                screen.SetText("正在下载数据文件： " + System.Math.Round(downloadPercent, 2) * 100 + " %");
                yield return null;
            }
            downloadPercent = www.progress;
            byte[] buffer = www.bytes;

            try
            {
                if (!Directory.Exists(FileLoader.DefaultDataPath))
                    Directory.CreateDirectory(FileLoader.DefaultDataPath);

                Util.Unzip(FileLoader.DefaultDataPath, buffer);
                screen.SetText("解压完成，请重启游戏");
            }
            catch (Exception e)
            {
                screen.SetText("<color=red>发生错误：" + "\n" + e.Message + "\nTrace: " + e.StackTrace + "</color>");
            }
            finally
            {
                //删除临时文件
                try { File.Delete(downloadTo); }
                catch { }

                Destroy(gameObject);
            }

        }
    }
}

