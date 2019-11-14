using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateViveController : MonoBehaviour
{
    public int speed = 50;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0, 1 * Time.deltaTime * speed, 0);
    }
}
