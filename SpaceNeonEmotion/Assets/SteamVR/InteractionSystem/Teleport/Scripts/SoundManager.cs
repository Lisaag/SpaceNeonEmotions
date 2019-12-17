using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviourExtensionCoroutines
{
    public int baseHeartrate;
    public static SoundManager instance;
    private Action playSoundClip;
    private ClipInfo clipInfoObject;

    private struct ClipInfo
    {
        public AudioSource clip;
        public GameObject clipObject;
        public bool loop;

        public ClipInfo(AudioSource clip, GameObject position, bool loop)
        {
            this.clip = clip ?? throw new ArgumentNullException(nameof(clip));
            this.clipObject = position ?? throw new ArgumentNullException(nameof(position));
            this.loop = loop;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (SoundManager.instance == null)
            instance = this;
        else
            Destroy(gameObject);

        playSoundClip = ActuallyPlay;
    }

    public void PlaySound(AudioSource clip, GameObject movingObject, bool loop, int delayInSeconds)
    {
        clipInfoObject = new ClipInfo(clip, movingObject, loop);

        if (delayInSeconds > 0)
            StartCoroutine(this, playSoundClip, delayInSeconds);

        else
            ActuallyPlay();
    }

    private void ActuallyPlay()
    {
        var clip = clipInfoObject.clip;
        clip.loop = clipInfoObject.loop;

        if (clipInfoObject.clipObject == null)
        {
            clipInfoObject.clipObject = new GameObject("SoundPlayer");
            clipInfoObject.clipObject.transform.parent = gameObject.transform;
        }

        AudioSource soundHolderClip;
        soundHolderClip = clipInfoObject.clipObject.GetComponent<AudioSource>();

        if (soundHolderClip != null)
        {
            soundHolderClip.clip = clip.clip;
            soundHolderClip.loop = clip.loop;
        }
        else
        {
            soundHolderClip = clipInfoObject.clipObject.AddComponent<AudioSource>();
            soundHolderClip.clip = clip.clip;
            soundHolderClip.loop = clip.loop;
        }

        if (!soundHolderClip.isPlaying)
            soundHolderClip.PlayOneShot(soundHolderClip.clip);
    }
}
