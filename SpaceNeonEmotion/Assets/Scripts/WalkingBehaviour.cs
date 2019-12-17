using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WalkingBehaviour : MonoBehaviour
{
    [SerializeField]private bool isMoving;
    [SerializeField]private GameObject moveTowardsObject;
    [SerializeField] private GameObject pickupLocation;
    private Animator animator;
    bool isWaiting;
    public bool IsMoving { get { return isMoving; } }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void MoveTowards(GameObject go)
    {
        animator.SetBool("isWalking", true);
        moveTowardsObject = go;
        isMoving = true;
    }
    void LateUpdate()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTowardsObject.transform.position, Time.deltaTime * AIManager.Instance.movementSpeed);
        }

        if (transform.position == moveTowardsObject.transform.position && isMoving)
        {
            StopMoving();
            CheckForPlacement(moveTowardsObject);
        }
    }

    public void StopMoving()
    {
        animator.SetBool("isWalking", false);
        isMoving = false;
    }

    public void CheckForPlacement(GameObject go)
    {
        DrawGizmo lookatGizmo = moveTowardsObject.GetComponent<DrawGizmo>();
        if (lookatGizmo.isShapeLocation)
        {
            transform.LookAt(lookatGizmo.attachedObject.transform);
            Vector3 tempTrans =lookatGizmo.attachedObject.transform.position;
            tempTrans.y = this.transform.position.y;
            transform.LookAt(tempTrans);
            lookatGizmo.attachedObject.GetComponent<HologramShapes>().Delocate(pickupLocation.gameObject);
            AIManager.Instance.MoveRobotToDropoff();
            return;
        }

        if (lookatGizmo.isDropoffLocation)
        {
            pickupLocation.GetComponentInChildren<HologramShapes>().LetGo();
            StartCoroutine(AIManager.Instance.MoveRobotToRandom(5f));
            return;
        }
        StartCoroutine(AIManager.Instance.MoveRobotToRandom(1f));
    }
}
