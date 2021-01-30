using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

/// <summary>
/// 检查并下载 Default 数据
/// </summary>
public class DefaultDataDownloader : MonoBehaviour
{
    public Text tip; //提示
    public bool downloadWithoutChecking = false;
    private float downloadPercent = 0f;
    private const string LINK = "http://zl.lovezr.cn/api/Default.zip";
    private WaitingScreen screen;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (!CheckDefaultFiles() || downloadWithoutChecking)
            WaitingScreen.LoadWaitingScreen(delegate() {
                StartCoroutine(DownloadData(LINK, Path.Combine(FileLoader.DataRoot, "temp.zip"))); 
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
        bool flag = Directory.Exists(FileLoader.DefaultDataPath);
        if (!flag)
            Debug.LogWarning("DefaultData missing!(" + FileLoader.DefaultDataPath + ")");
        return flag;
    }

    private IEnumerator DownloadData(string url, string downloadTo)
    {
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
            //tip.text = "下载已完成，正在解压..."; //没必要，显示不出来
            Util.Unzip(FileLoader.DataRoot, buffer);
            screen.SetText("解压完成，请重启游戏");
        }
        catch (Exception e)
        {
            screen.SetText("<color=red>发生错误：" + "\n" + e.Message + "</color>");
        }
        finally
        {
            //删除临时文件
            try { File.Delete(downloadTo); }
            catch { }

            Destroy(gameObject);
        }

    }

    //TODO 分离此方法到单独的类里面
    public string HttpGetString(string url)
    {
        WWW www = new WWW(url);
        while (!www.isDone) { };
        return System.Text.Encoding.Default.GetString(www.bytes);
    }
}
