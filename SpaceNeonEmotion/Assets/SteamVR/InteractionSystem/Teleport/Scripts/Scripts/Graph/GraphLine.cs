using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphLine : MonoBehaviour
{
    [SerializeField]
    float drawSpeed;

    Graph graph;

    bool firstSpawned;
    bool stopDrawing;

    void Start()
    {
        graph = this.GetComponentInParent<Graph>();
    }

    void FixedUpdate()
    {
        if (!stopDrawing) this.transform.localScale += new Vector3(0, drawSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GraphPoint"))
        {
            if (!firstSpawned)
            {
                firstSpawned = true;
                return;
            }

            stopDrawing = true;
            graph.lineCount++;

            if (graph.lineCount < graph.graphPointCount - 1)
            {
                graph.DrawLines(graph.lineCount);
            }
            else
            {
                Debug.Log("Done drawing!");
                //call emoji thing spawn function
                graph.PlaceSurveyResults();
            }
        }
    }
}
