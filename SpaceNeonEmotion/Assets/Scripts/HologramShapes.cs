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
    public AudioSource clip;

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
        this.GetComponent<Interactable>().enabled = false;
        this.GetComponent<Throwable>().attachmentFlags = Hand.AttachmentFlags.TurnOnKinematic;
        this.transform.rotation = colObj.gameObject.transform.rotation;
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.transform.position = colObj.transform.position;
        this.gameObject.AddComponent<CubeRotator>();
        colObj.gameObject.SetActive(false);
        GameManager.Instance.CheckPlacement();
        SoundManager.instance.PlaySound(clip, gameObject, false, 0);
    }
    public void LetGo()
    {
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

        this.transform.parent = null;
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<Rigidbody>().drag = 0f;
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Interactable>().enabled = true;
        this.GetComponent<Throwable>().attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.TurnOnKinematic;
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
        Destroy(this.GetComponent<CubeRotator>());
    }
}
