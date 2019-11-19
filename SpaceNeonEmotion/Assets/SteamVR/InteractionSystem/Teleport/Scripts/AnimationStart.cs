using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStart : MonoBehaviour
{
    public AnimationClip clip;
    public Animation animation;

    private void Start()
    {
        animation = GetComponent<Animation>();
        animation.wrapMode = WrapMode.Once;
    }

    public void StartAnimation()
    {
        //animation.AddClip(clip, "clip");
        animation.Play("PortalResize");
    }
}
