using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CleanBezierCurve : MonoBehaviour
{
    Mesh mesh;

    [SerializeField]
    int curveDetail = 15;

    [Tooltip("Pleas use even number")]
    [SerializeField]
    int curveCount = 14;

    [SerializeField]
    float boundsX = 0;

    [SerializeField]
    float boundsZ = 0;

    [SerializeField]
    int cylinderDetail = 0;

    [SerializeField]
    float radius = 0.1f;

    [SerializeField]
    GameObject checkpointsParent = null;

    [SerializeField]
    GameObject wireEnding = null;

    [SerializeField]
    float yStep = 0.0f; //the amount every point will be offset on the y-asix

    [SerializeField]
    float generateSpeed = 0;

    public Vector3[] ringDir;
    public float zOffsetPp = 0;

    //TEMP
    List<Vector3> perpVectors = new List<Vector3>();

    List<Vector3> orthogonal = new List<Vector3>();

    List<Vector3> sloep = new List<Vector3>();

    List<Vector3> gizzies = new List<Vector3>();

    Vector3 LastPoint = new Vector3(0, 0, 0);
    ///

    List<Vector3> curvePoints = new List<Vector3>();
    List<int> triangles = new List<int>();

    bool isFirstCurve = true;
    int wireIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        curveCount -= curveCount % 2;
        CalculateWire();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(RemoveWire());
        }
    }

    public IEnumerator RemoveWire()
    {
        if (triangles.Count == 0)
        {
            CalculateWire();
            yield break;
        }
        triangles.RemoveRange(triangles.Count - (cylinderDetail) * 6, (cylinderDetail) * 6);
        mesh.triangles = triangles.ToArray();

        yield return new WaitForSeconds(0.01f);
        StartCoroutine(RemoveWire());
    }

    void CalculateWire()
    {
        Reset();
        InitializeMesh();
        CalculateCurve();
        CreateCircle();
        int triangleIndex = 0;
        StartCoroutine(WaitDrawTriangle(triangleIndex));
        PlaceCheckPoints();
        PlaceWireEnding();
        if(wireIndex == 0)
        {
            curveCount = 8;
            yStep = 0.085f;
        }
        else if(wireIndex == 1)
        {
            curveCount = 14;
            yStep = 0.045f;
        }
        wireIndex++;
    }

    void PlaceWireEnding()
    {
        wireEnding.transform.localPosition = new Vector3(0.0f, 0.1f, zOffsetPp);
    }

    void Reset()
    {
        if (isFirstCurve) return;
        if (mesh.vertices.Length != 0) Array.Clear(mesh.vertices, 0, mesh.vertices.Length);
        if (mesh.normals.Length != 0) Array.Clear(mesh.normals, 0, mesh.vertices.Length);
        if (mesh.triangles.Length != 0) Array.Clear(mesh.triangles, 0, mesh.vertices.Length);

        perpVectors.Clear();
        orthogonal.Clear();
        sloep.Clear();
        gizzies.Clear();
        triangles.Clear();
        curvePoints.Clear();
    }

    void InitializeMesh()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        mesh = new Mesh();
        mf.mesh = mesh;
    }

    void PlaceCheckPoints()
    {
        ringDir = new Vector3[checkpointsParent.transform.childCount];
        int wireDivisions = checkpointsParent.transform.childCount + 1;

        for (int i = 0; i < checkpointsParent.transform.childCount; i++)
        {
            checkpointsParent.transform.GetChild(i).transform.localPosition = curvePoints[(curvePoints.Count / wireDivisions) * (i + 1)] * this.transform.localScale.y + this.transform.position;
            ringDir[i] = curvePoints[(curvePoints.Count / wireDivisions) * (i + 1) + 1] * this.transform.localScale.y + this.transform.position;

            Vector3 relativePos = ringDir[i] - checkpointsParent.transform.GetChild(i).position;

            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            checkpointsParent.transform.GetChild(0).rotation = rotation;
        }
    }

    IEnumerator WaitDrawTriangle(int i)
    {
        if ((i == curveDetail - 3 && isFirstCurve) || (i == (mesh.vertices.Length / cylinderDetail) - curveDetail + 2 && !isFirstCurve))
        {
            GetComponent<MeshCollider>().sharedMesh = mesh;
            isFirstCurve = false;
            Debug.Log("Done generating triangles");

            yield break;
        }
        Debug.Log(cylinderDetail);

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


        yield return new WaitForSeconds(0.01f);

        i++;
        StartCoroutine(WaitDrawTriangle(i));
    }

    void CreateCircle()
    {
        List<Vector3> verts = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();

        for (int j = 0; j < curvePoints.Count - 1; j++)
        {
            Vector3 firstPerp;

            if (isFirstCurve)
            {
                firstPerp = curvePoints[j];
                firstPerp.x += 0.2f;
            }
            else if (j < curveDetail)
            {
                Vector3 p = curvePoints[j] + new Vector3(0, 0, 10);
                firstPerp = curvePoints[j] - (p - curvePoints[j]).normalized;
            }
            else
            {
                firstPerp = Vector3.Cross((curvePoints[j] - curvePoints[j + 1]).normalized, Vector3.up.normalized).normalized;
                firstPerp = curvePoints[j] + (firstPerp * 0.3f);
                firstPerp.y = curvePoints[j].y;
            }

            perpVectors.Add(firstPerp);

            Vector3 secondPerp = (Vector3.Cross(curvePoints[j + 1] - curvePoints[j], firstPerp - curvePoints[j])).normalized; //Get vector orthogonal to fp and the next point on the curve          
            secondPerp = curvePoints[j] + (secondPerp * 0.2f);

            orthogonal.Add(secondPerp);

            Vector3 center = curvePoints[j];

            const float maxt = 2 * Mathf.PI;
            float stept = maxt / cylinderDetail;

            List<Vector3> checkVerts = new List<Vector3>();
            List<Vector3> checkNormals = new List<Vector3>();


            for (float i = 0; i < maxt; i += stept)
            {
                Vector3 p = center + radius * Mathf.Cos(i) * (center - secondPerp).normalized + radius * Mathf.Sin(i) * (center - firstPerp).normalized;
                Vector3 normal = Mathf.Cos(i) * (center - secondPerp).normalized + Mathf.Sin(i) * (center - firstPerp).normalized;

                verts.Add(p);
                checkVerts.Add(p);
                checkNormals.Add(normal);
            }

            if (j != 0)
            {
                //Check which vertex is closest by
                float dist = 0;
                float shortestDist = 0;
                int checkIndex = cylinderDetail * (j - 1);
                int index = 0;
                int shortestDistIndex = 0;

                foreach (Vector3 v in checkVerts)
                {
                    if (!(verts.Count < checkIndex))
                    {
                        if (j == 1) dist = (v - verts[checkIndex]).magnitude;
                        else dist = (v - gizzies[j - 2]).magnitude;

                        if (index == 0)
                        {
                            shortestDist = dist;
                        }
                        if (dist < shortestDist)
                        {
                            shortestDist = dist;
                            shortestDistIndex = index;
                        }
                        index++;
                    }
                }
                gizzies.Add(checkVerts[shortestDistIndex]);

                //Reorder vertices
                index = 0;
                foreach (Vector3 v in checkVerts)
                {
                    if (shortestDistIndex == cylinderDetail) shortestDistIndex = 0;
                    sloep.Add(checkVerts[shortestDistIndex]);
                    normals.Add(checkNormals[shortestDistIndex]);
                    shortestDistIndex++;
                }
            }

            mesh.vertices = sloep.ToArray();
            mesh.normals = normals.ToArray();
        }
    }

    void CalculateCurve()
    {
        float tStep = 1.0f / (curveDetail - 1); //the amount that the position on the curve will go up each iteration
        float y = 0;
        int zDir = -1;
        float yOffset = 1;

        Vector2 ContrPtMinMaxOffset = new Vector2(1.0f, 1.5f);

        Vector3 FirstPoint = new Vector3(0, 0, 0); ;
        Vector3 SecondPoint = new Vector3(0, 0, 0);
        Vector3 FirstControlPoint = new Vector3(0, 0, 0); ;
        Vector3 SecondControlPoint = new Vector3(0, 0, 0);
        Vector3 lastPointDir = new Vector3(0, 0, 0);

        bool wireMiddle = false;

        Vector3 curvePoint;

        if (isFirstCurve)
        {
            float t = 0;

            for (int i = 0; i < curveDetail; i++)
            {
                float height = 10.0f;
                float offset = 2.0f;
                curvePoint = Mathf.Pow((1 - t), 3) * new Vector3(0.0f, 0, -offset) + 3 * Mathf.Pow((1 - t), 2) * t * new Vector3(0.0f, height, -offset)
                + 3 * (1 - t) * Mathf.Pow(t, 2) * new Vector3(0.0f, height, offset) + Mathf.Pow(t, 3) * new Vector3(0.0f, 0, offset);
                t += tStep;
                curvePoints.Add(curvePoint);
            }
            return;
        }

        for (int j = 0; j < curveCount; j++)
        {
            Vector3 ControlPointOffset = new Vector3(RandomTwoRanges(-ContrPtMinMaxOffset.x, -ContrPtMinMaxOffset.y, ContrPtMinMaxOffset.x, ContrPtMinMaxOffset.y),
                                          0, RandomTwoRanges(-ContrPtMinMaxOffset.x, -ContrPtMinMaxOffset.y, ContrPtMinMaxOffset.x, ContrPtMinMaxOffset.y));

            if (j == 0) //The points of the first curve of the wire
            {
                FirstPoint = new Vector3(0, 0, zDir * zOffsetPp);
                SecondPoint = new Vector3(0, 0, zDir * zOffsetPp);

                FirstControlPoint = new Vector3(0, 0, zDir * zOffsetPp);

                ControlPointOffset = new Vector3(RandomTwoRanges(-ContrPtMinMaxOffset.x, -ContrPtMinMaxOffset.y, ContrPtMinMaxOffset.x, ContrPtMinMaxOffset.y),
                    0, RandomTwoRanges(-ContrPtMinMaxOffset.x, -ContrPtMinMaxOffset.y, ContrPtMinMaxOffset.x, ContrPtMinMaxOffset.y)); //Create new Random Value

                SecondControlPoint = new Vector3(0, 0, zDir * zOffsetPp);
            }
            else if (j >= curveCount - 3)
            {
                float dist = (SecondPoint - SecondControlPoint).magnitude; //returns length of the difference between the two Vectors
                Vector3 dir = (SecondPoint - SecondControlPoint).normalized;
                FirstControlPoint = SecondPoint + dir * dist;
                FirstPoint = SecondPoint; //Set the coordinates of first point of the new curve equal to the second point of the previous curve

                SecondPoint = FirstPoint;
                SecondPoint.z += 0.005f;
                SecondControlPoint = SecondPoint;
                SecondControlPoint.z -= 0.0045f;

            }
            else
            {
                //Create a controlpoint exactly on the oppsite side of the second point
                float dist = (SecondPoint - SecondControlPoint).magnitude; //returns length of the difference between the two Vectors
                Vector3 dir = (SecondPoint - SecondControlPoint).normalized;
                FirstControlPoint = SecondPoint + dir * dist;
                FirstPoint = SecondPoint; //Set the coordinates of first point of the new curve equal to the second point of the previous curve

                if (j == curveCount - 4) SecondPoint = new Vector3(0, 0, zDir * zOffsetPp);
                else SecondPoint = new Vector3(UnityEngine.Random.Range(0, -(boundsX + 1)), 0, UnityEngine.Random.Range(zDir * zOffsetPp, zDir * (boundsZ + 1)));

                ControlPointOffset = new Vector3(RandomTwoRanges(-ContrPtMinMaxOffset.x, -ContrPtMinMaxOffset.y, ContrPtMinMaxOffset.x, ContrPtMinMaxOffset.y),
                    0, RandomTwoRanges(-ContrPtMinMaxOffset.x, -ContrPtMinMaxOffset.y, ContrPtMinMaxOffset.x, ContrPtMinMaxOffset.y)); //Create new Random Value

                SecondControlPoint = SecondPoint + ControlPointOffset;
            }

            float lastY = y;

            if (j == curveCount / 2 - 2)
            {
                zDir *= -1;
            }
            else if (j == curveCount / 2 - 1)
            {
                wireMiddle = true;
            }
            else if (j == curveCount / 2)
            {
                yOffset = -1;
                wireMiddle = false;
            }

            float t = 0;

            for (int i = 0; i < curveDetail; i++)
            {
                if (i == 0 && j != 0)
                {
                    continue;
                }


                if (j == 0)
                {
                    curvePoint = new Vector3(0, 0, zDir * zOffsetPp);
                }

                else
                {
                    curvePoint = Mathf.Pow((1 - t), 3) * FirstPoint + 3 * Mathf.Pow((1 - t), 2) * t * FirstControlPoint
                         + 3 * (1 - t) * Mathf.Pow(t, 2) * SecondControlPoint + Mathf.Pow(t, 3) * SecondPoint;
                }

                t += tStep;

                if (!wireMiddle)
                {
                    y += (yStep * yOffset);
                }
                else
                {
                    float v = -.8f * (Mathf.Sin((((Mathf.PI) / (curveDetail - 1)) * i) / -1.0f)) + lastY;
                    y = v;

                }
                curvePoint.y = y;
                curvePoints.Add(curvePoint);
            }
        }
    }

    //this function returns a random value between two ranges
    float RandomTwoRanges(float FirstMin, float FirstMax, float SecondMin, float SecondMax)
    {
        int chooseRange = UnityEngine.Random.Range(0, 2); //returns 0 or 1, determines which range will be chosen

        if (chooseRange == 0)
            return UnityEngine.Random.Range(FirstMin, FirstMax);
        else
            return UnityEngine.Random.Range(SecondMin, SecondMax);
    }
}
