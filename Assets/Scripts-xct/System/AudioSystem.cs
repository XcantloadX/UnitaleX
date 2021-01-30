using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour {

    [Header("Debug")]
    private static AudioSource sound;
    private static AudioSource music;
    /// <summary>
    /// 当前正在播放的 Music AudioClip
    /// </summary>
    public static AudioClip CurrentMusicClip
    {
        get
        {
            InstanceCheck();
            return music.clip;
        }
        set { PlayMusic(value); }
    }
    public static bool IsPlayingMusic
    {
        get { return music.isPlaying; }
    }

    void Awake ()
    {
        //单例模式
        GameObject obj = GameObject.FindObjectOfType<AudioSystem>().gameObject;
        if (obj != null && obj != gameObject)
            GameObject.Destroy(obj);

        //初始化
        if (sound == null)
            sound = gameObject.AddComponent<AudioSource>();
        if (music == null)
            music = gameObject.AddComponent<AudioSource>();

        sound.playOnAwake = false;
        music.playOnAwake = false;

        //加载场景时不销毁
        GameObject.DontDestroyOnLoad(gameObject);
        GameObject.DontDestroyOnLoad(music);
        GameObject.DontDestroyOnLoad(sound);
	}

    public static void PlaySound(AudioClip clip)
    {
        InstanceCheck();
        sound.clip = clip;
        sound.Play();
    }

    public static void PlayMusic(AudioClip clip, bool loop = true)
    {
        InstanceCheck();
        music.clip = clip;
        music.loop = loop;
        music.Play();
        
    }

    public static void StopMusic()
    {
        music.Stop();
    }

    /// <summary>
    /// 是否有指定 AudioClip 正在播放（包括 Sound 和 Music 通道）？
    /// </summary>
    /// <param name="clip">指定的 AudioClip</param>
    /// <returns>是否有指定 AudioClip 正在播放</returns>
    public static bool isPlayingWith(AudioClip clip)
    {
        InstanceCheck();
        //是否有AudioClip正在播放？没有则直接返回假
        bool playing = music.isPlaying || sound.isPlaying;
        if (!playing)
            return false;

        //如果正在播放，则检测指定 AudioClip
        bool isCurrentClip = (music.clip == clip || sound.clip == clip);
        return isCurrentClip;
    }

    //检查场景中是否有SoundSystem实例
    private static void InstanceCheck()
    {
        AudioSystem obj = GameObject.FindObjectOfType<AudioSystem>();
        if (obj == null)
        {
            obj = new GameObject("AudioSystem").AddComponent<AudioSystem>();
            
        }

    }
}
