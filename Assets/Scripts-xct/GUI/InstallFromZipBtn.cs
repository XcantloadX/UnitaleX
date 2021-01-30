using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class InstallFromZipBtn : MonoBehaviour {

    public InputField input;
#if UNITY_EDITOR
    private const string FILE_PATH_CHAR = "\\";
#elif UNITY_WINDOWS
    private const string FILE_PATH_CHAR = "\\";
#elif UNITY_ANDROID
    private const string FILE_PATH_CHAR = "/";
#endif

    public void OnClick()
    {
        string zipPath = input.text;
        string modName = Path.GetFileNameWithoutExtension(zipPath);
        DirectoryInfo dir = Directory.CreateDirectory(FileLoader.DataRoot + FILE_PATH_CHAR + "Mods" + FILE_PATH_CHAR + modName); //解压后的文件夹(Mod/MODNAME)

        Debug.Log("Install mod from: " + zipPath);
        Debug.Log("Install mod to: " + dir.FullName);

        //解压
        Util.Unzip(dir.FullName, zipPath);

        Debug.Log("CHAR=" + FILE_PATH_CHAR);
        //如果解压后还有一层目录
        if (!Directory.Exists(dir.FullName + "\\Lua"))
        {
            DirectoryInfo[] dirs = dir.GetDirectories();
            if (dirs.Length <= 0 || !Directory.Exists(dirs[0].FullName + FILE_PATH_CHAR + "Lua"))
                throw new Exception("Mod 格式不正确！未找到 Lua 文件夹");

            Debug.Log("Moving " + dirs[0].FullName + " to " + dir.Parent.FullName + FILE_PATH_CHAR + modName + "_");
            /* dir = "Mod\TEST_MOD"
             * dirs[0] = "Mod\TEST_MOD\TRUEMOD"
             * dir.Parent + "\\" + modName = "Mod\TRUEMOD"
             */
            Directory.Move(dirs[0].FullName, dir.Parent.FullName + FILE_PATH_CHAR + modName + "_"); //移出来
            Directory.Delete(dir.FullName); //删除原来的文件夹
            Debug.Log("Installed successly.");

            //刷新场景
            SceneSystem.Load(SceneSystem.MOD_SELECT);
        }
    }
}
