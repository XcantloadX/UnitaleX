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

#if UNITY_EDITOR
        if (string.IsNullOrEmpty(SceneSystem.PreviousSceneName))
            GlobalSettings.Init();
#endif

        Init();
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}



    private void Init()
    {
        float y = 0, x = 45f;

        MakeTitle(x, ref y, "游戏");
        MakeEntryBool(x, ref y, "失败后自动重新开始", GlobalSettings.settings.autoRestartAfterGameOver, delegate(UButton self) { GlobalSettings.settings.autoRestartAfterGameOver = (bool)self.GetValue(); });
        MakePadding(ref y);

        MakeTitle(x, ref y, "控制");
        MakeEntryBool(x, ref y, "显示虚拟键盘", GlobalSettings.settings.useDPad, delegate(UButton self) { GlobalSettings.settings.useDPad = (bool)self.GetValue(); });
        //MakeEntryBool(x, ref y, "使用 C 键 (菜单键)", GlobalSettings.settings.useCButton, delegate(UButton self) { GlobalSettings.settings.useCButton = (bool)self.GetValue(); });
        MakeEntryBool(x, ref y, "使用震动", GlobalSettings.settings.useVibrator, delegate(UButton self) { GlobalSettings.settings.useVibrator = (bool)self.GetValue(); });
        MakePadding(ref y);

        MakeTitle(x, ref y, "调试");
        MakeEntryBool(x, ref y, "启用调试", GlobalSettings.settings.debug, delegate(UButton self) { GlobalSettings.settings.debug = (bool)self.GetValue(); });
        MakeEntryBool(x, ref y, "锁血", GlobalSettings.settings.noHit, delegate(UButton self) {
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
