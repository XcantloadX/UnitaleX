using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour
{
    public const string START_MENU = "StartMenu";
    public const string SETTINGS = "Settings";
    public const string FILE_EXPLORER = "FileExplorer";
    public const string BATTLE = "Battle";
    public const string LOADING = "Loading";
    public const string MOD_SELECT = "ModSelect";
    public const string DATA_DOWNLOADING = "Download";
    public const string LUA_ERROR = "Error";
    public const string GAME_OVER = "GameOver";

    /// <summary>
    /// 当前激活 Scene 的名字
    /// </summary>
    public static string CurrentSceneName
    {
        get { return Application.loadedLevelName; }
    }
    /// <summary>
    /// 是否正在进行游戏
    /// </summary>
    public static bool IsInGame { get { return SceneSystem.CurrentSceneName == "Battle" || SceneSystem.CurrentSceneName == "GameOver"; } }

    public static void LoadStartMenu()
    {
        SceneManager.LoadScene(START_MENU);
    }

    public static void LoadSettings()
    {
        SceneManager.LoadScene(SETTINGS);
    }

    public static void LoadFileExplorer()
    {
        SceneManager.LoadScene(FILE_EXPLORER);
    }

    public static void LoadBattle()
    {
        SceneManager.LoadScene(BATTLE);
    }

    private static void OnSceneChanged(Scene previousScene, Scene changedScene)
    {

    }

    /// <summary>
    /// 加载指定名字的 Scene
    /// </summary>
    public static void Load(string name)
    {
        SceneManager.LoadScene(name);
    }

    /// <summary>
    /// 卸载当前 Scene
    /// </summary>
    public static void UnloadCurrent()
    {
        SceneManager.UnloadSceneAsync(CurrentSceneName);
    }

    private void RunCoroutine(IEnumerator enumerator)
    {
        base.StartCoroutine(enumerator);
    }
}
