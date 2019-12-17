using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckKinematic : MonoBehaviour
{
    private Rigidbody rb;
    public TutorialManager tut;
    bool hasBeenKin = false;
    bool hasPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
                    SoundManager.instance.PlaySound(tut.done, tut.gameObject, false, 0);
                    hasPlayed = true;
                }

                if (!tut.done.isPlaying && hasPlayed)
                {
                    tut.NextPhase();
                }

            }
        }
    }
}
