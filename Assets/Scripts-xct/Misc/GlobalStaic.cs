using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStaic : MonoBehaviour {

    public static bool noHit = false;
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
}
