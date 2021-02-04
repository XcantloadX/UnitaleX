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
    public string[] choices = new string[0];
    public object[] values;
    public int currectChoice = 0;
    private bool actived = false;

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
            if (string.IsNullOrEmpty(value))
                return;
            text.text = value;
        }
    }
    private string originText;

    public RectTransform rect { get { return gameObject.GetComponent<RectTransform>(); } }

    public delegate void Clicked(UButton self);
    public event Clicked EventClicked;

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

        if (choices != null && choices.Length > 0)
            DisplayText = choices[currectChoice];

        this.SetColor(normalColor);
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

    //添加事件触发器
    private void AddTrigger(EventTriggerType type, UnityAction<BaseEventData> callback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
        trigger.AddListener(callback);
        entry.callback = trigger;
        entry.eventID = type;
        eventTrigger.triggers.Add(entry);
    }

    //光标按下
    private void OnPointerDown(BaseEventData data)
    {
        if (this.disabled)
            return;

        SetColor(pressedColor);
        AudioSystem.PlaySound(confirm);
        if(choices != null && choices.Length > 0)
            DisplayText = GetModifiedText(choices[NextChoice()]);
    }

    //光标放开
    private void OnPointerUp(BaseEventData data)
    {
    }

    //光标进入
    private void OnPointerEnter(BaseEventData data)
    {
        if (this.disabled)
            return;

        actived = true;
        AudioSystem.PlaySound(move);
        SetColor(pressedColor);

        text.text = GetModifiedText(originText);
    }

    //光标离开
    private void OnPointerExit(BaseEventData data)
    {
        if (this.disabled)
            return;

        actived = false;
        SetColor(normalColor);
        text.text = originText;
    }

    //光标点击
    private void OnPoinerClick(BaseEventData data)
    {
        if (this.disabled)
            return;
        
        if (EventClicked != null)
            EventClicked(this);
    }

    //TODO 为了进一步检测光标是否在按钮上
    private void OnEnable()
    {
        
    }

    public void SetColor(Color color)
    {
        text.color = color;
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

    /// <summary>
    /// 手动触发 Click 事件
    /// </summary>
    public void Click()
    {
        OnPointerDown(null);
        OnPointerUp(null);
    }

    /// <summary>
    /// 获取当前选中项的值
    /// </summary>
    /// <returns>值</returns>
    public object GetValue()
    {
        return values[currectChoice];
    }

    //取得下一个选项的 index
    private int NextChoice()
    {
        currectChoice++;
        if (currectChoice >= choices.Length)
            currectChoice = 0;
        originText = choices[currectChoice];
        return currectChoice;
    }

    //取得加了修饰符的文本（> TEXT <）
    private string GetModifiedText(string s)
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        return "> " + s + " <";
#else
        return s;
#endif
    }
}
