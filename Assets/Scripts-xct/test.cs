using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerformaceTestObjMaker : MonoBehaviour {

    public int num;
    public GameObject[] objects;
    public GameObject target;
    public float speed;
    public bool objectMovedByItselfsScript;

    private bool isCanvasObject;
    private GameObject canvas;

	void Start () {
        objects = new GameObject[num];

        if(target.GetComponent<RectTransform>() != null)
        {
            isCanvasObject = true;
            canvas = GameObject.Find("Canvas");
        }
        for (int i = 0; i < num; i++)
        {
            objects[i] = GameObject.Instantiate(target);
            if (isCanvasObject)
            {//如果是 Canvas Object
                objects[i].transform.parent = canvas.transform;
                objects[i].transform.position = target.transform.position;
            }
            if (objectMovedByItselfsScript)
                objects[i].AddComponent<PerformaceTestObjMover>().speed = speed;
        }
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (objectMovedByItselfsScript)
            return;

		foreach(GameObject obj in objects)
        {
            if (isCanvasObject)
                obj.GetComponent<RectTransform>().Translate(Random.insideUnitCircle * speed * 100f);
            else
                obj.transform.Translate(Random.insideUnitCircle * speed);
        }
	}


}
