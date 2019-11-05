using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanBezierCurve : MonoBehaviour
{
    Mesh mesh;

    [SerializeField]
    int curveDetail = 15;

    [SerializeField]
    int curveCount = 14;

    [SerializeField]
    float boundsX = 0;

    [SerializeField]
    float boundsZ = 0;

    [SerializeField]
    int cylinderDetail = 15;

    [SerializeField]
    float radius = 0.1f;

    [SerializeField]
    GameObject checkpointsParent = null;

    [SerializeField]
    float yStep = 0.0f; //the amount every point will be offset on the y-asix

    public float zOffsetPp = 0;
    public Vector3[] ringDir;

    //TEMP
    List<Vector3> perpVectors = new List<Vector3>();
    List<Vector3> orthogonal = new List<Vector3>();
    List<Vector3> tmps = new List<Vector3>();
    List<Vector3> finalPerps = new List<Vector3>();
    ///

    List<Vector3> curvePoints = new List<Vector3>();
    List<int> triangles = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        InitializeMesh();
        CalculateCurve();
        CreateCircle();
        int triangleIndex = 0;
        StartCoroutine(WaitDrawTriangle(triangleIndex));
        PlaceCheckPoints();
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

        for(int i = 0; i < checkpointsParent.transform.childCount; i++)
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
        if (i == curvePoints.Count - 2)
        {
            Debug.Log("Wire mesh collider updated");
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
            Vector3 tmp;
            Vector3 finalperp;

            if (j < curveDetail)
            {
                Vector3 p = curvePoints[j] + new Vector3(0, 0, 10);
                firstPerp = curvePoints[j] - (p - curvePoints[j]).normalized;
                tmp = new Vector3(0, 0, 0);
                finalperp = curvePoints[j] - (p - curvePoints[j]).normalized;
            }
            else
            {
                Vector3 baseLine = curvePoints[j + 1] - curvePoints[j]; //first vector of orthogonal basis
                                                                        //inversing the baseline vector, making one negative (x=-z, z=x), and dividing it by the baseline's magnitude
                Vector2 perp2D = new Vector2(-baseLine.z, baseLine.x) / Mathf.Sqrt(Mathf.Pow(baseLine.x, 2) + Mathf.Pow(baseLine.z, 2)); //;
                firstPerp = new Vector3(perp2D.x, curvePoints[j].y, perp2D.y).normalized; //Direction of the perpendicular vector
                firstPerp = curvePoints[j] + (firstPerp * 0.2f); //offsetting the perpendicular vector from circle origin
                firstPerp.y = curvePoints[j].y; //resetting the y axis, bc this is a 2d perpendicular vector

                baseLine = curvePoints[j - 1] - curvePoints[j];
                perp2D = new Vector3(-baseLine.z, baseLine.x) / Mathf.Sqrt(Mathf.Pow(baseLine.x, 2) + Mathf.Pow(baseLine.z, 2));
                tmp = new Vector3(perp2D.x, curvePoints[j].y, perp2D.y).normalized;
                tmp = curvePoints[j] + (tmp * 0.2f * -1.0f); //offsetting the perpendicular vector from circle origin
                tmp.y = curvePoints[j].y; //resetting the y axis, bc this is a 2d perpendicular vector


                finalperp = (((firstPerp + tmp) / 2.0f) - curvePoints[j]).normalized;
                finalperp = curvePoints[j] + (finalperp * 0.1f);
                finalperp.y = curvePoints[j].y;
                //  finalPerps.Add(finalperp);

                //   Debug.Log(finalperp);
            }

            tmps.Add(tmp);
            perpVectors.Add(firstPerp);
            finalPerps.Add(finalperp);

            Vector3 secondPerp = (Vector3.Cross(curvePoints[j + 1] - curvePoints[j], finalperp - curvePoints[j])).normalized; //Get vector orthogonal to fp and the next point on the curve          
            secondPerp = curvePoints[j] + (secondPerp * 0.2f);

            orthogonal.Add(secondPerp);

            Vector3 center = curvePoints[j];

            const float maxt = 2 * Mathf.PI;
            float stept = maxt / cylinderDetail;

            for (float i = 0; i < maxt; i += stept)
            {
                Vector3 p = center + radius * Mathf.Cos(i) * (center - secondPerp).normalized + radius * Mathf.Sin(i) * (center - finalperp).normalized;
                Vector3 normal = Mathf.Cos(i) * (center - secondPerp).normalized + Mathf.Sin(i) * (center - finalperp).normalized;

                verts.Add(p);
                normals.Add(normal);
            }

        }

        mesh.vertices = verts.ToArray();
        mesh.normals = normals.ToArray();
    }

    void CalculateCurve()
    {
        float tStep = 1.0f / (curveDetail - 1); //the amount that the position on the curve will go up each iteration
        float y = 0;
        int zDir = -1;

        Vector2 ContrPtMinMaxOffset = new Vector2(1.0f, 1.5f);

        Vector3 FirstPoint = new Vector3(0, 0, 0); ;
        Vector3 SecondPoint = new Vector3(0, 0, 0);
        Vector3 FirstControlPoint = new Vector3(0, 0, 0); ;
        Vector3 SecondControlPoint = new Vector3(0, 0, 0); ;

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
            else
            {
                //Create a controlpoint exactly on the oppsite side of the second point
                float dist = (SecondPoint - SecondControlPoint).magnitude; //returns length of the difference between the two Vectors
                Vector3 dir = (SecondPoint - SecondControlPoint).normalized;
                FirstControlPoint = SecondPoint + dir * dist;
                FirstPoint = SecondPoint; //Set the coordinates of first point of the new curve equal to the second point of the previous curve

                if (j == curveCount * 2 - 1)
                {
                    SecondPoint = new Vector3(0, 0, zDir * zOffsetPp);
                }
                else
                {
                    SecondPoint = new Vector3(Random.Range(0, -(boundsX + 1)), 0, Random.Range(zDir * zOffsetPp, zDir * (boundsZ + 1)));
                }

                ControlPointOffset = new Vector3(RandomTwoRanges(-ContrPtMinMaxOffset.x, -ContrPtMinMaxOffset.y, ContrPtMinMaxOffset.x, ContrPtMinMaxOffset.y),
                    0, RandomTwoRanges(-ContrPtMinMaxOffset.x, -ContrPtMinMaxOffset.y, ContrPtMinMaxOffset.x, ContrPtMinMaxOffset.y)); //Create new Random Value

                SecondControlPoint = SecondPoint + ControlPointOffset;
            }

            if (j == curveCount / 2 - 2)
            {
                zDir *= -1;
            }
            else if (j == curveCount / 2)
            {
                yStep *= -1;
            }

            float t = 0;

            for (int i = 0; i < curveDetail; i++)
            {
                if (i == 0 && j != 0)
                {
                    continue;
                }

                Vector3 curvePoint;

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
                curvePoint.y = y;
                y += yStep;

                curvePoints.Add(curvePoint);
            }
        }
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
        if (curvePoints.Count != 0)
        {
            Gizmos.color = Color.grey;

            int index = 0;
            foreach (Vector3 p in curvePoints)
            {
                Gizmos.DrawSphere(p, 0.05f);

                index++;
            }
            
            Gizmos.color = Color.magenta;
            index = 0;
            foreach(Vector3 p in finalPerps)
            {
                Gizmos.DrawLine(p, curvePoints[index]);
                index++;
            }

             Gizmos.color = Color.red;
             index = 0;
             foreach (Vector3 p in orthogonal)
             {
                 //Gizmos.DrawSphere(p, 0.03f);

                 Gizmos.DrawLine(p, curvePoints[index]);
                 index++;
             }

            Gizmos.color = Color.grey;
            for(int i = 0; i < mesh.vertices.Length; i++)
            {
                Gizmos.DrawSphere(mesh.vertices[i], 0.005f);
            }
        }
    }*/
}
