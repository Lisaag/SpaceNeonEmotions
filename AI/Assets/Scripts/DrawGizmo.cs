using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmo : MonoBehaviour
{
    public bool isButtonPosition;
    public GameObject attachedObject;
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(this.transform.position, 0.1f);
    }
}
