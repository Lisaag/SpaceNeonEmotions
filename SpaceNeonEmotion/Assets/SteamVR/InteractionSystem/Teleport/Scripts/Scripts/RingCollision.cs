using UnityEngine;
using Valve.VR;

public class RingCollision : MonoBehaviour
{
    [SerializeField]
    GameObject colliderParent = null;

    [SerializeField]
    GameObject ring = null;

    CollisionBehaviour collisionBehaviour;

    [SerializeField]
    GameObject wire = null;

    Vector3 ringRotatePoint;
    CleanBezierCurve wmg;

    CheckPoint cp;

    int checkpointId = 0;

    void Start()
    {
        collisionBehaviour = colliderParent.GetComponent<CollisionBehaviour>();
        wmg = wire.GetComponent<CleanBezierCurve>();
        cp = ring.GetComponent<CheckPoint>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            collisionBehaviour.reachedCheckpoint = true;
            checkpointId = other.GetComponent<CheckPointId>().id;
            collisionBehaviour.currentCheckpointId = checkpointId;
        }


        if (other.CompareTag("WireEnding"))
        {
            if (!collisionBehaviour.finishedWire)
            {
                ring.GetComponent<Rigidbody>().useGravity = false;
                ring.GetComponent<Rigidbody>().isKinematic = true;
                collisionBehaviour.finishedWire = true;
                checkpointId = -1;
                collisionBehaviour.currentCheckpointId = -1;
                cp.MoveRingToStartPoint();
                wmg.placeNewWire();
                collisionBehaviour.finishedWire = false;
            }
        }

        if (!collisionBehaviour.hasCollided)
        {
            if (other.CompareTag("Wire"))
            {
                cp.OnCollisionWithWire();
            }
        }
    }

    void Reset()
    {
        collisionBehaviour.hasCollided = true;
    }
}
