﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckKinematic : MonoBehaviour
{
    private Rigidbody rb;
    public TutorialManager tut;
    bool hasBeenKin = false;

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
                tut.NextPhase();
            }
        }
    }
}
