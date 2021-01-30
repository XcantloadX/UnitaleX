using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EventHelper : MonoBehaviour {

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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
}
