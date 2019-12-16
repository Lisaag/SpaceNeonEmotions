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
        gameObject.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_BaseColor", new Color(color.r, color.g, color.b, alphaValue));
        meshRenderer.material.color = new Color(color.r, color.g, color.b, alphaValue);

        Debug.Log("changing graph point color");
    }
}
