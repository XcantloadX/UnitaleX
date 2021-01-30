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

        DebugInfoScreen.instance.AddNewLine(BULLET_NUM, 0);
        DebugInfoScreen.instance.AddNewLine(LUASPR_NUM, 0);
        

        for (int i = 0; i < enemies.Length; i++)
        {
            DebugInfoScreen.instance.AddNewLine(ENEMY_HP + i.ToString(), 0);
        }

        SceneManager.activeSceneChanged += delegate(Scene oldScene, Scene newScene) {
            if(newScene.name != "Battle"){
                Hide();
            }
        };
	}
	
    private void Hide()
    {
        DebugInfoScreen.instance.RemoveLine(BULLET_NUM);
        DebugInfoScreen.instance.RemoveLine(LUASPR_NUM);
        for (int i = 0; i < enemies.Length; i++)
        {
            DebugInfoScreen.instance.RemoveLine(ENEMY_HP + i.ToString());
        }
    }

	// Update is called once per frame
	void Update () {
        if(enabled)
        {
            DebugInfoScreen.instance.EditLine(BULLET_NUM, GetActiveChildCount(bulletPool.transform));
            DebugInfoScreen.instance.EditLine(LUASPR_NUM, belowArenaLayer.transform.childCount);

            for (int i = 0; i < enemies.Length; i++)
            {
                DebugInfoScreen.instance.EditLine(ENEMY_HP + i.ToString(), enemies[i].HP);
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
