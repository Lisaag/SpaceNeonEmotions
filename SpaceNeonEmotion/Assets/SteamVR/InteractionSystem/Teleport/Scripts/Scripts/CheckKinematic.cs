using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckKinematic : MonoBehaviour
{
    private Rigidbody rb;
    public TutorialManager tut;
    bool hasBeenKin = false;
    bool hasPlayed = false;
    AudioSource[] sources;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    sources = tut.gameObject.GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.isKinematic)
        {
            hasBeenKin = true;
        }

        else
        {
            if (hasBeenKin)
            {
                if (!hasPlayed)
                {
                    tut.StopAllHints();
                    sources[sources.Length - 1].Play();
                    hasPlayed = true;
                }

                if (!sources[sources.Length - 1].isPlaying && hasPlayed == true)
                {
                    tut.NextPhase();
                }

            }
        }
    }
}
