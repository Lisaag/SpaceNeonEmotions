using System.Collections;
using System.Collections.Generic;
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
    GameObject checkPoint = null;

    [SerializeField]
    GameObject wire = null;

    Vector3 ringRotatePoint;
    WireMeshGeneration wmg;

    void Start()
    {
        collisionBehaviour = colliderParent.GetComponent<CollisionBehaviour>();
        audioSource = colliderParent.GetComponent<AudioSource>();
        wmg = wire.GetComponent<WireMeshGeneration>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Pickup();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            MoveRingToCheckpoint();
        }
    }

    void MoveRingToCheckpoint()
    {
        ringRotatePoint = wmg.ringDir;
        Debug.Log(ringRotatePoint);

        this.transform.position = checkPoint.transform.position;
        Debug.Log("Chakram moved");

        Vector3 dir = ringRotatePoint - this.transform.position;
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = rot;

        Vector3 temp = transform.rotation.eulerAngles;
        temp.x += 90.0f;
        transform.rotation = Quaternion.Euler(temp);
    }

    void OnTriggerEnter(Collider other)
    {
       // if (!collisionBehaviour.hasCollided)
      //  {
            if (other.CompareTag("Wire"))
            {
            MoveRingToCheckpoint();
            Reset();
            }
       // }
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
