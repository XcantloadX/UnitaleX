using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单一实例类。挂到物体上，用来保证 DontDestroyOnLoad 的物体不会重复创建。
/// </summary>
public class SingleInstance : LazyScriptBase 
{
    public static int count = 0;
    public static GameObject obj;

	void Awake () 
    {
        //检查重复
        if (SingleInstanceStats.Check(gameObject.name))
        {
            Destroy(gameObject);
            return;
        }
        
        //计数
        SingleInstanceStats.ObjectCreated(gameObject.name);
	}

    private void DestroySelf()
    {
        List<Transform> list = new List<Transform>(transform.childCount);
        for (int i = 0; i < list.Count; i++)
            list.Add(transform.GetChild(i));
        foreach (Transform t in list)
            Destroy(t.gameObject);
        Destroy(gameObject);
    }

    void OnDestroy()
    {

    }
}

public class SingleInstanceStats
{
    public static Dictionary<string, bool> countByName = new Dictionary<string, bool>(10); //对应的 GameObject 是否已创建

    /// <summary>
    /// 当新实例创建时调用，用于计数
    /// </summary>
    public static void ObjectCreated(string name)
    {
        if (!countByName.ContainsKey(name))
            countByName.Add(name, true);
        else
            countByName[name] = true;
    }

    /// <summary>
    /// 检查是否重复创建（如果为真，则表示重复创建了）
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool Check(string name)
    {
        if (!countByName.ContainsKey(name))
            return false;
        else
            return countByName[name];
    }

    /// <summary>
    /// 当实例被销毁时调用
    /// </summary>
    /// <param name="name">GameObject 名称</param>
    public static void ObjectDestroyed(string name)
    {
        if (countByName.ContainsKey(name))
            countByName[name] = false;
    }
}
