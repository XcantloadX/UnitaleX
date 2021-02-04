using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStaic : MonoBehaviour {

    public static bool noHit = false;
    public static int hits = 0;
    public static int totalHurtHP = 0;

#if UNITY_ANDROID
    public static bool useDPad = true;
#else
    public static bool useDPad = false;
#endif

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void RestInBattleStats()
    {
        hits = 0;
    }
}
