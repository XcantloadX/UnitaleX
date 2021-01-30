using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour {

    private const string FRONT = "Frontground";
    private const string BACK = "Background";
    [SerializeField] private GameObject frontObj;
    [SerializeField] private GameObject backObj;
    [SerializeField] [Range(0, 1f)] private float progress = 0;
    public float Progress
    {
        get { return progress; }
        set { progress = value;}
    }
    private Vector2 frontRect;

	void Start ()
    {
        frontObj = GameObject.Find(FRONT);
        backObj = GameObject.Find(BACK);
        frontRect = frontObj.transform.localScale;
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 scale = new Vector2(frontRect.x * progress, frontRect.y); ;
        frontObj.transform.localScale = scale;
	}

    //public static void Scale
}
