using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 用于在战斗时暂停游戏
/// 由于重写过于麻烦，这里采用了取巧的办法
/// </summary>
public class GamePause : MonoBehaviour {

    private bool paused = false;
    private float timeScaleBak = 0;
    private bool[] audioPlayingState;
    private AudioSource[] audioSource;
    private GameObject gameCanvas;
    private RectTransform uiCanvas;
    private DPadController dPad;
    private bool dPadStateBak = false;

    public GameObject pauseScreen; //暂停提示屏幕
    public RawImage background; //游戏截图背景
    public Text modName; //mod 名称显示

	void Start () {
        audioSource = GameObject.FindObjectsOfType<AudioSource>();
        audioPlayingState = new bool[audioSource.Length];
        dPad = GameObject.FindObjectOfType<DPadController>();
        gameCanvas = GameObject.Find("GameCanvas");
        uiCanvas = GameObject.Find("UICanvas").GetComponent<RectTransform>();
	}
	

	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            StartCoroutine(SetPause(!paused));
	}

    private IEnumerator SetPause(bool paused)
    {
        this.paused = paused;

        if(paused)
        {
            //暂停音乐
            for (int i = 0; i < audioSource.Length; i++ )
            {
                if (audioSource[i].enabled)
                {
                    audioPlayingState[i] = audioSource[i].isPlaying;
                    audioSource[i].Pause();
                }
            }

            yield return new WaitForEndOfFrame();

            //截图
            Texture2D t = new Texture2D(Screen.width, Screen.height);
            t.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            t.Apply();

            //-----开启暂停屏幕-----
            modName.text = StaticInits.MODFOLDER;

            //TODO 完善背景图功能
            pauseScreen.gameObject.SetActive(true);
            /*background.gameObject.SetActive(true); 
            background.texture = t;*/

            //调整背景图位置
            background.rectTransform.position = WorldToUIPosition(uiCanvas, Camera.main, gameCanvas.transform.position);
            background.rectTransform.sizeDelta = gameCanvas.GetComponent<RectTransform>().sizeDelta;

            //暂停所有用到 timeScale 的动画等等
            timeScaleBak = Time.timeScale;
            Time.timeScale = 0;

            //禁用所有有关 Object（不要问我为什么）
            gameCanvas.SetActive(false);

            dPadStateBak = dPad.displaying;
            dPad.SetDisplayRaw(false); //隐藏虚拟键盘
        }
        else
        {
            //关闭暂停屏幕
            pauseScreen.gameObject.SetActive(false);
            background.gameObject.SetActive(false);

            

            Time.timeScale = timeScaleBak;

            gameCanvas.SetActive(true);
            dPad.SetDisplayRaw(dPadStateBak);

            for (int i = 0; i < audioSource.Length; i++)
            {
                if (audioSource[i].enabled && audioPlayingState[i])
                    audioSource[i].Play();
            }
        }
    }

    /// <summary>
    /// 暂停/取消暂停
    /// </summary>
    public void SwicthPause()
    {
        StartCoroutine(SetPause(!paused));
    }

    public void ExitBattle()
    {
        //退出之前记得把 timeScale 改回去！
        Time.timeScale = timeScaleBak;
        SceneSystem.ExitFromBattleTo(SceneSystem.MOD_SELECT);
    }

    public void RestartBattle()
    {
        StartCoroutine(SetPause(false));
        /* 直接重载场景会导致 UICanvas 重复
         * 如果给 UICanvas 加上单例的话，就会销毁新的 UICancas，保留旧的，但是这样 Inspector 里的绑定会掉
         * 所以，直接销毁掉旧的
         */
        Destroy(uiCanvas.gameObject);
        Destroy(GameObject.Find("EventSystem"));
        SceneSystem.Load(SceneSystem.CurrentSceneName);
        
    }

    /*void OnGUI()
    {
        if (GUILayout.Button("Pause/Unpause"))
            StartCoroutine(SetPause(!paused));
    }*/

    /// <summary>
    /// 世界坐标转UI坐标
    /// </summary>
    /// <param name="canvasRectTransform">Canvas的RectTransform</param>
    /// <param name="uiCamera">UI相机</param>
    /// <param name="worldPosition">对象世界坐标</param>
    /// <returns></returns>
    Vector2 WorldToUIPosition(RectTransform canvasRectTransform, Camera uiCamera, Vector3 worldPosition)
    {
        //世界坐标 -> ViewPort视图坐标   
        Vector2 viewPos = uiCamera.WorldToViewportPoint(worldPosition);
        //ViewPort视图坐标 -〉UI坐标   
        return new Vector2(canvasRectTransform.rect.width * viewPos.x, canvasRectTransform.rect.height * viewPos.y);
    }
}
