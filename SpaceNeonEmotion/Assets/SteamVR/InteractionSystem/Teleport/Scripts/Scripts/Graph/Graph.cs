﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField]
    GameObject graphPoint;

    [SerializeField]
    GameObject cylinderPrefab;

    public int graphPointCount;

    [SerializeField]
    float drawSpeed;

    [SerializeField]
    GameObject gameManagerObjects = null;

    [SerializeField]
    GameObject baseLine = null;

    [SerializeField]
    float saveHeartRateTime = 0.0f;

    [SerializeField]
    GameObject[] surveyButtons = null;

    [SerializeField]
    SurveyManager surveyManager = null;

    GameManager gameManager;
    private List<int> heartrateValues = new List<int>();

    private GameObject[] graphPoints;
    private Vector3[] graphPointPositions;

    GameObject cylinderModel;

    public int lineCount = 0;

    float timeElapsed = 0.0f;

    void Start()
    {
        gameManager = gameManagerObjects.GetComponent<GameManager>();
        StartCoroutine(ReadHeartRate());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            timeElapsed = Time.time;

            StopAllCoroutines();

            baseLine.SetActive(true);
            graphPoints = new GameObject[heartrateValues.Count];
            graphPointPositions = new Vector3[heartrateValues.Count];
            graphPointCount = heartrateValues.Count;

            int index = 0;
            StartCoroutine(DrawGraphPoints(index));
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("TIME: " + Time.time);
        }
    }

    IEnumerator ReadHeartRate()
    {
        yield return new WaitForSeconds(saveHeartRateTime);

        heartrateValues.Add(gameManager.heartrate);
        StartCoroutine(ReadHeartRate());
    }

    IEnumerator DrawGraphPoints(int index)
    {
        if (index == heartrateValues.Count)
        {
            DrawLines(0);
            yield break;
        }

        Debug.Log("theta: " + (Mathf.Sin((Mathf.PI / graphPointCount) * index)));
        graphPoints[index] = Instantiate(graphPoint, transform);
        float radius = 5.0f;
        float xPos = this.transform.position.x - radius * Mathf.Sin((Mathf.PI / graphPointCount) * index);
        float zPos = this.transform.position.z - radius * Mathf.Cos((Mathf.PI / graphPointCount) * index);

        float offsetY = heartrateValues[index] * 0.02f;

        graphPoints[index].transform.localPosition = new Vector3(xPos, offsetY, zPos);
        graphPointPositions[index] = graphPoints[index].transform.position;

        yield return new WaitForSeconds(drawSpeed);

        index++;
        StartCoroutine(DrawGraphPoints(index));
    }

    public void DrawLines(int index)
    {
        cylinderModel = Instantiate(cylinderPrefab, this.transform);

        cylinderModel.transform.position = graphPoints[index].transform.position;
        Vector3 dir = graphPoints[index + 1].transform.position - graphPoints[index].transform.position;
        Quaternion rot = Quaternion.LookRotation(dir);
        cylinderModel.transform.rotation = rot;

        Vector3 temp = cylinderModel.transform.rotation.eulerAngles;
        temp.x += 90.0f;
        cylinderModel.transform.rotation = Quaternion.Euler(temp);
    }

    public void PlaceSurveyResults()
    {
        if (surveyManager.surveyData.Count != 0)
        {
            for (int i = 0; i < surveyManager.surveyData.Count; i++)
            {
                Debug.Log(timeElapsed);
                float radius = 5.0f;
                float xPos = this.transform.position.x - radius * Mathf.Sin((Mathf.PI / timeElapsed) * surveyManager.surveyData[i].Item2);
                float zPos = this.transform.position.z - radius * Mathf.Cos((Mathf.PI / timeElapsed) * surveyManager.surveyData[i].Item2);

                GameObject sb = Instantiate(surveyButtons[surveyManager.surveyData[i].Item1], transform);

                sb.transform.localPosition = new Vector3(xPos, this.transform.position.y, zPos);

            }
        }
    }
}
