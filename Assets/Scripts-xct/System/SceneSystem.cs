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

    private static bool init = false;
    private static string previous = "";
    private static List<string> loadedScenes = new List<string>(5);
    public static string PreviousSceneName { get { return previous; } }

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
    public static bool IsInGame { get { return loadedScenes.Contains(BATTLE) || loadedScenes.Contains(GAME_OVER); } }

    public static void Init()
    {
        if (init)
            return;

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.activeSceneChanged += OnSceneChanged;
        loadedScenes.Add(CurrentSceneName);
        init = true;

    }

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
        previous = previousScene.name;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        loadedScenes.Add(scene.name);
    }

    private static void OnSceneUnloaded(Scene scene)
    {
        loadedScenes.Remove(scene.name);
    }

    /// <summary>
    /// 加载指定名字的 Scene
    /// </summary>
    public static void Load(string name)
    {
        SceneManager.LoadScene(name);
    }

    /// <summary>
    /// 加载指定名字的 Scene，并保持原 Scene 不被销毁
    /// </summary>
    /// <param name="name">Scene 名称</param>
    public static void LoadInAddition(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Additive);
    }

    /// <summary>
    /// 卸载当前 Scene
    /// </summary>
    public static void UnloadCurrent()
    {
        SceneManager.UnloadSceneAsync(CurrentSceneName);
    }

    /// <summary>
    /// 返回到上一场景
    /// <param name="fallback">如果上一场景为空，则应该跳转的场景。设置为 null 表示不跳转。</param>
    /// </summary>
    public static void GoBack(string fallback)
    {
        if (!string.IsNullOrEmpty(PreviousSceneName))
            Load(PreviousSceneName);
        else if (!string.IsNullOrEmpty(fallback))
            Load(fallback);
    }

}
