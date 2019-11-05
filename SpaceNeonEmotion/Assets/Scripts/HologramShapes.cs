using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class HologramShapes : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("CubeLocation") && this.transform.CompareTag("HologramCube"))
        {
            GameManager.Instance.cubePlaced = true;
            SetLocation(collision.gameObject);

        }

        else if(collision.transform.CompareTag("SphereLocation") && this.transform.CompareTag("HologramSphere"))
        {
            GameManager.Instance.spherePlaced = true;
            SetLocation(collision.gameObject);

        }
        else if (collision.transform.CompareTag("TriangleLocation") && this.transform.CompareTag("HologramTriangle"))
        {
            GameManager.Instance.trianglePlaced = true;
            SetLocation(collision.gameObject);

        }
    }

    private void SetLocation(GameObject colObj)
    {
        if (this.transform.parent.GetComponent<Hand>())
        {
            this.transform.parent.GetComponent<Hand>().DetachObject(this.gameObject);
        }
        this.GetComponent<Interactable>().enabled = false;
        colObj.gameObject.SetActive(false);
        this.transform.rotation = colObj.gameObject.transform.rotation;
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.transform.position = colObj.transform.position;
        gameObject.AddComponent<CubeRotator>();
        GameManager.Instance.checkPlacement();
    }
}
