using System.Collections;
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

    [SerializeField]
    Transform rotateTowardsPoint = null;

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

    public void DrawGraph()
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

    IEnumerator ReadHeartRate()
    {
        yield return new WaitForSeconds(saveHeartRateTime);
        //heartrateValues.Add(Random.Range(60, 160));
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

        graphPoints[index] = Instantiate(graphPoint, transform);
        float radius = 5.0f;
        float xPos = this.transform.position.x - radius * Mathf.Sin((Mathf.PI / (graphPointCount - 1.0f)) * index);
        float zPos = this.transform.position.z - radius * Mathf.Cos((Mathf.PI / (graphPointCount - 1.0f)) * index);

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
                Debug.Log("Surveydata index: " + surveyManager.surveyData.Count);
                float radius = 5.0f;
                float xPos = this.transform.position.x - radius * Mathf.Sin((Mathf.PI / timeElapsed) * surveyManager.surveyData[i].Item2);
                float zPos = this.transform.position.z - radius * Mathf.Cos((Mathf.PI / timeElapsed) * surveyManager.surveyData[i].Item2);

                GameObject sb = Instantiate(surveyButtons[surveyManager.surveyData[i].Item1], transform);

                sb.transform.localPosition = new Vector3(xPos, this.transform.position.y, zPos);
                sb.transform.localPosition += new Vector3(0.0f, 5.0f, 0.0f);

                Quaternion newRotation = Quaternion.LookRotation(rotateTowardsPoint.position - sb.transform.position, Vector3.up);
                sb.transform.rotation = newRotation;

            }
        }
    }
}
