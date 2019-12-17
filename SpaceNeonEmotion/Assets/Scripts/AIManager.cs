using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    private static AIManager _instance;
    public static AIManager Instance { get { return _instance; } }

    [SerializeField] private GameObject movPosParent;
    [SerializeField] private GameObject shapePosParent;
    [SerializeField] private GameObject dropoffLocation;
    [SerializeField] private GameObject robot;
    public List<GameObject> movementPositions = new List<GameObject>();
    public List<GameObject> shapePositions = new List<GameObject>();
    public float WaitTimeBetweenMove;

    private AIBehaviour aiBehaviour;

    public float movementSpeed = 1f;
    public float turningSpeed = 60f;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        foreach (Transform child in movPosParent.transform)
        {
                movementPositions.Add(child.gameObject);

        }
        foreach (Transform child in shapePosParent.transform)
        {
            shapePositions.Add(child.gameObject);
        }
        aiBehaviour = robot.GetComponent<AIBehaviour>();
    }

    private void Start()
    {
        StartCoroutine(MoveRobotToRandom(0f));
    }

    public IEnumerator MoveRobotToRandom(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        GameObject go = movementPositions[UnityEngine.Random.Range(0, shapePositions.Count)];
        while (go.transform.position == robot.transform.position)
        {
            go = movementPositions[UnityEngine.Random.Range(0, shapePositions.Count)];
        }
        aiBehaviour.GoToPosition(go);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            aiBehaviour.walkingBehaviour.StopMoving();
            MoveRobot(WaitTimeBetweenMove);
        }
    }

    public void MoveRobot(float timeToWait)
    {
        if (GameManager.Instance.isShapePlaced())
        {
            StartCoroutine(MoveRobotToShape(timeToWait));
        }
        else
        {
            StartCoroutine(MoveRobotToRandom(timeToWait));
        }
    }
    public IEnumerator MoveRobotToDropoff(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        aiBehaviour.GoToPosition(dropoffLocation);
    }

    public IEnumerator MoveRobotToShape(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        aiBehaviour.GoToPosition(GetRandomPlacedShape());
    }

    public GameObject GetRandomPlacedShape()
    {
        List<GameObject> placedShapes = new List<GameObject>();
        foreach (GameObject obj in shapePositions)
        {
            if (obj.GetComponent<DrawGizmo>().attachedHologramLoc.activeInHierarchy == false)
            {
                placedShapes.Add(obj);
            }
        }
        if (placedShapes.Count != 0)
        {
            return placedShapes[UnityEngine.Random.Range(0, placedShapes.Count)];
        }
        return null;
    }
}
