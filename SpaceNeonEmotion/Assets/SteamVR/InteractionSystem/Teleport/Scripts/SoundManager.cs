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

        GameObject soundPlayer = new GameObject("SoundPlayer");

        soundPlayer.transform.parent = clipInfoObject.clipObject.transform;

        soundPlayer.transform.localRotation = new Quaternion(0, 0, 0, 1);
        soundPlayer.transform.localScale = Vector3.zero;
        soundPlayer.transform.localPosition = Vector3.zero;

        AudioSource soundHolderClip = clip;

        if (clipInfoObject.clipObject.GetComponentInChildren<AudioSource>() == null)
        {
            soundHolderClip = soundPlayer.AddComponent<AudioSource>();
        }

        soundHolderClip.PlayOneShot(soundHolderClip.clip);
        Destroy(soundPlayer, soundHolderClip.clip.length + 1);

    }
}
