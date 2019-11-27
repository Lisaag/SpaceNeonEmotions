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
    bool isMeasured = false;

    void Update()
    {
        if (teleport.isOnWirePoint && !isMeasured)
        {
            Debug.Log("WireManager() camera height: " + steamCamera.transform.localPosition.y);
            wire.GetComponent<CleanBezierCurve>().playerHeight = steamCamera.transform.localPosition.y;
            wire.SetActive(true);
            isMeasured = true;
        }
    }
}
