using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 显示在屏幕左上方的 Debug 信息（就像 Minecraft 一样）
/// </summary>
public class DebugInfoScreen : MonoBehaviour {

    public static DebugInfoScreen instance = null;
    private List<string> lines = new List<string>(10);
    private Dictionary<string, object> infos = new Dictionary<string, object>(10);
    private GUIStyle style = new GUIStyle();

	void Start()
    {
        if(instance != null) //保证单一实例
        {
            Destroy(this);
            return;
        }

        instance = this;
        style.fontSize = 20;
        style.normal.textColor = Color.white;
        style.richText = true;

        //TODO 这种方法没有透明度
        //Texture2D t = new Texture2D(1, 1);
        //t.SetPixel(0, 0, new Color(0, 0, 0, 10));
        //t.Apply();
        //style.normal.background = t;
        CheckDebuggers();
        
	}

    private void CheckDebuggers()
    {
        if (GlobalSettings.settings.noHit)
            DebugNoHit.Enable();
    }
	
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.F3))
            GlobalSettings.settings.debug = !GlobalSettings.settings.debug;

	}

    void OnGUI()
    {
        if (!GlobalSettings.settings.debug)
            return;
        GUILayout.BeginArea(new Rect(0, 0, 400, Screen.height));

        foreach (KeyValuePair<string, object> kv in infos)
        {
            GUILayout.Label(kv.Key + ": " + kv.Value, style);
        }


        GUILayout.EndArea();
    }

    /// <summary>
    /// 新增一条 Debug 信息
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="value">值</param>
    public void AddNewLine(string name, object value)
    {
        //Debug.Log(name + "|" + infos.ContainsKey(name));
        if (infos.ContainsKey(name))
            return;
        infos.Add(name, value);
    }

    /// <summary>
    /// 修改一条 Debug 信息的值，修改之前请确保已经存在此条信息
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="value">值</param>
    /// <param name="color">颜色</param>
    public void EditLine(string name, object value)
    {
        infos[name] = value;
    }

    /// <summary>
    /// 移除一条信息
    /// </summary>
    /// <param name="name">名称</param>
    public void RemoveLine(string name)
    {
        infos.Remove(name);
    }
}
