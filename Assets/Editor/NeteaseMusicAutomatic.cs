using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEditor;

#if UNITY_5 && UNITY_EDITOR_WIN
//来回切网易云音乐实在是太麻烦了
//Auto pause netease music when play. Unity 5 only.
[InitializeOnLoadAttribute]
public static class NeteaseMusicAutomatic
{
    private static bool isQQMusicRunning = false;
    private static bool isNeteaseMusicRunning = false;
    private const int KEYEVENTF_KEYUP = 2;

    [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
    private static extern void keybd_event(int bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

    //每重新编译一次，这个方法就会被执行
    static NeteaseMusicAutomatic()
    {
        EditorApplication.playmodeStateChanged += PlayModeStateChanged;

        //判断网易云音乐/QQ音乐是否运行
        System.Diagnostics.Process[] processList = System.Diagnostics.Process.GetProcesses();
        foreach (System.Diagnostics.Process process in processList)
        {
            if (process.ProcessName.ToLower() == "cloudmusic" || process.ProcessName.ToLower() == "justlisten")
            {
                isNeteaseMusicRunning = true;
                Debug.Log("Netease Music is running.");

            }
            else if (process.ProcessName.ToLower() == "qqmusic")
            {
                isQQMusicRunning = true;
                Debug.Log("QQ Music is running.");
            }
        }
        if (!isQQMusicRunning && !isNeteaseMusicRunning)
            Debug.Log("No supported music app is running.");

        Debug.Log("MusicSwticher initialized.");
    }

    private static void PlayModeStateChanged()
    {
        if (isNeteaseMusicRunning && (EditorState.willEnterPlayMode || EditorState.willExitPlayMode))
        {
            keybd_event(17, 0, 0, 0); //Ctrl = 17
            keybd_event(18, 0, 0, 0); //Alt = 18
            keybd_event(80, 0, 0, 0); //P = 80
            keybd_event(17, 0, KEYEVENTF_KEYUP, 0);
            keybd_event(18, 0, KEYEVENTF_KEYUP, 0);
            keybd_event(80, 0, KEYEVENTF_KEYUP, 0);
        }
        else if (isQQMusicRunning && (EditorState.willEnterPlayMode || EditorState.willExitPlayMode))
        {
            keybd_event(17, 0, 0, 0); //Ctrl = 17
            keybd_event(18, 0, 0, 0); //Alt = 18
            keybd_event(80, 0, 0, 0); //F5 = 116
            keybd_event(17, 0, KEYEVENTF_KEYUP, 0);
            keybd_event(18, 0, KEYEVENTF_KEYUP, 0);
            keybd_event(116, 0, KEYEVENTF_KEYUP, 0);
        }
    }
}

/// <summary>
/// 获取 Editor 的运行状态，Unity 5 以上不需要
/// </summary>
public static class EditorState {

    public static bool willEnterPlayMode {
        get { return !EditorApplication.isPlaying
                     && EditorApplication.isPlayingOrWillChangePlaymode; }
    }

    public static bool didEnterPlayMode {
        get { return EditorApplication.isPlaying
                     && EditorApplication.isPlayingOrWillChangePlaymode; }
    }

    public static bool willExitPlayMode {
        get { return EditorApplication.isPlaying
                     && !EditorApplication.isPlayingOrWillChangePlaymode; }
    }

    public static bool didExitPlayMode {
        get { return !EditorApplication.isPlaying
                     && !EditorApplication.isPlayingOrWillChangePlaymode; }
    }
}

#endif

