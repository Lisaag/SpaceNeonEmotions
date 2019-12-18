using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject rotator;
    public RotateBehaviour rotatorBehaviour;
    public WalkingBehaviour walkingBehaviour;
    private bool isMoving;
    // Start is called before the first frame update
    void Start()
    {
        rotatorBehaviour = rotator.GetComponent<RotateBehaviour>();
        walkingBehaviour = GetComponent<WalkingBehaviour>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            walkingBehaviour.StopMoving();
            rotatorBehaviour.StopRotating();
            if (this.GetComponent<WalkingBehaviour>().pickupLocation.GetComponentInChildren<HologramShapes>() != null)
            {
                this.GetComponent<WalkingBehaviour>().pickupLocation.GetComponentInChildren<HologramShapes>().LetGo();
            }
        }
    }
    public void GoToPosition(GameObject go)
    {
        RotateToPosition(go);
        MoveToPosition(go);
    }
    public void RotateToPosition(GameObject go)
    {
            rotatorBehaviour.RotateTowards(go);
    }

    public void MoveToPosition(GameObject go)
    {
            walkingBehaviour.MoveTowards(go);
    }
}
