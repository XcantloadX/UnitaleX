using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FileExplorer : MonoBehaviour {

    [Header("GameObject")]
    public GameObject content;
    public InputField pathText;
    public GameObject buttonPrefab;
    [Header("Position")]
    public float buttonX = 0f;
    public float buttonY;
    public float buttonHeight;
    public float spacing;
    [Header("Path")]
    [SerializeField] private string _path;
    [SerializeField] private string goBackPath;

    private int buttonCount;
    private ArrayList buttons = new ArrayList();
    private static string lastPath;


    void Start ()
    {
        buttonHeight = buttonPrefab.GetComponent<RectTransform>().rect.height;
        //pathText = GameObject.Find("Path").GetComponentInChildren<Text>();
        _path = string.IsNullOrEmpty(lastPath) ? Path.Combine(FileLoader.DataRoot, "Mods") : lastPath;
        GoToPath(_path);
	}
	
	
	void Update ()
    {
		
	}

    public void GoToPath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return;

        _path = path;
        lastPath = _path;
        pathText.text = _path;
        buttonCount = 0;
        Clear();//销毁当前所有的按钮
        EnumFile(path);
        goBackPath = "";//清除上一级路径
    }

    public void GoToPath()
    {
        GoToPath(pathText.text);
    }

    public void GoBack()
    {
        string s = new DirectoryInfo(Path.Combine(_path, "..")).FullName;
        if (s == _path)
            SceneManager.LoadScene("StartMenu");
        GoToPath(s);
    }

    public void SetGoBackPath(string path)
    {
        this.goBackPath = path;
    }

    /// <summary>
    /// 枚举指定目录下的文件和文件夹
    /// </summary>
    /// <param name="path">指定目录</param>
    private void EnumFile(string path)
    {
        //返回上一级文件夹按钮
        string back = ( string.IsNullOrEmpty(goBackPath) ? new DirectoryInfo(Path.Combine(path, "..")).FullName : FileExplorer.GetParentPath(goBackPath) );
        NewFileButton(back, "..");

        //枚举所有文件(夹)并添加按钮
        DirectoryInfo directory = new DirectoryInfo(path);
        FileSystemInfo[] info = directory.GetFileSystemInfos();
        for (int i = 0; i < info.Length; i++)
        {
            string s = info[i].FullName;
            NewFileButton(s);
        }
    }

    private void NewFileButton(string path, string name = null)
    {
        GameObject obj = Instantiate(buttonPrefab);
        buttons.Add(obj);

        //调整新按钮
        obj.transform.SetParent(content.transform);

        //设置坐标
        //RectTransform.anchoredPosition：RectTransform以锚点为原点的坐标系
        obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(buttonX, -(buttonHeight * buttonCount) );

        //初始化新按钮
        FileButton button = obj.GetComponent<FileButton>();
        if (string.IsNullOrEmpty(name))
            button.init(path, this);
        else
            button.init(path, name, this);

        buttonCount++;

        //调整Content的大小，要不然会无法继续滑动下去
        Vector2 size = content.GetComponent<RectTransform>().sizeDelta;
        float width = size.x;
        float height = size.y;
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(width, buttonHeight * buttonCount);
    }

    public void Clear()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            GameObject.Destroy((GameObject)buttons[i]);
        }
        buttons.Clear();
    }

    public static bool HasAnyLuaFile(string path)
    {
        DirectoryInfo directory = new DirectoryInfo(path);
        FileSystemInfo[] info = directory.GetFileSystemInfos();
        for (int i = 0; i < info.Length; i++)
        {
            string s = info[i].FullName;
            if(Path.GetExtension(s) == ".lua")
            {
                return true;
            }
        }
        return false;
    }

    public static string GetRelativePathWithoutExtension(string rootPath, string fullPath)
    {
        int index = fullPath.IndexOf(rootPath);
        string s = fullPath.Remove(index, rootPath.Length);
        return s;
    }

    public static string GetPartOfPath(string path, int count)
    {
        if (count <= 0)
            return string.Empty;

        string[] keywords = path.IndexOf('\\') != -1 ? path.Split('\\') : path.Split('/');
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < count; i++)
        {
            sb.Append(keywords[i] + "\\");
        }

        return sb.ToString();
    }

    /// <summary>
    /// 获取指定路径的上一级路径（如："C:\Windows\System"返回"C:\Windows\"）
    /// </summary>
    /// <param name="path">指定的路径</param>
    /// <returns>上一级路径</returns>
    public static string GetParentPath(string path)
    {
        return new DirectoryInfo(Path.Combine(path, "..")).FullName;
    }
}
