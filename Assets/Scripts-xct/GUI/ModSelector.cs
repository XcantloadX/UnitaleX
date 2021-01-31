using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

/// <summary>
/// Mod 选择界面 UI
/// </summary>
public class ModSelector : MonoBehaviour
{

    public GameObject modBtn;
    public GameObject content;
    public GameObject loadingText;
    private float y = 0;
    private List<GameObject> buttons = new List<GameObject>(10);
    public string currentModName = null;

    public const int MODE_ROOT = 0; //扫描安装的 mod
    public const int MODE_ENCOUNTER = 1; //扫描 mod 里所有的 encounter
    public const int MODE_LUA = 2; //进入 encounter
    
    void Start()
    {
        ScanAllMods();
    }

    private void ScanAllMods()
    {
        DirectoryInfo info = new DirectoryInfo(Path.Combine(FileLoader.DataRoot, "Mods"));
        DirectoryInfo[] subInfos = info.GetDirectories();

        
        foreach (DirectoryInfo subInfo in subInfos)
        {
            Debug.Log(subInfo.FullName);
            NewButton(subInfo.Name, Path.Combine(subInfo.FullName, "Lua/Encounters"), MODE_ENCOUNTER);
        }

    }

    private void ScanAllEncounters(string path)
    {
        DirectoryInfo info = new DirectoryInfo(path);
        FileInfo[] files = info.GetFiles();
        NewButton("返回", null, MODE_ROOT);
        foreach (FileInfo f in files)
        {
            if (f.Extension == ".lua")
            {
                NewButton(Path.GetFileNameWithoutExtension(f.Name), f.FullName, MODE_LUA);
                
            }
        }

        //StartCoroutine(SetBtnWidth());
    }

    public void Enter(string buttonText, string path, int mode)
    {
        //清除之前的数据
        foreach (GameObject obj in buttons)
        {
            GameObject.Destroy(obj);
        }
        y = 0;

        if (mode == MODE_ROOT) //扫描安装的 mod
            ScanAllMods();
        else if (mode == MODE_ENCOUNTER) //扫描 mod 里所有的 encounter
        {
            ScanAllEncounters(path);
            //StaticInits.MODFOLDER = new DirectoryInfo(path).Parent.Parent.Name; //Mod 所在文件夹的名字
            StaticInits.MODFOLDER = buttonText;
        }
        else if (mode == MODE_LUA) //进入 encounter
        {
            StaticInits.ENCOUNTER = Path.GetFileNameWithoutExtension(new DirectoryInfo(path).Name);
            WaitingScreen.LoadWaitingScreen(delegate() {
                SceneSystem.Load(SceneSystem.BATTLE);
            }, "加载中...");
            
        }
        else
            throw new Exception("Unknown mode: " + mode);
    }

    /// <summary>
    /// 实例化新按钮
    /// </summary>
    /// <param name="text">显示的文本</param>
    /// <param name="path">有关路径</param>
    /// <param name="mode">该按钮的模式</param>
    private void NewButton(string text, string path, int mode)
    {
        GameObject obj = GameObject.Instantiate(modBtn);
        buttons.Add(obj);
        obj.transform.SetParent(content.transform);
        obj.GetComponentInChildren<Text>().text = text;
        //设置坐标（注意 Pivot、Anchors 等问题）
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0, -y);
        //TODO 自适应文本框
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 700);
        y += rect.rect.height + 10f;
        //很奇怪，scale 会自己变
        rect.localScale = Vector2.one;
        //设置 encounterPath
        ModSelectButton selector = obj.AddComponent<ModSelectButton>();
        selector.encounterPath = path;
        selector.selector = this;
        selector.mode = mode;
        selector.text = text;
    }

    IEnumerator SetBtnWidth()
    {
        yield return new WaitForEndOfFrame();

        foreach (GameObject obj in buttons)
        {
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, obj.GetComponentInChildren<Text>().preferredWidth);
            Debug.Log(obj.GetComponentInChildren<Text>().preferredWidth);
        }
    }
}
