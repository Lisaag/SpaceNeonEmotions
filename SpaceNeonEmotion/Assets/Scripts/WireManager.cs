using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class WireManager : MonoBehaviour
{
    [SerializeField]
    Teleport teleport;

    [SerializeField]
    GameObject wire;

    void Update()
    {
        if (teleport.isOnWirePoint)
        {
            wire.SetActive(true);
        }
    }
}
