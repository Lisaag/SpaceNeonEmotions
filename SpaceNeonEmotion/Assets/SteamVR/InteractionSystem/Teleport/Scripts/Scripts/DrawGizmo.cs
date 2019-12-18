using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmo : MonoBehaviour
{
    public bool isShapeLocation;
    public bool isDropoffLocation;
    public GameObject attachedObject;
    public GameObject attachedHologramLoc;
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(this.transform.position, 0.1f);
    }
}
