using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 错误通知
/// </summary>
public class ErrorNotice : MonoBehaviour
{
    private string errMsg = "";
    private float timer = 0;
    private int id = 0;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);
        Application.RegisterLogCallback(Handle); //注册回调
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= 0)
            timer -= Time.deltaTime;

        if(timer <= 0)
        {
            DebugInfoScreen.instance.RemoveLine(id);
        }
    }

    void OnGUI()
    {
        //显示错误信息
        if(errMsg != "" && timer >= 0)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 20;
            style.normal.textColor = Color.red;
            style.wordWrap = true; //自动换行
            
            //GUI.Label(new Rect(0, Screen.height - 40, Screen.width, 40), "错误：" + errMsg, style);
            //GUILayout.Label("错误：" + errMsg, style);
        }
    }

    private void Handle(string logString, string stackTrace, LogType type)
    {
        if ((type == LogType.Error || type == LogType.Exception) && timer <= 0)
        {
            errMsg = logString + "\n" + stackTrace;
            timer = 6f;
            id = DebugInfoScreen.instance.NewLine("<color=red>" + errMsg + "</color>");
        }
    }
}
