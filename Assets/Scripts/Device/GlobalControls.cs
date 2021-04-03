using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls that should be active on all screens. Pretty much a hack to allow people to reset.
/// <para>
/// 注：若要实现一种新的 Input，创建一个继承 UndertaleInput 的类，然后调用 SetInput 方法
/// </para>
/// </summary>
public class GlobalControls : MonoBehaviour
{
    /// <summary>
    /// 游戏内逻辑用的 Input（比如菜单选择）
    /// </summary>
    public static UndertaleInput input = new KeyboardInput();
    /// <summary>
    /// Lua 脚本内用的 Input
    /// </summary>
    public static LuaInputBinding luaInput = new LuaInputBinding(input);

    /// <summary>
    /// Control checking.
    /// </summary>
    void Update() {

        if (Input.GetKeyDown(KeyCode.F4))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
        else if (Input.GetKeyDown(KeyCode.F9))
        {
            UserDebugger.instance.gameObject.SetActive(!UserDebugger.instance.gameObject.activeSelf);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (SceneSystem.CurrentSceneName)
            {
                case SceneSystem.LUA_ERROR:
                    SceneSystem.Load(SceneSystem.MOD_SELECT);
                    AudioSystem.StopMusic();
                    StaticInits.Reset();
                    break;
                case SceneSystem.MOD_SELECT:
                case SceneSystem.SETTINGS:
                    SceneSystem.Load(SceneSystem.START_MENU);
                    break;
                case SceneSystem.GAME_OVER:
                    SceneSystem.ExitFromGameOverTo(SceneSystem.MOD_SELECT);
                    break;
                default:
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && SceneSystem.CurrentSceneName == SceneSystem.MOD_SELECT)
            SceneSystem.Load(SceneSystem.START_MENU);
	}

    /// <summary>
    /// 设置输入类型
    /// </summary>
    public static void SetInput(UndertaleInput newInput)
    {
        input = newInput;
        luaInput.SetInput(newInput);
    }
}
