using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModSelectButton : MonoBehaviour {

    public string encounterPath;
    public ModSelector selector;
    public int mode = -1;
    public string text;

	void Start () 
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
	}
	

    void OnClick()
    {
        selector.Enter(text, encounterPath, mode);
    }
}
