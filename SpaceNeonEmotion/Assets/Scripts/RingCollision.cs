using UnityEngine;

public class RingCollision : MonoBehaviour
{
    [SerializeField]
    GameObject colliderParent = null;

    [SerializeField]
    GameObject ring = null;

    CollisionBehaviour collisionBehaviour;
    AudioSource audioSource;

    [SerializeField]
    GameObject wire = null;

    Vector3 ringRotatePoint;
    CleanBezierCurve wmg;

    CheckPoint cp;

    int checkpointId = 0;

    void Start()
    {
        collisionBehaviour = colliderParent.GetComponent<CollisionBehaviour>();
        audioSource = colliderParent.GetComponent<AudioSource>();
        wmg = wire.GetComponent<CleanBezierCurve>();
        cp = ring.GetComponent<CheckPoint>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            collisionBehaviour.reachedCheckpoint = true;
            checkpointId = other.GetComponent<CheckPointId>().id;
            Debug.Log("chackram collided with checkpoint " + checkpointId);
        }

        if (!collisionBehaviour.hasCollided)
        {
            if (other.CompareTag("Wire"))
            {
                cp.MoveRingToCheckpoint(checkpointId);
                Reset();
            }
        }

        if (other.CompareTag("WireEnding"))
        {
            collisionBehaviour.wireIndex++;
            cp.MoveRingToStartPoint();
            StartCoroutine(wmg.RemoveWire());
        }
    }

    void Reset()
    {
        collisionBehaviour.hasCollided = true;
        audioSource.Play();
    }

    void Pickup()
    {
        collisionBehaviour.hasCollided = false;
    }
}
