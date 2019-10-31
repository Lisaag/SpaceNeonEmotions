using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HologramCube : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("CubeLocation"))
        {
            Destroy(collision.gameObject);
            this.transform.rotation = collision.gameObject.transform.rotation;
            this.GetComponent<Rigidbody>().isKinematic = true;
            this.transform.position = collision.transform.position;
            gameObject.AddComponent<CubeRotator>();
        }
    }
}
