using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    public int[] colA = new int[3] { 1, 1, 1 };
    [ColorUsageAttribute(true,true)]
    public Color color;
    public float intensity;
    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = this.GetComponent<Renderer>();
        StartCoroutine(ChangeColor());
    }

    IEnumerator ChangeColor()
    {
        //Debug.Log(color.r);
        //Debug.Log("Changing colors to: " + colA[0] + " | " + colA[1] + " | " + colA[2] + "| Intensity: " + intensity);
        //rend.material.SetColor("_EmissionColor", new Color(colA[0], colA[1], colA[2]));
        //color = new Color(255, 0, 0);
        //color.r++;
        color = new Vector4(colA[0], colA[1], colA[2], 0);
        rend.material.SetColor("_EmissionColor", color);
        rend.material.SetFloat("EmissionIntensity", 0);
        yield return new WaitForSeconds(2f);
        StartCoroutine(ChangeColor());
    }
}
