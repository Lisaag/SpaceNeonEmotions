using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float speedY, strength, speed;

    private void Start()
    {
        speedY = Random.Range(0.01f, 0.03f);
        var scale = Random.Range(0.05f, 0.17f);
        transform.localScale = new Vector3(scale, scale, scale);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        strength = Random.Range(0.01f, 0.03f);
        transform.position += (Mathf.Sin(2 * Mathf.PI * speed * Time.time) - Mathf.Sin(2 * Mathf.PI * speed * (Time.time - Time.deltaTime))) * transform.forward * strength;
        strength = Random.Range(0.01f, 0.03f);
        transform.position += (Mathf.Sin(2 * Mathf.PI * speed * Time.time) - Mathf.Sin(2 * Mathf.PI * speed * (Time.time - Time.deltaTime))) * transform.right * strength;
        transform.position += new Vector3(0, speedY / 2, 0);
    }
}
