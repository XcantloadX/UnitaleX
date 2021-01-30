using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformaceTestObjMover : MonoBehaviour {

    public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Random.insideUnitCircle * speed);
	}
}
