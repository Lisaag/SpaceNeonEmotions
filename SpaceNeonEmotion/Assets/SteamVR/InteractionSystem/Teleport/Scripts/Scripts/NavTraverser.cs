using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavTraverser : MonoBehaviour
{
    public NavMeshAgent agent;
    public Vector3 destination;
    public int pospos;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            pospos++;
            if (pospos >= AIManager.Instance.movementPositions.Count)
            {
                pospos = 0;
            }
            destination = AIManager.Instance.movementPositions[pospos].transform.position;
            agent.SetDestination(destination);
            animator.SetBool("isWalking", true);
        }

        if (transform.position.x == destination.x && transform.position.z == destination.z)
        {
            animator.SetBool("isWalking", false);
        }
    }
}
