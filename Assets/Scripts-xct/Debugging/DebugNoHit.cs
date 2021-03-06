﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 锁血模式
/// </summary>
public class DebugNoHit : AbstractDebugger
{
    public static DebugNoHit instance = null;

    void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if(GlobalSettings.settings.noHit)
            DebugInfoScreen.instance.EditKVLine("Hits", GlobalSettings.settings.hits + " (= -" + GlobalSettings.settings.totalHurtHP + " HP)");
    }

    public override void TryEnable()
    {
        if (!GlobalSettings.settings.debug || !GlobalSettings.settings.noHit)
            return;
        else
            Enable();
    }

    public override void Enable()
    {
        GlobalSettings.settings.noHit = true;
        DebugInfoScreen.instance.NewKVLine("NoHit", true);
        DebugInfoScreen.instance.NewKVLine("Hits", 0);
    }

    public override void Disable()
    {
        GlobalSettings.settings.noHit = false;
        DebugInfoScreen.instance.RemoveKVLine("NoHit");
        DebugInfoScreen.instance.RemoveKVLine("Hits");
    }

    public override void TryUpdate()
    {
        if (GlobalSettings.settings.noHit)
            Enable();
        else
            Disable();
    }
}
