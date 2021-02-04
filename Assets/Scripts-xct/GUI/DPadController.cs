using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem = UnityEngine.EventSystems.EventSystem;
using ButtonState = KeyboardInput.ButtonState;

public class DPadController : MonoBehaviour {

    public class InputWrapper : UndertaleInput
    {
        public override ButtonState Confirm { get { return controller == null ? ButtonState.NONE : controller.GetButtonState(DPadButtonType.Confirm); } }
        public override ButtonState Cancel { get { return controller == null ? ButtonState.NONE : controller.GetButtonState(DPadButtonType.Cancel); } }
        public override ButtonState Menu { get { return controller == null ? ButtonState.NONE : controller.GetButtonState(DPadButtonType.Menu); } }
        public override ButtonState Up { get { return controller == null ? ButtonState.NONE : controller.GetButtonState(DPadButtonType.Up); } }
        public override ButtonState Down { get { return controller == null ? ButtonState.NONE : controller.GetButtonState(DPadButtonType.Down); } }
        public override ButtonState Left { get { return controller == null ? ButtonState.NONE : controller.GetButtonState(DPadButtonType.Left); } }
        public override ButtonState Right { get { return controller == null ? ButtonState.NONE : controller.GetButtonState(DPadButtonType.Right); } }

        public DPadController controller;

        public InputWrapper(DPadController controller)
        {
            this.controller = controller;
        }
    }

    /// <summary>
    /// 是否使用振动
    /// </summary>
    public bool useVibrator = true;
    /// <summary>
    /// 震动时间，毫秒
    /// </summary>
    public int vibratingTime = 30; //TODO 支持自定义
    private static bool inited = false; //保证只有一个实例
    public bool displaying { get; private set; }
    /// <summary>
    /// 在 Editor 模式下是否显示
    /// </summary>
    public bool displayInEditor = false;

    public DPadButton[] buttons;
    public Dictionary<DPadButtonType, DPadButton> buttonsDictionary = new Dictionary<DPadButtonType, DPadButton>(7);

    private InputWrapper inputWrapper;

	void Start () 
    {
        GameObject.DontDestroyOnLoad(this);

        //初始化 wrapper
        inputWrapper = new InputWrapper(this);
        GlobalControls.SetInput(inputWrapper); //GlobalControls 里设置了才能生效

        displaying = buttons[0].gameObject.activeSelf;

        //遍历所有 DPad 按钮
        foreach (DPadButton btn in buttons)
        {
            btn.controller = this;
            buttonsDictionary.Add(btn.buttonType, btn);
        }

        useVibrator = GlobalSettings.settings.useVibrator;
        SetDisplayRaw(GlobalSettings.settings.useDPad);
        
	}
	
	void Update () 
    {
        if (!SceneSystem.IsInGame)
        {
            SetDisplayRaw(false);
            Destroy(gameObject);
        }
	}

    /// <summary>
    /// 如果启用了震动，触发震动
    /// </summary>
    public void Vibrate()
    {
        if (this.useVibrator)
            StartCoroutine(AsyncVibrate());
    }


    public void SetDisplayRaw(bool display)
    {
        this.displaying = display;

        foreach (DPadButton btn in buttons)
        {
            btn.controller = this; //很奇怪，这个有时会自己掉
            btn.gameObject.SetActive(display);
        }

        if (display)
            GlobalControls.SetInput(inputWrapper);
        else
            GlobalControls.SetInput(new KeyboardInput());
    }

    /// <summary>
    /// 隐藏或显示 DPad
    /// </summary>
    public void SetDisplay(bool display)
    {
        SetDisplayRaw(displaying);
        GlobalSettings.settings.useDPad = display;
    }

    /// <summary>
    /// 切换 DPad 的显示
    /// </summary>
    public void SwitchDisplay()
    {
        displaying = !displaying;
        SetDisplay(displaying);
    }

    /// <summary>
    /// 获取某个按键的状态
    /// </summary>
    /// <param name="type">按键类型</param>
    /// <returns>按键状态</returns>
    public ButtonState GetButtonState(DPadButtonType type)
    {
        DPadButton btn = null;
        buttonsDictionary.TryGetValue(type, out btn);
        return btn != null ? btn.buttonState : ButtonState.NONE;
    }

    IEnumerator AsyncVibrate()
    {
        Vibration.Vibrate(vibratingTime);
        yield break;
    }
}
