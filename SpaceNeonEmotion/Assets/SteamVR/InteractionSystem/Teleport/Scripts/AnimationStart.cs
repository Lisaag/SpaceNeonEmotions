using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStart : MonoBehaviour
{
    public Animator animation;

    private void Start()
    {
        animation = GetComponent<Animator>();
    }

    public void StartAnimation()
    {
        animation.SetTrigger("PlayAnimation");
    }
}
