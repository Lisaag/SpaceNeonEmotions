using UnityEngine;

public class RingCollision : MonoBehaviour
{
    [SerializeField]
    GameObject colliderParent = null;

    [SerializeField]
    GameObject ring;

    CollisionBehaviour collisionBehaviour;
    AudioSource audioSource;
    // CollisionBehaviour collisionBehaviour = new CollisionBehaviour();

    [SerializeField]
    readonly GameObject checkPoint = null;

    [SerializeField]
    GameObject wire = null;

    Vector3 ringRotatePoint;
    CleanBezierCurve wmg;

    CheckPoint cp;

    void Start()
    {
        collisionBehaviour = colliderParent.GetComponent<CollisionBehaviour>();
        audioSource = colliderParent.GetComponent<AudioSource>();
        wmg = wire.GetComponent<CleanBezierCurve>();
        cp = ring.GetComponent<CheckPoint>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!collisionBehaviour.hasCollided)
        {
            if (other.CompareTag("Wire"))
            {
                cp.MoveRingToCheckpoint();
                Reset();
            }
        }
        if (other.CompareTag("Checkpoint"))
        {
            collisionBehaviour.reachedCheckpoint = true;
            Debug.Log("Checkpointieeee");
        }
    }

    void Reset()
    {
        collisionBehaviour.hasCollided = true;
        audioSource.Play();

        //  ring.transform.position = new Vector3(0, 2, 0);
    }

    void Pickup()
    {
        collisionBehaviour.hasCollided = false;
    }
}
