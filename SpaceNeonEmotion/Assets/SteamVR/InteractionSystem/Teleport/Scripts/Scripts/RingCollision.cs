using UnityEngine;
using Valve.VR;

public class RingCollision : MonoBehaviour
{
    public SteamVR_Input_Sources LeftInputsource = SteamVR_Input_Sources.LeftHand;
    public SteamVR_Input_Sources RightInputsource = SteamVR_Input_Sources.RightHand;
    public SteamVR_Action_Vibration vibrate;


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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("WIRE IS KLAAR");
            collisionBehaviour.finishedWire = true;
            checkpointId = -1;
            cp.MoveRingToStartPoint();
            wmg.placeNewWire();
            collisionBehaviour.finishedWire = false;
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            cp.MoveRingToCheckpoint(checkpointId);
            Reset();
        }
    }

    private void Pulse(float delay, float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        vibrate.Execute(delay, duration, frequency, amplitude, source);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            collisionBehaviour.reachedCheckpoint = true;
            checkpointId = other.GetComponent<CheckPointId>().id;
            collisionBehaviour.currentCheckpointId = checkpointId;
            // Debug.Log("chackram collided with checkpoint " + checkpointId);
        }


        if (other.CompareTag("WireEnding"))
        {
            if (!collisionBehaviour.finishedWire)
            {
                Debug.Log("WIRE IS KLAAR");
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
                Pulse(0f, 1f, 100, 40, LeftInputsource);
                Pulse(0f, 1f, 100, 40, RightInputsource);

                ring.GetComponent<Rigidbody>().useGravity = false;

                cp.MoveRingToCheckpoint(collisionBehaviour.currentCheckpointId);
                Reset();
            }
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
