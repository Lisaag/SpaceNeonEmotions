using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movementSpeed = 10;
    public float turningSpeed = 60;

    public GameObject rotateTowards;

    public GameObject dropoffLoc;
    public bool droppingOff;
    public GameObject movPos;
    public GameObject shapePos;
    public GameObject carryPos;
    public List<GameObject> movementPositions = new List<GameObject>();
    public List<GameObject> shapePositions = new List<GameObject>();

    private Vector3 oldRota = Vector3.zero;
    public bool isMoving;
    public bool isRotating;
    public bool hasRotated;
    public GameObject toGameObject;
    private Animator animator;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
        foreach (Transform child in movPos.transform)
        {
            movementPositions.Add(child.gameObject);
        }

        foreach (Transform child in shapePos.transform)
        {
            shapePositions.Add(child.gameObject);
        }
        StartCoroutine(RandomMove());
    }
    void Update()
    {
        oldRota = transform.localEulerAngles;
    }

    void RotateTowards(GameObject obj)
    {
        rotateTowards.transform.LookAt(obj.transform);
        if (rotateTowards.transform.localEulerAngles != Vector3.zero)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTowards.transform.rotation, Time.deltaTime * turningSpeed);
            if (transform.localEulerAngles.y < oldRota.y)
            {
                animator.SetBool("isTurningLeft", true);
            }
            else if (transform.localEulerAngles.y > oldRota.y)
            {
                animator.SetBool("isTurningRight", true);
            }
        }
    }
    void MoveToGameObject(GameObject go)
    {
        if (isRotating)
        {
            RotateTowards(go);

            if (rotateTowards.transform.localEulerAngles == Vector3.zero)
            {
                hasRotated = true;
                isRotating = false;
                animator.SetBool("isTurningRight", false);
                animator.SetBool("isTurningLeft", false);
            }
        }
        if (hasRotated)
        {
            if (transform.position != go.transform.position)
            {
                animator.SetBool("isWalking", true);
                transform.position = Vector3.MoveTowards(transform.position, go.transform.position, Time.deltaTime * movementSpeed);
            }
            else if (transform.position == go.transform.position)
            {
                animator.SetBool("isWalking", false);
                if (go.transform.gameObject.name == "DropoffLocation" && GetComponentInChildren<HologramShapes>() != null)
                {
                    GetComponentInChildren<HologramShapes>().LetGo();
                    droppingOff = false;
                }
                if (go.GetComponent<DrawGizmo>().isButtonPosition)
                {
                    //animator.SetTrigger("PressButton");
                    if (go.GetComponent<DrawGizmo>().attachedObject != null)
                    {
                        Vector3 tempTrans = go.GetComponent<DrawGizmo>().attachedObject.transform.position;
                        tempTrans.y = 0.08f;
                        transform.LookAt(tempTrans);
                        go.GetComponent<DrawGizmo>().attachedObject.GetComponent<HologramShapes>().Delocate(carryPos);
                        droppingOff = true;
                        dropOff();
                    }
                }

                if (!droppingOff)
                {
                    hasRotated = false;
                    isMoving = false;
                }

            }
        }
    }

    public void dropOff()
    {
        toGameObject = dropoffLoc;
        isRotating = true;
        isMoving = true;
    }
  
    GameObject CheckShapePlacement()
    {
        //if (GameManager.Instance.cubePlaced || GameManager.Instance.spherePlaced || GameManager.Instance.trianglePlaced) {
        List<GameObject> objs = new List<GameObject>();
            int i = 0;
            foreach (GameObject obj in shapePositions)
            {
                if (obj.GetComponent<DrawGizmo>().attachedHologramLoc.gameObject.active == false)
                {
                objs.Add(obj);
                i++;
                }
            }
        Debug.Log("i = " + i);
            return objs[Random.Range(0, i)];
        //}
    }
    void LateUpdate()
    {
        if (isMoving)
        {
            MoveToGameObject(toGameObject);
        }
    }

    IEnumerator RandomMove()
    {
        if (droppingOff)
        {
            yield return new WaitForSeconds(10f);
            StartCoroutine(RandomMove());
            yield break;
        }
        GameObject go;
        if (GameManager.Instance.cubePlaced || GameManager.Instance.spherePlaced || GameManager.Instance.trianglePlaced)
        {
            go = CheckShapePlacement();
        }
        else
        {
            go = movementPositions[Random.Range(0, movementPositions.Count)];
            while (go == toGameObject)
            {
                go = movementPositions[Random.Range(0, movementPositions.Count)];
            }
        }

        toGameObject = go;
        isRotating = true;
        isMoving = true;
        yield return new WaitForSeconds(10f);
        StartCoroutine(RandomMove());
    }
}