using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSDisplay : AbstractDebugger
{
    public float updateTime = 1f;

    private int frames = 0;
    private float timer = 0;
    private float fps = 0;
    private float runTime = 0;
    private static bool inited = false;

    void Start()
    {
    }

    void Update()
    {
        if (!GlobalSettings.settings.debug)
            return;

        frames++;
        timer += Time.deltaTime;

        //计算 fps
        if(timer > updateTime)
        {
            fps = (float)Math.Round(frames / timer, 1);
            DebugInfoScreen.instance.EditKVLine("Fps", fps);

            frames = 0;
            timer = 0;
        }

        if(fps < 10)
            DebugInfoScreen.instance.EditKVLine("Fps", "<color=red>" + fps + " (May cause problem!)</color>");
        else if(fps < 30)
            DebugInfoScreen.instance.EditKVLine("Fps", "<color=yellow>" + fps + "</color>");
        else
            DebugInfoScreen.instance.EditKVLine("Fps", fps);

        DebugInfoScreen.instance.EditKVLine("Time", System.Math.Round(Time.time, 1));
    }

    public override void Disable()
    {
        DebugInfoScreen.instance.RemoveKVLine("Fps");
        DebugInfoScreen.instance.RemoveKVLine("Time");
    }

    public override void Enable()
    {
        inited = true;
        GameObject.DontDestroyOnLoad(this);
        DebugInfoScreen.instance.NewKVLine("Fps", 0);
        DebugInfoScreen.instance.NewKVLine("Time", 0);
    }
}
