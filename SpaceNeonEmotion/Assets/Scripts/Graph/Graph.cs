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
    GameObject gameManagerObjects;

    GameManager gameManager;
    private List<int> heartrateValues = new List<int>();

    private GameObject[] graphPoints;
    private Vector3[] graphPointPositions;

    GameObject cylinderModel;

    public int lineCount = 0;

    void Start()
    {
        graphPoints = new GameObject[graphPointCount];
        graphPointPositions = new Vector3[graphPointCount];

        gameManager = gameManagerObjects.GetComponent<GameManager>();

        StartCoroutine(ReadHeartRate());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            int index = 0;
            StartCoroutine(DrawGraphPoints(index));
        }
    }

    IEnumerator ReadHeartRate()
    {
        heartrateValues.Add(gameManager.heartrate);

        yield return new WaitForSeconds(5);

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
        float xPos = this.transform.position.x - radius * Mathf.Sin((Mathf.PI / graphPointCount) * index);
        float zPos = this.transform.position.z - radius * Mathf.Cos((Mathf.PI / graphPointCount) * index);

        float offsetY = heartrateValues[index] * 0.1f;

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
}
