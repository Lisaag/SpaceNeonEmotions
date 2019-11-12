using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    const float G = 667.4f;
    public float newDrag = 2.5f;
    public Rigidbody rb;
    [SerializeField] private GameObject forcefield;
    //public static List<Attractor> Attractors;

    private void FixedUpdate()
    {
        //foreach (Attractor attractor in Attractors)
        //{
        //    if (attractor != this)
        //        Attract(attractor);
        //}
    }

    private bool CheckTags(Collider other)
    {
        if (this.CompareTag("CubeLocation") && other.CompareTag("HologramCube")
            || this.CompareTag("SphereLocation") && other.CompareTag("HologramSphere")
                || this.CompareTag("TriangleLocation") && other.CompareTag("HologramTriangle"))
        {
            return true;
        }
            return false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (CheckTags(other))
            this.forcefield.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (CheckTags(other))
            this.forcefield.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        if (CheckTags(other))
            Attract(other.gameObject.GetComponent<Rigidbody>());
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
        //if (!rb.CompareTag("CubeLocation") && rbToAttract.CompareTag("HologramCube") || !rb.CompareTag("SphereLocation") && rbToAttract.CompareTag("HologramSphere") || !rb.CompareTag("TriangleLocation") && rb.CompareTag("HologramTriangle")){
        //    return;
        //}
        Debug.Log("Attracting");
        if (rbToAttract.drag != newDrag)
        {
            rbToAttract.drag = newDrag;
        }
        rbToAttract.useGravity = false;

        //Rigidbody rbToAttract = objToAttract.rb;
        Vector3 direction = rb.position - rbToAttract.position;
        float distance = direction.magnitude;

        if (distance == 0f)
        {
            return;
        }
        float forceMagnitude = (rb.mass * rbToAttract.mass) * Mathf.Pow(distance, 2);
        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }
}
