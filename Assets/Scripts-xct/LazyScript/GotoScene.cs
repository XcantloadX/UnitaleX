using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GotoScene : LazyScriptBase
{
    public void Goto(string name)
    {
        SceneSystem.Load(name);
    }

    public void GotoMenu()
    {
        Goto(SceneSystem.START_MENU);
    }

    public void GotoModSelect()
    {
        Goto(SceneSystem.MOD_SELECT);
    }

    public void ReloadCurrent()
    {
        SceneSystem.Load(SceneSystem.CurrentSceneName);
    }
}
