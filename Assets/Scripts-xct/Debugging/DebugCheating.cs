using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 锁血
/// </summary>
public class DebugCheating : MonoBehaviour
{
    public bool Cheating;

    void Start()
    {
        DebugInfoScreen.instance.AddNewLine("Hits", 0);
        if (Cheating)
        {
            DebugInfoScreen.instance.AddNewLine("NoHit", true);
            GlobalStaic.noHit = true;
        }
    }

    void Update()
    {
        DebugInfoScreen.instance.EditLine("Hits", GlobalStaic.hits + " (= -" + GlobalStaic.totalHurtHP + " HP)");
    }

}
