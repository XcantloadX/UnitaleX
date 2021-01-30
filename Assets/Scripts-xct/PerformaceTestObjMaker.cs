using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    public int num;
    public GameObject[] objects;
    public GameObject target;

	// Use this for initialization
	void Start () {
        objects = new GameObject[num];
        for (int i = 0; i < num; i++)
        {
            objects[i] = GameObject.Instantiate(target);
        }
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		foreach(GameObject obj in objects)
        {
            
        }
	}


}
