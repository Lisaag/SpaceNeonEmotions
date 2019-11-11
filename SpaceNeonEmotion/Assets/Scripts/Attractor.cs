using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    const float G = 667.4f;
    public Rigidbody rb;
    public static List<Attractor> Attractors;

    private void FixedUpdate()
    {
        //foreach (Attractor attractor in Attractors)
        //{
        //    if (attractor != this)
        //        Attract(attractor);
        //}
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() != null)
        {
            Attract(other.gameObject.GetComponent<Rigidbody>());
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            Attract(collision.gameObject.GetComponent<Rigidbody>());
        }
    }
    //private void OnEnable()
    //{
    //    if (Attractors == null)
    //    {
    //        Attractors = new List<Attractor>();
    //    }
    //    Attractors.Add(this);
    //}

    //private void OnDisable()
    //{
    //    Attractors.Remove(this);
    //}
    void Attract(Rigidbody rbToAttract)
    {
        Debug.Log("Attracting");
        rbToAttract.drag = 2.5f;
        //Rigidbody rbToAttract = objToAttract.rb;
        Vector3 direction = rb.position - rbToAttract.position;
        float distance = direction.magnitude;

        if (distance == 0f)
        {
            return;
        }
        float forceMagnitude = (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }
}
