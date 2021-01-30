using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FileButton : MonoBehaviour {

    [Header("Icon")]
    public Sprite file;
    public Sprite folder;
    public Sprite lua;

    [Header("GameObject")]
    public Image iconImage;
    public Button button;
    public Text text;

    [Header("Debug")]
    [SerializeField] private string diaplayName = "";
    [SerializeField] private string path = "";

    private FileExplorer explorer;
    public bool IsFile
    {
        get
        {
            return File.Exists(path);
        }
    }
    public bool IsLua
    {
        get
        {
            return Path.GetExtension(path) == ".lua";
        }
    }
    public bool IsModFolder
    {
        get
        {
            if (IsFile)
                return false;
            if (!Directory.Exists(path))
                return false;
            string s = Path.Combine(path, "Lua\\Encounters");
            if (Directory.Exists(s) && FileExplorer.HasAnyLuaFile(s))
                return true;
            return false;
        }
    }

	void Start ()
    {
        if (button == null)
            button = gameObject.GetComponent<Button>();
        if(text == null)
            text = gameObject.GetComponentInChildren<Text>();
        //iconImage = gameObject.GetComponentInChildren<Image>();

        //添加点击事件
        Button.ButtonClickedEvent buttonEvent = new Button.ButtonClickedEvent();
        buttonEvent.AddListener(OnCliked);
        button.onClick = buttonEvent;

        //Debug.Log(path + "|" + isModFolder);
	}

    public void init(string path, FileExplorer explorer)
    {
        //得到应该显示的名字
        string name = Path.GetFileName(path);
        init(path, name, explorer);
    }

    public void init(string path,string name, FileExplorer explorer)
    {
        this.explorer = explorer;
        this.path = path;

        //得到应该显示的名字
        diaplayName = name;
        text.text = diaplayName;

        //设置图标
        if (IsLua)
            iconImage.sprite = lua;
        else if (IsFile)
            iconImage.sprite = file;
        else
            iconImage.sprite = folder;
    }

    private void OnCliked()
    {
        if (IsModFolder && diaplayName != "..")//如果是mod所在文件夹
        {
            string s = Path.Combine(path, "Lua\\Encounters");
            explorer.SetGoBackPath(path);
            explorer.GoToPath(s);
        }
        else if (!IsFile)//如果不是文件(即普通文件夹)
            explorer.GoToPath(path);
        else if(IsLua)//如果是mod文件
        {
            //暂停背景音乐
            AudioSystem.StopMusic();

            //加载mod文件
            //MODFOLDER = mod所在文件夹路径(相对于mod文件夹)
            //ENCOUNTER = mod文件名(不带后缀名)
            StaticInits.MODFOLDER = FileExplorer.GetPartOfPath(FileLoader.getRelativePathWithoutExtension(FileLoader.ModDataPath, path), 1);
            Debug.Log(StaticInits.MODFOLDER);
            StaticInits.ENCOUNTER = Path.GetFileNameWithoutExtension(path);
            Debug.Log(StaticInits.ENCOUNTER);
            SceneSystem.LoadBattle();
        }

    }
}
