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
        //保证只有一个实例
        if (inited)
            Destroy(gameObject);
    }

    void Update()
    {
        
        frames++;
        timer += Time.deltaTime;

        //计算 fps
        if(timer > updateTime)
        {
            fps = (float)Math.Round(frames / timer, 1);
            DebugInfoScreen.instance.EditLine("Fps", fps);

            frames = 0;
            timer = 0;
        }

        if(fps < 10)
            DebugInfoScreen.instance.EditLine("Fps", "<color=red>" + fps + " (May cause problem!)</color>");
        else if(fps < 30)
            DebugInfoScreen.instance.EditLine("Fps", "<color=yellow>" + fps + "</color>");
        else
            DebugInfoScreen.instance.EditLine("Fps", fps);

        DebugInfoScreen.instance.EditLine("Time", System.Math.Round(Time.time, 1));
    }

    public override void Disable()
    {
        DebugInfoScreen.instance.RemoveLine("Fps");
        DebugInfoScreen.instance.RemoveLine("Time");
    }

    public override void Enable()
    {
        if (inited)
            return;
        inited = true;
        GameObject.DontDestroyOnLoad(this);
        DebugInfoScreen.instance.AddNewLine("Fps", 0);
        DebugInfoScreen.instance.AddNewLine("Time", 0);
    }
}
