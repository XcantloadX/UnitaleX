using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 放一些全局都需要的代码
/// </summary>
public class GlobalObject : MonoBehaviour
{

    [SerializeField]
    private float startTime;
    [SerializeField]
    private int clicked = 0;
    private static bool inited = false; //保证只有一个实例

    // Use this for initialization
    void Start ()
    {
        //只允许同时存在一个实例
        if (inited)
            Destroy(gameObject);
        inited = true;
        GameObject.DontDestroyOnLoad(this);

        //屏幕常亮
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
	
	// Update is called once per frame
	void Update ()
    {
#if UNITY_ANDROID || UNITY_IOS
        DoubleClickExit();
#endif
	}

    //双击返回键退出
    private void DoubleClickExit()
    {
        if (Time.time - startTime > 2)//如果点击间隔大于2s
        {
            startTime = Time.time;
            clicked = 0;
        }

        //ESC 键 == 返回键
        if (Input.GetKeyDown(KeyCode.Escape) && (SceneSystem.CurrentSceneName == SceneSystem.START_MENU || SceneSystem.CurrentSceneName == SceneSystem.DATA_DOWNLOADING))
        {
            AndroidUtil.Toast("再按一次退出");
            if (Time.time - startTime <= 2 && clicked >= 1) //间隔小于2s且点击了两次
                Application.Quit();
            else if (Time.time - startTime <= 2) //间隔小于2s
                clicked++;
            
        }
    }
}
