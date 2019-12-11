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

    private AIBehaviour aiBehaviour;
    //private Animator

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
        StartCoroutine(GoToRandomMovePosition());
    }

    IEnumerator GoToRandomMovePosition()
    {
        GameObject go = movementPositions[UnityEngine.Random.Range(0, shapePositions.Count)];
        while (go.transform.position == transform.position)
        {
            go = movementPositions[UnityEngine.Random.Range(0, shapePositions.Count)];
        }
        aiBehaviour.GoToPosition(go);
        yield return new WaitForSeconds(10f);
        StartCoroutine(GoToRandomMovePosition());
    }
}
