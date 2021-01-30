using UnityEngine;

/// <summary>
/// Disables vertical sync, sets resolution to 640x480 and sets the target framerate to 60FPS.
/// Mostly here to prevent high refresh rate screens from being unable to play the game as a lot of scripts are tied to per-frame Update loops.
/// </summary>
public class ScreenResolution : MonoBehaviour
{
    public static bool hasInitialized = false;

    private void Start()
    {
        if (hasInitialized)
        {
            Destroy(this);
            return;
        }
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        //强制设置分辨率会导致画面强制缩放，变得很模糊
        //因为现在改成了 World Canvas，所以用不着缩放
        //TODO
#if UNITY_WINDOWS || UNITY_LIUNX || UNITY_MACOS
        Screen.SetResolution(640, 480, false, 0);
#endif
        hasInitialized = true;
    }
}