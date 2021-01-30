using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    public float updateTime = 1f;

    private int frames = 0;
    private float timer = 0;
    private float fps = 0;
    private float runTime = 0;
    private static bool inited = false; //保证只有一个实例

    void Start()
    {
        if (inited)
            Destroy(gameObject);
        inited = true;
        GameObject.DontDestroyOnLoad(this);
        DebugInfoScreen.instance.AddNewLine("Fps", 0);
        DebugInfoScreen.instance.AddNewLine("Time", 0);

        
    }

    void Update()
    {
        
        frames++;
        //runTime += Time.;
        timer += Time.deltaTime;

        //计算 fps
        if(timer > updateTime)
        {
            fps = (float)Math.Round(frames / timer, 1);
            DebugInfoScreen.instance.EditLine("Fps", fps);

            frames = 0;
            timer = 0;
        }

        DebugInfoScreen.instance.EditLine("Time", System.Math.Round(Time.time, 1));
    }
}
