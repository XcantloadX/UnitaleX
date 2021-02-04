using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour {

    private GameObject uButton;
    private GameObject uLabel;
    private float height;

    public RectTransform content;

	// Use this for initialization
	void Start () 
    {
        uButton = (GameObject)Resources.Load("Prefabs/UI/SettingsUButton");
        uLabel = (GameObject)Resources.Load("Prefabs/UI/SettingsULabel");

        Init();
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}



    private void Init()
    {
        float y = 0, x = 45f;

        MakeTitle(x, ref y, "DPad");
        MakeEntryBool(x, ref y, "Show", GlobalSettings.settings.useDPad, delegate(UButton self) { GlobalSettings.settings.useDPad = (bool)self.GetValue(); });
        MakeEntryBool(x, ref y, "Use C Button (MENU Button)", GlobalSettings.settings.useCButton, delegate(UButton self) { GlobalSettings.settings.useCButton = (bool)self.GetValue(); });
        MakeEntryBool(x, ref y, "Use Vibrator", GlobalSettings.settings.useVibrator, delegate(UButton self) { GlobalSettings.settings.useVibrator = (bool)self.GetValue(); });
        MakePadding(ref y);

        MakeTitle(x, ref y, "Debug");
        MakeEntryBool(x, ref y, "Enable Debug", GlobalSettings.settings.debug, delegate(UButton self) { GlobalSettings.settings.debug = (bool)self.GetValue(); });
        MakeEntryBool(x, ref y, "No Hit", GlobalSettings.settings.noHit, delegate(UButton self) {
            GlobalSettings.settings.noHit = (bool)self.GetValue();
            DebugInfoScreen.instance.UpdateDebuggers();
        });

        
    }

    public void Exit()
    {

        SceneSystem.Load(SceneSystem.START_MENU);
    }
    
    private void MakePadding(ref float y)
    {
        y -= 10;
    }

    //创建一个标题
    private void MakeTitle(float x, ref float y, string label)
    {
        x -= 25;

        RectTransform lbl = GameObject.Instantiate(uLabel).GetComponent<RectTransform>();
        Text text = lbl.GetComponent<Text>();
        height = lbl.rect.height;

        lbl.SetParent(content);
        lbl.localScale = Vector2.one;
        lbl.anchoredPosition = new Vector2(x, y);
        text.text = label;
        text.color = Color.gray;

        y -= height; //修改坐标
    }

    //创建一个条目
    private UButton MakeEntry(float x, ref float y, string lable, string[] choices, object[] values, int defaultChoice = 0, UButton.Clicked onclick = null)
    {
        RectTransform btn = GameObject.Instantiate(uButton).GetComponent<RectTransform>();
        RectTransform lbl = GameObject.Instantiate(uLabel).GetComponent<RectTransform>();
        UButton ubtn = btn.GetComponent<UButton>();
        height = lbl.rect.height;

        btn.SetParent(content);
        lbl.SetParent(content);
        btn.localScale = Vector2.one;
        lbl.localScale = Vector2.one;
        btn.anchoredPosition = new Vector2(x + lbl.sizeDelta.x, y);
        lbl.anchoredPosition = new Vector2(x, y);
        lbl.GetComponent<Text>().text = lable;

        ubtn.currectChoice = defaultChoice;
        ubtn.choices = choices;
        ubtn.values = values;
        ubtn.EventClicked += onclick;

        y -= height; //修改坐标

        return ubtn;
    }

    private UButton MakeEntryBool(float x, ref float y, string lable, bool value, UButton.Clicked onclick)
    {
        int choice = value ? 0 : 1;
        return MakeEntry(x, ref y, lable, new string[2] { "ON", "OFF" }, new object[2] { true, false }, choice, onclick);
    }
}
