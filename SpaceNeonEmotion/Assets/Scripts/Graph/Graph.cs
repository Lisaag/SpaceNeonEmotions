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

    private GameObject[] graphPoints;
    private Vector3[] graphPointPositions;

    GameObject cylinderModel;

    public int lineCount = 0;

    void Start()
    {
        graphPoints = new GameObject[graphPointCount];
        graphPointPositions = new Vector3[graphPointCount];

        int index = 0;
        StartCoroutine(DrawGraphPoints(index));
    }

    IEnumerator DrawGraphPoints(int index)
    {
        if (index == graphPointCount)
        {
            DrawLines(0);
            yield break;
        }

        graphPoints[index] = Instantiate(graphPoint, transform);
        float radius = 5.0f;
        float xPos = this.transform.position.x - radius * Mathf.Sin((Mathf.PI / graphPointCount) * index);
        float zPos = this.transform.position.z - radius * Mathf.Cos((Mathf.PI / graphPointCount) * index);

        float offsetY = Random.Range(0, 6.0f);

        graphPoints[index].transform.localPosition = new Vector3(xPos, offsetY, zPos);
        graphPointPositions[index] = graphPoints[index].transform.position;

        yield return new WaitForSeconds(drawSpeed);

        index++;
        StartCoroutine(DrawGraphPoints(index));
    }

    public void DrawLines(int index)
    {
        cylinderModel = Instantiate(cylinderPrefab, transform);

        cylinderModel.transform.position = graphPoints[index].transform.localPosition;
        Debug.Log("Moving cylinder to: " + cylinderModel.transform.localPosition);
        Vector3 dir = graphPoints[index + 1].transform.position - graphPoints[index].transform.position;
        Quaternion rot = Quaternion.LookRotation(dir);
        cylinderModel.transform.rotation = rot;

        Vector3 temp = cylinderModel.transform.rotation.eulerAngles;
        temp.x += 90.0f;
        cylinderModel.transform.rotation = Quaternion.Euler(temp);
    }
}
