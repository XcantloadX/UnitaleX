using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LaterStart : MonoBehaviour {
    private bool called = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        called = true;
        LateStart();
	}

    protected abstract void LateStart();
}
