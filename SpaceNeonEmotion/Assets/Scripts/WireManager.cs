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

    [SerializeField]
    GameObject steamCamera;

    void Update()
    {
        if (teleport.isOnWirePoint)
        {
            wire.GetComponent<CleanBezierCurve>().playerHeight = steamCamera.transform.localPosition.y;
            wire.SetActive(true);
        }
    }
}
