using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLine : MonoBehaviour
{
    [SerializeField]
    MeshFilter mf;

    private const int lineDetail = 30;

    [SerializeField]
    private float lineHeight;

    [SerializeField]
    private float lineThickness;

    List<Vector3> linePoints = new List<Vector3>();
    Vector3[] vertices = new Vector3[lineDetail * 2];
    private Mesh mesh;

    private void Start()
    {
        Debug.Log("vertices size: " + vertices.Length);
        mesh = GetComponent<MeshFilter>().mesh;
        CalculateLinePoints();
        DrawTriangles();
    }

    void CalculateLinePoints()
    {
        for (int i = 0; i < lineDetail; i++)
        {
            Debug.Log(i + lineDetail);
            float radius = 5.0f;
            float xPos = this.transform.position.x - radius * Mathf.Sin((Mathf.PI / lineDetail) * i);
            float zPos = this.transform.position.z - radius * Mathf.Cos((Mathf.PI / lineDetail) * i);
            linePoints.Add(new Vector3(xPos, lineHeight, zPos));
            vertices[i] = new Vector3(xPos, lineHeight, zPos);
            vertices[i + lineDetail] = new Vector3(xPos, lineHeight - lineThickness, zPos);
        }

        mesh.vertices = vertices;
    }

    void DrawTriangles()
    {
        List<int> triangles = new List<int>();

        for (int i = 0; i < lineDetail - 1; i++)
        {
            triangles.Add(i);
            triangles.Add(lineDetail + i + 1);
            triangles.Add(lineDetail + i);

            triangles.Add(i);
            triangles.Add(i + 1);
            triangles.Add(lineDetail + i + 1);
        }

        for (int i = 0; i < lineDetail - 1; i++)
        {
            triangles.Add(i);
            triangles.Add(lineDetail + i);
            triangles.Add(lineDetail + i + 1);

            triangles.Add(i);
            triangles.Add(lineDetail + i + 1);
            triangles.Add(i + 1);
        }

        mesh.triangles = triangles.ToArray();
    }
}
