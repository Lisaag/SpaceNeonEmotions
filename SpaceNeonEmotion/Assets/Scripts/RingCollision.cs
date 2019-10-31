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

    void Start()
    {
        collisionBehaviour = colliderParent.GetComponent<CollisionBehaviour>();
        audioSource = colliderParent.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Pickup();
            Debug.Log("Object picked up");
        }
    }

    void OnTriggerEnter(Collider other)
    {
       // if (!collisionBehaviour.hasCollided)
      //  {
            if (other.CompareTag("Wire"))
            {
                Debug.Log("BIEM!!1 collider");
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
