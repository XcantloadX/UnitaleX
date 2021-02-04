using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 锁血模式
/// </summary>
public class DebugNoHit : MonoBehaviour
{
    public static DebugNoHit instance = null;

    void Start()
    {
        DebugInfoScreen.instance.AddNewLine("Hits", 0);
        if (GlobalSettings.settings.debug)
        {
            DebugInfoScreen.instance.AddNewLine("NoHit", true);
            GlobalSettings.settings.noHit = true;
        }
    }

    void Update()
    {
        DebugInfoScreen.instance.EditLine("Hits", GlobalSettings.settings.hits + " (= -" + GlobalSettings.settings.totalHurtHP + " HP)");
    }

    void OnDisable()
    {
        DebugInfoScreen.instance.RemoveLine("NoHit");
        DebugInfoScreen.instance.RemoveLine("Hits");
    }

    public static void Enable()
    {
        if (!GlobalSettings.settings.debug)
        {
            GlobalSettings.settings.noHit = false;
            return;
        }

        GlobalSettings.settings.noHit = true;
        GameObject obj = new GameObject("NoHitMode");
        GameObject.DontDestroyOnLoad(obj);
        instance = obj.AddComponent<DebugNoHit>();
    }

    public static void Disable()
    {
        GlobalSettings.settings.noHit = false;
        if(instance != null)
            GameObject.Destroy(instance.gameObject);
    }
}
