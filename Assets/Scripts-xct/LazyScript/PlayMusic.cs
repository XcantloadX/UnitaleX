using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : LazyScriptBase {

    public AudioClip clip;
    public bool loop = true;
    public bool resetWhilePlaying = false;//指定 clip 已经在播放时是否重新播放一遍？
    public bool stopOnDestory = true;
    private static PlayMusic instance;

	void Start ()
    {
        if (!AudioSystem.isPlayingWith(clip))
        {
            Play();
            return;
        }
        if (resetWhilePlaying)
            Play();

        GameObject.Destroy(gameObject);
    }

    void Play()
    {
        AudioSystem.PlayMusic(clip, loop);
    }

    void OnDestory()
    {
        AudioSystem.StopMusic();
        Debug.Log("DESTORYED");
    }

}
