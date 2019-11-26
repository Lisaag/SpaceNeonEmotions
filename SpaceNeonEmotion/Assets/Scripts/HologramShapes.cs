using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class HologramShapes : MonoBehaviour
{
    public GameObject cubeLoc;
    public GameObject triangleLoc;
    public GameObject sphereLoc;
    //public GameObject forcefieldSphere;
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

    //public void enableSphere()
    //{
    //    forcefieldSphere.SetActive(true);
    //}

    //public void disableSphere()
    //{
    //    forcefieldSphere.SetActive(false);
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            LetGo();
        }
    }
    private void SetLocation(GameObject colObj)
    {
        Destroy(this.GetComponent<Throwable>());
        //Destroy(this.GetComponent<Interactable>());//.enabled = false;
        this.GetComponent<Interactable>().enabled = false;
        colObj.gameObject.SetActive(false);
        this.transform.rotation = colObj.gameObject.transform.rotation;
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.transform.position = colObj.transform.position;
        gameObject.AddComponent<CubeRotator>();
        GameManager.Instance.CheckPlacement();
    }
    public void LetGo()
    {
        this.transform.parent = null;
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().drag = 0f;

        if (this.CompareTag("HologramCube"))
        {
            cubeLoc.gameObject.SetActive(true);
            cubeLoc.GetComponentInChildren<Attractor>().forcefield.SetActive(false);
        }
        else if (this.CompareTag("HologramTriangle"))
        {
            triangleLoc.gameObject.SetActive(true);
            triangleLoc.GetComponentInChildren<Attractor>().forcefield.SetActive(false);
        }
        else if (this.CompareTag("HologramSphere"))
        {
            sphereLoc.gameObject.SetActive(true);
            sphereLoc.GetComponentInChildren<Attractor>().forcefield.SetActive(false);
        }
    }

    public void Delocate(GameObject obj)
    {
        if (this.CompareTag("HologramTriangle"))
        {
            GameManager.Instance.trianglePlaced = false;
        } else if (this.CompareTag("HologramCube"))
        {
            GameManager.Instance.cubePlaced = false;
        } else if (this.CompareTag("HologramSphere"))
        {
            GameManager.Instance.spherePlaced = false;
        }
        this.transform.parent = obj.transform;
        this.transform.position = obj.transform.position;
        if (gameObject.GetComponent<Interactable>().enabled == false)
        {
            gameObject.GetComponent<Interactable>().enabled = true;
            gameObject.AddComponent<Throwable>();
        }
        Destroy(this.GetComponent<CubeRotator>());

    }
}
