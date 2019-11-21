using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviourExtensionCoroutines
{
    public static SoundManager instance;
    public Action<AudioSource, Transform, bool> ikhebtitled;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void PlaySound(AudioSource clip, Transform position, bool loop, int delayInSeconds)
    {
        this.StartCoroutine(() =>
        {
           gameObject.SetActive(false);
        }, delayInSeconds);
    }
}
