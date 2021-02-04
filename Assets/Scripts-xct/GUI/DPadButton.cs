using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ButtonState = KeyboardInput.ButtonState;

public enum DPadButtonType
{
    Confirm,
    Cancel,
    Menu,
    Up,
    Down,
    Left,
    Right
}

public class DPadButton : MonoBehaviour
{
    public ButtonState buttonState = ButtonState.NONE;
    private float timer = 0;
    private float pressedTime = 0;

    private RectTransform trans;
    private Rect rect;
    public DPadController controller;
    public DPadButtonType buttonType;
    

    void Start()
    {
        trans = gameObject.GetComponent<RectTransform>();
        rect = trans.rect;
    }

    void Update()
    {
        return;

        //如果为 PRESSED，开始计时
        if (buttonState == ButtonState.PRESSED)
        {
            if (timer > 0)
            {
                buttonState = ButtonState.HELD;
                timer = 0;
                //Debug.Log("目前是 HELD 状态");
            }
            else
            {
                timer += Time.deltaTime;
                //Debug.Log("目前是 PRESSED 状态");
                return;
            }
            
        }


        //ButtonState.PRESSED 如果状态持续超过一帧就变为 HELD，timer > 0 即 PRESSED 触发后的下一帧
        //！还必须检测当前状态是不是 PRESSED，要不然在低帧率下会出问题（按键无法释放）！
        /*if (timer > 0 && buttonState == ButtonState.PRESSED) 
        {
            buttonState = ButtonState.HELD;
            timer = 0; //重置 timer
            Debug.Log("目前是 HELD 状态");
        }*/

        //TODO: DPad 按钮支持滑动按下（按下一个按钮，不放开，在滑动到另一个按钮）
    }



    //按钮按下事件
    public void OnPressed()
    {
        buttonState = ButtonState.PRESSED;
        StartCoroutine(ButtonPressed());
#if UNITY_ANDROID || UNITY_IOS
        controller.Vibrate();
#endif
        //Debug.Log("按键按下");
    }

    //按钮放开事件
    public void OnReleased()
    {
        buttonState = ButtonState.RELEASED;
        StartCoroutine(ButtonReleased());
        //Debug.Log("按键释放");
    }

    private IEnumerator ButtonPressed()
    {
        buttonState = ButtonState.PRESSED;
        yield return 0;
        if(buttonState == ButtonState.PRESSED) //一帧后仍然是按着的话
            buttonState = ButtonState.HELD;
    }

    private IEnumerator ButtonReleased()
    {
        buttonState = ButtonState.RELEASED;
        yield return 0;
        if (buttonState == ButtonState.RELEASED) //一帧后仍然是放开的话
            buttonState = ButtonState.NONE;
    }
}
