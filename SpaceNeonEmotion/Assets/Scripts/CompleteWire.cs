using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteWire : MonoBehaviour
{
    private bool finishedWire = false;

    [SerializeField]
    GameObject wire;

    [SerializeField]
    GameObject ring;

    CleanBezierCurve cbc;
    CheckPoint cp; 

    private void Start()
    {
        cbc = wire.GetComponent<CleanBezierCurve>();
        cp = ring.GetComponent<CheckPoint>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ChakramCollider")){
            cp.MoveRingToStartPoint();
            wmg.placeNewWire();
        }
    }
}
