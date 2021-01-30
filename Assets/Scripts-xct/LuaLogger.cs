using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LuaLogger : MonoBehaviour {

    private Text text;
    private Queue<string> queue;
    public int maxCount = 100;
    public bool syncToUnity = false;

	void Awake ()
    {
        queue = new Queue<string>();
        text = gameObject.GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Log(object message)
    {
        string s = "[Info]" + message.ToString();
        if (syncToUnity)
            Debug.Log(s);
        queue.Enqueue(s);
        UpdateText();
    }

    public void Log(params object[] message)
    {
        foreach(object obj in message)
        {
            Log(obj);
        }
    }

    public void LogWarning(object message)
    {
        string s = string.Format("<color=yellow>[Warn]{0}</color>", message.ToString());
        if (syncToUnity)
            Debug.LogWarningFormat(s);
        queue.Enqueue(s);
        UpdateText();
    }

    public void LogError(object message)
    {
        string s = string.Format("<color=red>[Error]{0}</color>", message.ToString());
        if (syncToUnity)
            Debug.LogErrorFormat(s);
        queue.Enqueue(s);
        UpdateText();
    }

    public void Clear()
    {
        queue.Clear();
        UpdateText();
    }
    

    private void UpdateText()
    {
        //如果当前行数大于最大行数
        if (queue.Count > maxCount)
            queue.Dequeue();

        //将所有行转换为文本
        StringBuilder sb = new StringBuilder();
        foreach(string s in queue)
        {
            sb.AppendLine(s);
            //Debug.Log(s);
        }
        Debug.Log(sb);
        text.text = sb.ToString();
        
    }
}
