using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class USwitch : MonoBehaviour {


    [SerializeField] private string[] texts;
    [SerializeField] private int current;

    [Header("Localization")]
    public string key;
    public bool localizated;
    /// <summary>
    /// 当前选项的文本
    /// </summary>
    public string Text
    {
        get
        {
            return texts[current];
        }
    }
    public UButton Button
    {
        get
        {
            return button;
        }
    }

    public string ID
    {
        get
        {
            return "swi";
        }
    }

    public string Key
    {
        get
        {
            return key;
        }
    }

    public string DisplayText
    {
        get
        {
            return GetCurrentText();
        }

        set
        {
            throw new NotImplementedException();
        }
    }
    public bool Localized
    {
        get{ return localizated; }

        set { localizated = value; }
    }

    private UButton button;
    private string lastString = "";

	// Use this for initialization
	void Start ()
    {
        button = gameObject.GetComponent<UButton>();
        button.PointerClicked += Clicked;

        this.UpdateText();
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateText();
    }

    private void Clicked()
    {
        current++;
        if (current >= texts.Length)
            current = 0;

        UpdateText();
    }

    private string GetCurrentText()
    {
        if (current == 0)
            return texts[current] + " ->";
        else if (current == texts.Length - 1)
            return "<- " + texts[current];
        return "<- " + texts[current] + " ->";
    }

    private void UpdateText()
    {
        if(button != null && (texts[current] != lastString))
        {
            button.SetText(GetCurrentText());
            lastString = texts[current];
        }
            
    }

    public void SetValueByText(string s)
    {
        int index = Array.IndexOf(texts, s);
        if (index < 0)
            return;
        SetValue(index);
    }

    public void SetValue(int index)
    {
        if(index >= 0 && index < texts.Length)
            current = index;
    }
}
