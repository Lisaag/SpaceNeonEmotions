using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireMeshGeneration : MonoBehaviour
{
    [SerializeField]
    float boundsX = 0;

    [SerializeField]
    float boundsZ = 0;

    [SerializeField]
    int cylinderDetail;

    Mesh mesh;

    const int curveCount = 15; //Amount of curves generated
    const int curveDetail = 15; //Amount of points on the curve calculated
    const int curvePointCount = curveCount * curveDetail;
    Vector3[] curvePoints = new Vector3[curvePointCount];

    List<Vector3> orthogonal = new List<Vector3>();
    List<Vector3> perpVectors = new List<Vector3>();
    List<Vector3> verts = new List<Vector3>();
    List<int> triangles = new List<int>();

    void Start()
    {
        InitializeMesh();
        CaluclateCurve();

        CreateCircle();
        int triangleIndex = 0;
        StartCoroutine(WaitDrawTriangle(triangleIndex));
    }

    void InitializeMesh()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        mesh = new Mesh();
        mf.mesh = mesh;
    }

    void CaluclateCurve()
    {
        Vector3[] FirstPoint = new Vector3[curveCount];
        Vector3[] SecondPoint = new Vector3[curveCount];

        Vector3[] FirstControlPoint = new Vector3[curveCount];
        Vector3[] SecondControlPoint = new Vector3[curveCount];

        float tStep = 1.0f / (curveDetail - 1); //the amount that the position on the curve will go up each iteration
        float y = 0;
        float yStep = 0.05f; //the amount every point will be offset on the y-asix

        Vector2 ContrPtMinMaxOffset = new Vector2(1.0f, 2.0f);

        for (int j = 0; j < curveCount; j++)
        {
            Vector3 ControlPointOffset = new Vector3(RandomTwoRanges(-ContrPtMinMaxOffset.x, -ContrPtMinMaxOffset.y, ContrPtMinMaxOffset.x, ContrPtMinMaxOffset.y),
                                            0, RandomTwoRanges(-ContrPtMinMaxOffset.x, -ContrPtMinMaxOffset.y, ContrPtMinMaxOffset.x, ContrPtMinMaxOffset.y));

            if (j == 0) //The points of the first curve of the wire
            {
                FirstPoint[j] = new Vector3(Random.Range(0, boundsX + 1), 0, Random.Range(0, boundsZ + 1));
                SecondPoint[j] = new Vector3(0, 0, 3);

                FirstControlPoint[j] = FirstPoint[j] + ControlPointOffset;

                ControlPointOffset = new Vector3(RandomTwoRanges(-ContrPtMinMaxOffset.x, -ContrPtMinMaxOffset.y, ContrPtMinMaxOffset.x, ContrPtMinMaxOffset.y),
                    0, RandomTwoRanges(-ContrPtMinMaxOffset.x, -ContrPtMinMaxOffset.y, ContrPtMinMaxOffset.x, ContrPtMinMaxOffset.y)); //Create new Random Value

                SecondControlPoint[j] = SecondPoint[j] + ControlPointOffset;
            }
            else
            {
                //Create a controlpoint exactly on the oppsite side of the second point
                float dist = (SecondPoint[j - 1] - SecondControlPoint[j - 1]).magnitude; //returns length of the difference between the two Vectors
                Vector3 dir = (SecondPoint[j - 1] - SecondControlPoint[j - 1]).normalized;
                FirstControlPoint[j] = SecondPoint[j - 1] + dir * dist;
                FirstPoint[j] = SecondPoint[j - 1]; //Set the coordinates of first point of the new curve equal to the second point of the previous curve

                SecondPoint[j] = new Vector3(Random.Range(0, boundsX + 1), 0, Random.Range(0, boundsZ + 1));

                ControlPointOffset = new Vector3(RandomTwoRanges(-ContrPtMinMaxOffset.x, -ContrPtMinMaxOffset.y, ContrPtMinMaxOffset.x, ContrPtMinMaxOffset.y),
                    0, RandomTwoRanges(-ContrPtMinMaxOffset.x, -ContrPtMinMaxOffset.y, ContrPtMinMaxOffset.x, ContrPtMinMaxOffset.y)); //Create new Random Value

                SecondControlPoint[j] = SecondPoint[j] + ControlPointOffset;
            }

            float t = 0;

            for (int i = 0; i < curveDetail; i++)
            {
                if (i != 0)
                {
                    y += yStep;
                }

                Vector3 curvePoint = Mathf.Pow((1 - t), 3) * FirstPoint[j] + 3 * Mathf.Pow((1 - t), 2) * t * FirstControlPoint[j]
                    + 3 * (1 - t) * Mathf.Pow(t, 2) * SecondControlPoint[j] + Mathf.Pow(t, 3) * SecondPoint[j];

                t += tStep;
                curvePoint.y = y;
                curvePoints[curveDetail * j + i] = curvePoint;
            }
        }
    }

    void CreateCircle()
    {
        for (int j = 0; j < curvePointCount - 1; j++)
        {
            if (j % curveDetail != curveDetail - 1 || j == 0)
            {
                Vector3 baseLine = curvePoints[j + 1] - curvePoints[j]; //first vector of orthogonal basis
                //inversing the baseline vector, making one negative (x=-z, z=x), and dividing it by the baseline's magnitude
                Vector2 perp2D = new Vector2(-baseLine.z, baseLine.x) / Mathf.Sqrt(Mathf.Pow(baseLine.x, 2) + Mathf.Pow(baseLine.z, 2));
                Vector3 firstPerp = new Vector3(perp2D.x, curvePoints[j].y, perp2D.y).normalized; //Direction of the perpendicular vector
                firstPerp = curvePoints[j] + (firstPerp * 0.2f); //offsetting the perpendicular vector from circle origin
                firstPerp.y = curvePoints[j].y; //resetting the y axis, bc this is a 2d perpendicular vector

                Vector3 secondPerp = Vector3.Cross(curvePoints[j + 1] - curvePoints[j], firstPerp - curvePoints[j]).normalized; //Get vector orthogonal to fp and the next point on the curve
                secondPerp = curvePoints[j] + (secondPerp / 8);

                perpVectors.Add(firstPerp);
                orthogonal.Add(secondPerp);

                Vector3 center = curvePoints[j];
                float radius = .1f;

                const float maxt = 2 * Mathf.PI;
                float stept = maxt / cylinderDetail;

                for (float i = 0; i < maxt; i += stept)
                {
                    Vector3 p = center + radius * Mathf.Cos(i) * (center - secondPerp).normalized + radius * Mathf.Sin(i) * (center - firstPerp).normalized;

                    verts.Add(p);
                }
            }
        }
        mesh.vertices = verts.ToArray();
    }

    IEnumerator WaitDrawTriangle(int i)
    {
        if (i == curvePointCount - cylinderDetail - 1)
        {
            GetComponent<MeshCollider>().sharedMesh = mesh;

            yield break;
        }

        for (int j = 0; j < cylinderDetail; j++)
        {
            triangles.Add(0 + j + (i * cylinderDetail));
            triangles.Add(cylinderDetail + j + (i * cylinderDetail));
            triangles.Add(1 + j + (i * cylinderDetail));


            if (j == cylinderDetail - 1)
            {
                triangles.Add(0 + j + (i * cylinderDetail));
                triangles.Add(1 + j + (i * cylinderDetail));
                triangles.Add(0 + (i * cylinderDetail));
            }

            else
            {
                triangles.Add(1 + j + (i * cylinderDetail));
                triangles.Add(cylinderDetail + j + (i * cylinderDetail));
                triangles.Add(cylinderDetail + j + 1 + (i * cylinderDetail));
            }
        }

        mesh.triangles = triangles.ToArray();


        yield return new WaitForSeconds(0.1f);

        i++;
        StartCoroutine(WaitDrawTriangle(i));
    }

    //this function returns a random value between two ranges
    float RandomTwoRanges(float FirstMin, float FirstMax, float SecondMin, float SecondMax)
    {
        int chooseRange = Random.Range(0, 2); //returns 0 or 1, determines which range will be chosen

        if (chooseRange == 0)
            return Random.Range(FirstMin, FirstMax);
        else
            return Random.Range(SecondMin, SecondMax);
    }

    /* void OnDrawGizmos()
     {
         if(curvePoints.Length != 0)
         {
             foreach (Vector3 p in curvePoints)
             {
                 Gizmos.DrawSphere(p, 0.05f);
             }
         }

         if(verts.Count != 0)
         {
             Gizmos.color = Color.cyan;
             foreach(Vector3 v in verts)
             {
                 Gizmos.DrawSphere(v, 0.025f);
             }
         }
     }*/
}
