using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WalkingBehaviour : MonoBehaviour
{
    [SerializeField]private bool isMoving;
    [SerializeField]private GameObject moveTowardsObject;
    private Animator animator;
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

        if (transform.position == moveTowardsObject.transform.position)
        {
            StopMoving();
            CheckForPlacement();
        }
    }

    public void StopMoving()
    {
        animator.SetBool("isWalking", false);
        isMoving = false;
    }

    public void CheckForPlacement()
    {

    }
}
