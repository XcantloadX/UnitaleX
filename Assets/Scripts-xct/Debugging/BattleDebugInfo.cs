using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 显示 Battle Scene 的 Debug 信息
/// </summary>
public class BattleDebugInfo : MonoBehaviour {
    public GameObject bulletPool;
    public EnemyController[] enemies;
    public GameObject belowArenaLayer;
    private bool enabled;

    private const string BULLET_NUM = "BulletNum";
    private const string LUASPR_NUM = "LuaSprNum";
    private const string ENEMY_HP = "EnemyHP";

	// Use this for initialization
	void Start () {
        /*if (DebugInfoScreen.instance == null)
        {
            enabled = false;
            return;
        }*/

        enemies = GameObject.FindObjectsOfType<EnemyController>();
        belowArenaLayer = GameObject.Find("BelowArenaLayer");
        bulletPool = GameObject.Find("BulletPool");

        DebugInfoScreen.instance.NewKVLine(BULLET_NUM, 0);
        DebugInfoScreen.instance.NewKVLine(LUASPR_NUM, 0);
        

        for (int i = 0; i < enemies.Length; i++)
        {
            DebugInfoScreen.instance.NewKVLine(ENEMY_HP + i.ToString(), 0);
        }

        SceneManager.activeSceneChanged += delegate(Scene oldScene, Scene newScene) {
            if(newScene.name != "Battle"){
                Hide();
            }
        };
	}
	
    private void Hide()
    {
        DebugInfoScreen.instance.RemoveKVLine(BULLET_NUM);
        DebugInfoScreen.instance.RemoveKVLine(LUASPR_NUM);
        for (int i = 0; i < enemies.Length; i++)
        {
            DebugInfoScreen.instance.RemoveKVLine(ENEMY_HP + i.ToString());
        }
    }

	// Update is called once per frame
	void Update () {
        if(enabled)
        {
            DebugInfoScreen.instance.EditKVLine(BULLET_NUM, GetActiveChildCount(bulletPool.transform));
            DebugInfoScreen.instance.EditKVLine(LUASPR_NUM, belowArenaLayer.transform.childCount);

            for (int i = 0; i < enemies.Length; i++)
            {
                DebugInfoScreen.instance.EditKVLine(ENEMY_HP + i.ToString(), enemies[i].HP);
            }
        }

	}

    private int GetActiveChildCount(Transform t)
    {
        int num = t.childCount;
        int activeNum = 0;
        for (int i = 0; i < num; i++)
        {
            if (t.GetChild(i).gameObject.activeInHierarchy)
                activeNum++;
        }
        return activeNum;
    }
}
