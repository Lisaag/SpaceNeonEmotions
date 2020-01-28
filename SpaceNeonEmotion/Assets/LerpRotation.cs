using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpRotation : MonoBehaviour
{
    public Vector3 sinMultiplier;
    public float speed;

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = sinMultiplier.x * Mathf.Sin(Time.time * speed);
        float y = sinMultiplier.y * Mathf.Sin(Time.time * speed);
        float z = sinMultiplier.z * Mathf.Sin(Time.time * speed);

        transform.localEulerAngles = new Vector3(x, y, z);
    }
}
