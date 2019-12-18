using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinProps : MonoBehaviour
{
    public int speed = 1;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0, Time.deltaTime * speed, 0); //rotates 50 degrees per second around z axis
    }
}
