using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Undertale 风格按钮，请附加在普通UI按钮上
/// </summary>
public class UButton : MonoBehaviour{

    [Header("Button")]
    public bool disabled;
    public bool useKeyboard = false;
    [SerializeField] private bool actived = false;

    [Header("Text Color")]
    public Color pressedColor = Color.yellow;
    public Color normalColor = Color.white;
    public Color disabledColor = Color.gray;

    [Header("Button Group")]
    public UButton nextYBtn;
    public UButton prevYBtn;
    public UButton nextXBtn;
    public UButton prevXBtn;

    private EventTrigger eventTrigger;
    private Text text;
    private AudioClip move;
    private AudioClip confirm;
    private Button button;

    public List<UButton> xGroup = new List<UButton>(4);
    public List<UButton> yGroup = new List<UButton>(10);

    public string DisplayText
    {
        get
        {
            return text.text;
        }

        set
        {
            SetText(value);
        }
    }
    private string originText;

    public RectTransform rect { get { return gameObject.GetComponent<RectTransform>(); } }

    public delegate void clicked();
    public event clicked PointerClicked;

	void Start ()
    {
        //添加事件触发器
        eventTrigger = gameObject.AddComponent<EventTrigger>();
        AddTrigger(EventTriggerType.PointerDown, OnPointerDown);
        AddTrigger(EventTriggerType.PointerUp, OnPointerUp);
        AddTrigger(EventTriggerType.PointerEnter, OnPointerEnter);
        AddTrigger(EventTriggerType.PointerExit, OnPointerExit);
        AddTrigger(EventTriggerType.PointerClick, OnPoinerClick);

        //寻找组件
        text = gameObject.GetComponentInChildren<Text>();
        button = gameObject.GetComponent<Button>();

        //加载音效
        move = Resources.Load<AudioClip>("menu_move");
        confirm = Resources.Load<AudioClip>("menu_confirm");

        //确保Image组件已经被禁用了
        Image img = gameObject.GetComponent<Image>();
        if(img != null)
            img.enabled = false;

        //检测禁用
        if(this.disabled)
        {
            this.normalColor = disabledColor;
            this.pressedColor = disabledColor;
        }

        //this.UpdateText();
        this.SetColor(normalColor); //更新按钮颜色
        originText = DisplayText;
    }

    void Update()
    {
        //TODO 完善
        if(actived && useKeyboard)
        {
            //判断按键是否按下
            if (Input.GetKeyDown(KeyCode.LeftArrow) && prevXBtn != null)
            {
                SetActive(false);
                prevXBtn.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && nextXBtn != null)
            {
                SetActive(false);
                nextXBtn.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && prevYBtn != null)
            {
                SetActive(false);
                prevYBtn.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && nextYBtn != null)
            {
                SetActive(false);
                nextYBtn.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Return) && actived)
                Click();
        }

    }

    /// <summary>
    /// 添加事件触发器
    /// </summary>
    /// <param name="type">事件触发器类型</param>
    /// <param name="callback">回调方法</param>
    private void AddTrigger(EventTriggerType type, UnityAction<BaseEventData> callback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
        trigger.AddListener(callback);
        entry.callback = trigger;
        entry.eventID = type;
        eventTrigger.triggers.Add(entry);
    }

    //鼠标按下
    private void OnPointerDown(BaseEventData data)
    {
        if (this.disabled)
            return;
        SetColor(pressedColor);
        AudioSystem.PlaySound(confirm);
    }

    //鼠标放开
    private void OnPointerUp(BaseEventData data)
    {
        if (this.disabled)
            return;
        SetColor(normalColor);
    }

    //光标进入
    private void OnPointerEnter(BaseEventData data)
    {
        if (this.disabled)
            return;

        actived = true;
        AudioSystem.PlaySound(move);
        SetColor(pressedColor);

#if UNITY_WINDOWS || UNITY_EDITOR
        text.text = "> " + originText + " <";
#endif
        
    }

    //光标离开
    private void OnPointerExit(BaseEventData data)
    {
        if (this.disabled)
            return;
        actived = false;
        SetColor(normalColor);
#if UNITY_WINDOWS || UNITY_EDITOR
        text.text = originText;
#endif
    }

    //光标点击
    private void OnPoinerClick(BaseEventData data)
    {
        if (this.disabled)
            return;

        if(PointerClicked != null)
            PointerClicked.Invoke();
    }

    //TODO 为了进一步检测光标是否在按钮上
    private void OnEnable()
    {
        
    }

    public void SetColor(Color color)
    {
        text.color = color;
    }

    public void SetText(string s)
    {
        if (string.IsNullOrEmpty(s))
            return;
        text.text = s;
    }

    /// <summary>
    /// 设置按钮的激活状态（鼠标划过时的状态）
    /// </summary>
    /// <param name="b">是否激活</param>
    public void SetActive(bool b)
    {
        if (b)
            OnPointerEnter(null);
        else
            OnPointerExit(null);
    }

    public void Click()
    {
        OnPointerDown(null);
        OnPointerUp(null);
    }
}
