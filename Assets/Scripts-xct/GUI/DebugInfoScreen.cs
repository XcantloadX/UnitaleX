using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 显示在屏幕左上方的 Debug 信息（就像 Minecraft 一样）
/// </summary>
public class DebugInfoScreen : MonoBehaviour {

    public static DebugInfoScreen instance = null;
    private Dictionary<string, object> infos = new Dictionary<string, object>(10);
    private Dictionary<int, string> lines = new Dictionary<int, string>(10);
    private GUIStyle style = null;
    private System.Random rnd = new System.Random();

	void Start()
    {
        if(instance != null) //保证单一实例
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        CheckDebuggers();
	}

    private void CheckDebuggers()
    {
        AbstractDebugger[] debuggers = GameObject.FindObjectsOfType<AbstractDebugger>();
        foreach (AbstractDebugger debugger in debuggers)
        {
            debugger.TryEnable();
        }
    }

    public void UpdateDebuggers()
    {
        AbstractDebugger[] debuggers = GameObject.FindObjectsOfType<AbstractDebugger>();
        foreach (AbstractDebugger debugger in debuggers)
        {
            debugger.TryUpdate();
        }
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

        System.Text.StringBuilder sb = new System.Text.StringBuilder(10);
        foreach (KeyValuePair<string, object> kv in infos)
        {
            sb.AppendLine(kv.Key + ": " + kv.Value);
        }
        sb.AppendLine();
        foreach(KeyValuePair<int, string> kv in lines)
        {
            sb.AppendLine(kv.Value);
        }

        style = GUI.skin.box;
        style.fontSize = 20;
        style.normal.textColor = Color.white;
        style.richText = true;
        style.alignment = TextAnchor.UpperLeft;

        GUILayout.Box(sb.ToString(), style);
    }

    /// <summary>
    /// 新增一条 Debug 信息
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="value">值</param>
    public void NewKVLine(string name, object value)
    {
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
    public void EditKVLine(string name, object value)
    {
        infos[name] = value;
    }

    /// <summary>
    /// 移除一条信息
    /// </summary>
    /// <param name="name">名称</param>
    public void RemoveKVLine(string name)
    {
        infos.Remove(name);
    }

    public int NewLine(string text)
    {
        int id = rnd.Next();
        lines.Add(id, text);
        return id;
    }

    public void EditLine(int id, string text)
    {
        if (!lines.ContainsKey(id))
            return;
        lines[id] = text;
    }

    public void RemoveLine(int id)
    {
        if (lines.ContainsKey(id))
            lines.Remove(id);
    }
}
