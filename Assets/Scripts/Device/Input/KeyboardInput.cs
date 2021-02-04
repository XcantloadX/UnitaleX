﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 物理键盘输入
/// </summary>
public class KeyboardInput : UndertaleInput {
    KeyCode KC_CONFIRM = KeyCode.Z;
    KeyCode KC_CONFIRM_ALT = KeyCode.Return;
    KeyCode KC_CONFIRM_ALT2 = KeyCode.KeypadEnter;
    KeyCode KC_CONFIRM_ALT3 = (KeyCode)10; //注：这是 Android 上回车的 KeyCode（TMD Unity 文档居然没写！！）

    KeyCode KC_CANCEL = KeyCode.X;
    KeyCode KC_CANCEL_ALT = KeyCode.LeftShift;
    KeyCode KC_CANCEL_ALT2 = KeyCode.RightShift;

    KeyCode KC_MENU = KeyCode.C;
    KeyCode KC_MENU_ALT = KeyCode.LeftControl;

    KeyCode KC_UP = KeyCode.UpArrow;
    KeyCode KC_DOWN = KeyCode.DownArrow;
    KeyCode KC_LEFT = KeyCode.LeftArrow;
    KeyCode KC_RIGHT = KeyCode.RightArrow;

    public override UndertaleInput.ButtonState Confirm
    {
        get { return stateFor(KC_CONFIRM, KC_CONFIRM_ALT, KC_CONFIRM_ALT2, KC_CONFIRM_ALT3); }
    }

    public override UndertaleInput.ButtonState Cancel
    {
        get { return stateFor(KC_CANCEL, KC_CANCEL_ALT, KC_CANCEL_ALT2); }
    }

    public override UndertaleInput.ButtonState Menu
    {
        get { return stateFor(KC_MENU, KC_MENU_ALT); }
    }

    public override UndertaleInput.ButtonState Up
    {
        get { return stateFor(KC_UP); }
    }

    public override UndertaleInput.ButtonState Down
    {
        get { return stateFor(KC_DOWN); }
    }

    public override UndertaleInput.ButtonState Left
    {
        get { return stateFor(KC_LEFT); }
    }

    private UndertaleInput.ButtonState _right = ButtonState.NONE;
    public override UndertaleInput.ButtonState Right
    {
        get { return stateFor(KC_RIGHT); }
    }

    private ButtonState stateFor(KeyCode c)
    {

        if (Input.GetKeyDown(c))
        {
            return ButtonState.PRESSED;
        }
        else if (Input.GetKeyUp(c))
        {
            return ButtonState.RELEASED;
        }
        else if (Input.GetKey(c))
        {
            return ButtonState.HELD;
        }
        else
        {
            return ButtonState.NONE;
        }

    }

    private ButtonState stateFor(KeyCode a, KeyCode b)
    {
        ButtonState aState = stateFor(a);
        if (aState != ButtonState.NONE)
        {
            return aState;
        }
        else
        {
            return stateFor(b);
        }
    }

    private ButtonState stateFor(params KeyCode[] keys)
    {
        foreach(KeyCode key in keys){
            ButtonState state = stateFor(key);
            if (state != ButtonState.NONE)
            {
                return state;
            }
        }
        return ButtonState.NONE;
    }
}
