using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movementSpeed = 10;
    public float turningSpeed = 60;

    public GameObject rotateTowards;

    public List<GameObject> movementPositions;

    private Vector3 oldRota = Vector3.zero;
    public bool isMoving;
    public bool isRotating;
    public bool hasRotated;
    private GameObject toGameObject;
    private Animator animator;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
        StartCoroutine(RandomMove());
    }
    void Update()
    {
        oldRota = transform.localEulerAngles;
        HandleInput();
    }

    void MoveToGameObject(GameObject go)
    {
        if (isRotating)
        {
            rotateTowards.transform.LookAt(go.transform);
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
                if (go.GetComponent<DrawGizmo>().isButtonPosition)
                {
                    animator.SetTrigger("PressButton");
                    if (go.GetComponent<DrawGizmo>().attachedObject != null)
                    {
                        transform.LookAt(go.GetComponent<DrawGizmo>().attachedObject.transform);
                    }
                }
                hasRotated = false;
                isMoving = false;
            }
        }
    }
  
    void LateUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal") * turningSpeed * Time.deltaTime;
        transform.Rotate(0, horizontal, 0);

        float vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        transform.Translate(0, 0, vertical);

        if (isMoving)
        {
            MoveToGameObject(toGameObject);
        }

    }

    IEnumerator RandomMove()
    {
        GameObject go = movementPositions[Random.Range(0, movementPositions.Count)];
        while (go == toGameObject)
        {
            go = movementPositions[Random.Range(0, movementPositions.Count)];
        }
        toGameObject = go;
        isRotating = true;
        isMoving = true;
        yield return new WaitForSeconds(Random.Range(10, 20));
        StartCoroutine(RandomMove());
    }
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.G) && !isMoving)
        {
            GameObject go = movementPositions[Random.Range(0, movementPositions.Count)];
            while (go == toGameObject)
            {
                go = movementPositions[Random.Range(0, movementPositions.Count)];
            }
            toGameObject = go;
            isRotating = true;
            isMoving = true;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isIdle", false);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isIdle", true);
            Debug.Log(rotateTowards.transform.rotation);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetBool("isTurningLeft", true);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            animator.SetBool("isTurningLeft", false);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            animator.SetBool("isTurningRight", true);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {

            animator.SetBool("isSwiping", true);
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            animator.SetBool("isSwiping", false);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            animator.SetBool("isTurningRight", false);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            animator.SetBool("isPressing", true);
            animator.SetBool("isIdle", false);
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            animator.SetBool("isClapping", true);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Rotation = " + rotateTowards.transform.rotation);
            Debug.Log("Local Rotation = " + rotateTowards.transform.localRotation);
            Debug.Log("Euler angler = " + rotateTowards.transform.eulerAngles);
            Debug.Log("Local euelr = " + rotateTowards.transform.localEulerAngles);
        }
    }
}