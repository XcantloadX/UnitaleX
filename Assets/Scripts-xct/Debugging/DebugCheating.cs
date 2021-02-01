using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 锁血
/// </summary>
public class DebugCheating : MonoBehaviour {

    public bool Cheating;
	void Start () {
        if(Cheating)
        {
            DebugInfoScreen.instance.AddNewLine("NoHit", true);
            GlobalStaic.noHit = true;
        }
	}
	
}
