using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamCheck : MonoBehaviour
{
    public GameObject collided;

    private void OnTriggerEnter(Collider collision)
    {
        collided = collision.gameObject;
    }
}
