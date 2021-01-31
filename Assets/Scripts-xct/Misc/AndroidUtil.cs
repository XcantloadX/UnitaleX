using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidUtil
{
    public static void Toast(string text)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"); ;
        AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
        AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
        currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            Toast.CallStatic<AndroidJavaObject>("makeText", context, text, Toast.GetStatic<int>("LENGTH_SHORT")).Call("show");
        }
        ));
#endif
    }
}
