using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnlyOneEventSystem : LazyScriptBase {

	// Use this for initialization
	void Start () 
    {
        //一个给 GameCanvas，一个给 PadCanvas
        if (GameObject.FindObjectsOfType<EventSystem>().Length > 2)
            Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () 
    {
        /*if (!SceneSystem.IsInGame)
            Destroy(gameObject);*/
	}
}
