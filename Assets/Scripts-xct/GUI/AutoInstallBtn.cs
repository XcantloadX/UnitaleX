using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Keiwando.NFSO;
using System.IO;
using System;

public class AutoInstallBtn : MonoBehaviour {

    //文件路径分隔符
#if UNITY_EDITOR || UNITY_WINDOWS
    private const string FILE_PATH_CHAR = "\\";
#elif UNITY_ANDROID
    private const string FILE_PATH_CHAR = "/";
#endif

    private string MOD_PATH { get { return FileLoader.DataRoot + FILE_PATH_CHAR + "Mods"; } }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        NativeFileSO.shared.OpenFile(SupportedFilePreferences.supportedFileTypes, Callback);
    }

    void Callback(bool wasFileOpened, OpenedFile file)
    {
        if (wasFileOpened)
        {
            Debug.Log(file.Name);
            InstallMod(Path.GetFileNameWithoutExtension(file.Name), file.Extension, file.Data);
            SceneSystem.Load(SceneSystem.MOD_SELECT); //Android 上有 bug，需要再刷新一次
        }
    }

    /// <summary>
    /// 从 Zip/Rar 压缩文件安装 Mod
    /// </summary>
    /// <param name="name">文件名，不带扩展名</param>
    /// <param name="ext">扩展名，带点</param>
    /// <param name="data">文件数据</param>
    void InstallMod(string name, string ext, byte[] data)
    {
        string unzippedPath = MOD_PATH + FILE_PATH_CHAR + "temp_mod"; //解压后的位置
        string installedModPath = MOD_PATH + FILE_PATH_CHAR + name; //安装后的位置
        string tempFileName = MOD_PATH + FILE_PATH_CHAR + "temp" + ext; //临时文件名
        bool unzipped = false;

        if (ext == ".zip")
        {
            Util.Unzip(unzippedPath, data);
            unzipped = true;
        }
        else if (ext == ".rar")
        {
#if UNITY_ANDROID
            throw new Exception("Rar 尚不支持!");
            File.WriteAllBytes(tempFileName, data); //写到临时文件
            UnRarPlugin.UnRar(tempFileName, unzippedPath); //解压
            unzipped = true;
#elif UNITY_WINDOWS
            throw new Exception("Rar is not supported on Windows!");
#endif
        }
        else
            throw new Exception("尚不支持的压缩文件格式：" + ext);

        if(unzipped)
        {
            string realPath = ScanModPath(unzippedPath);
            if (realPath == null) //如果未找到 mod
                throw new Exception("不正确的 Mod 格式！");
            //把 Mod 移出去
            Directory.Move(realPath, installedModPath);
            Directory.Delete(unzippedPath);
            if(File.Exists(tempFileName))
                File.Delete(tempFileName);
            //刷新显示
            SceneSystem.Load(SceneSystem.MOD_SELECT);
            Debug.Log("New mod installed successfully to " + installedModPath);
        }
    }

    /// <summary>
    /// 检查一个文件夹是否是 Mod 所在文件夹
    /// </summary>
    /// <param name="dirPath"></param>
    /// <returns></returns>
    private bool IsModPath(string dirPath)
    {
        return Directory.Exists(dirPath + FILE_PATH_CHAR + "Lua");
    }

    /// <summary>
    /// 扫描一个文件夹下的 Mod 路径
    /// </summary>
    /// <param name="dirPath"></param>
    /// <returns>如果未找到，返回 null</returns>
    private string ScanModPath(string dirPath)
    {
        if (IsModPath(dirPath)) //如果传进来的就是 Mod 文件夹
            return dirPath;
        else
        {
            DirectoryInfo[] dirs = new DirectoryInfo(dirPath).GetDirectories();
            for (int i = 0; i < dirs.Length; i++)
            {
                if(IsModPath(dirs[i].FullName))
                    return dirs[i].FullName;
            }

            return null;
        }
       
    }
}
