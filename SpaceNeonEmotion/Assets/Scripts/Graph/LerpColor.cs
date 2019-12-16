using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpColor : MonoBehaviour
{
    [SerializeField]
    MeshRenderer meshRenderer;

    [SerializeField]
    Gradient gradient;

    [SerializeField]
    float alphaValue;

    private void Start()
    {
        Color color = gradient.Evaluate(transform.localPosition.y / 6.0f);

        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(mpb);
        mpb.SetColor("_BaseColor", new Color(color.r, color.g, color.b, alphaValue));
        meshRenderer.SetPropertyBlock(mpb);
    }
}
