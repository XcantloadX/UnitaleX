using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 锁血
/// </summary>
public class DebugCheating : MonoBehaviour {

    private PlayerController player;
    private EnemyController enemy;
    public bool Cheating;
    public bool KillMonsterAtOnce;

	void Start () {
        player = GameObject.FindObjectOfType<PlayerController>();
        if(Cheating)
        {
            DebugInfoScreen.instance.AddNewLine("NoHit", true);
            GlobalStaic.noHit = true;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
	}
	
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //场景改变时重新寻找 Player
        if(scene.name == "Battle")
        {
            player = GameObject.FindObjectOfType<PlayerController>();
            enemy = GameObject.FindObjectOfType<EnemyController>();
        }
        else
        {
            player = null;
            enemy = null;
        }
        
    }

	void Update () {
        //if (Cheating && player != null)
        //    player.setHP(PlayerCharacter.MaxHP);
        //if (KillMonsterAtOnce && enemy != null && enemy.HP > 1)
        //    enemy.HP = 1;
	}

    private void Inject()
    {

    }
}
