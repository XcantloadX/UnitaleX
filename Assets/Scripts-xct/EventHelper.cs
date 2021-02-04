using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventHelper : MonoBehaviour {

    public void GoToScene(string sceneName)
    {
        SceneSystem.Load(sceneName);
    }

    public void LoadSceneInAddition(string name)
    {
        SceneSystem.LoadInAddition(name);
    }

    public void UnloadCurrentScene()
    {
        SceneSystem.UnloadCurrent();
    }

    public void UnloadCurrentIfInBattle(string fallback)
    {
        if (SceneSystem.IsInGame)
            UnloadCurrentScene();
        else
            GoBack(fallback);
    }

    public void Destroy(GameObject obj)
    {
        GameObject.Destroy(obj);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StopMusic()
    {
        AudioSystem.StopMusic();
    }

    public void GoBack(string fallback)
    {
        SceneSystem.GoBack(fallback);
    }

    public void SaveSettings()
    {
        GlobalSettings.ins.Save();
    }

}
