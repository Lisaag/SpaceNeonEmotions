using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RotateBehaviour : MonoBehaviour
{
    private Transform parentTransform;
    private GameObject lookAtObject;
    private Animator animator;
    private bool isActive;
    public bool IsActive { get { return isActive; } }
    private void Start()
    {
        parentTransform = this.transform.parent.GetComponent<Transform>();
        animator = GetComponentInParent<Animator>();
    }

    public void RotateTowards(GameObject go)
    {
        lookAtObject = go;
        isActive = true;
    }

    private void LateUpdate()
    {
        if (isActive)
        {
            Vector3 oldRotation = parentTransform.localEulerAngles;
            transform.LookAt(lookAtObject.transform);
            if (transform.localEulerAngles != Vector3.zero)
            {
                parentTransform.rotation = Quaternion.RotateTowards(parentTransform.rotation, transform.rotation, Time.deltaTime * AIManager.Instance.turningSpeed);
                if (transform.localEulerAngles.y < oldRotation.y)
                {
                    animator.SetBool("isTurningLeft", true);
                }
                else if (transform.localEulerAngles.y > oldRotation.y)
                {
                    animator.SetBool("isTurningRight", true);
                }
            }
        }
        if (transform.localEulerAngles == Vector3.zero)
        {
            StopRotating();
        }
    }

    public void StopRotating()
    {
        animator.SetBool("isTurningLeft", false);
        animator.SetBool("isTurningRight", false);
        isActive = false;
    }
}
