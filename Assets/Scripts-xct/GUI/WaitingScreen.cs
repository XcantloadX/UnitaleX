using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaitingScreen : MonoBehaviour {

    public delegate void OnStart();
    /// <summary>
    /// 当 WaitingScreen 加载完成时调用
    /// </summary>
    public static event OnStart EventOnStart;
    public static WaitingScreen Instance = null;

    private static string message;
    private Text text;
    
	void Start () {
        Instance = this;
        text = GameObject.Find("Message").GetComponent<Text>();
        SetText(message);

        EventOnStart();
	}

    /// <summary>
    /// 加载等待屏幕
    /// </summary>
    /// <param name="start">等待屏幕加载完成后调用</param>
    public static void LoadWaitingScreen(OnStart start)
    {
        EventOnStart += start;
        SceneSystem.Load("WaitingScreen");
    }

    /// <summary>
    /// 以指定提示信息加载等待屏幕
    /// </summary>
    /// <param name="start">等待屏幕加载完成后调用</param>
    /// <param name="message">待显示信息</param>
    public static void LoadWaitingScreen(OnStart start, string message)
    {
        WaitingScreen.message = message;
        LoadWaitingScreen(start);
    }

    /// <summary>
    /// 设置提示信息
    /// </summary>
    public void SetText(string str)
    {
        text.text = str;
    }
}
