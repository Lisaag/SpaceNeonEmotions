using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class WireManager : MonoBehaviour
{
    [SerializeField]
    Teleport teleport = null;

    [SerializeField]
    GameObject wire = null;

    GameObject steamCamera = null;
    bool isMeasured = false;

    void Update()
    {
        if (teleport.isOnWirePoint && !isMeasured)
        {
            steamCamera = GameObject.Find("Player").GetComponentInChildren<Camera>().gameObject;
 
            Debug.Log("WireManager() camera height: " + steamCamera.transform.localPosition.y);
            wire.GetComponent<CleanBezierCurve>().playerHeight = steamCamera.transform.localPosition.y;
            wire.SetActive(true);
            isMeasured = true;
        }
    }
}
