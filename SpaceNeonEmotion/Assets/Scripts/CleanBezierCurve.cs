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
    List<Vector3> perpVectorsReversed = new List<Vector3>();

    List<Vector3> orthogonal = new List<Vector3>();
    List<Vector3> tmps = new List<Vector3>();
    List<Vector3> finalPerps = new List<Vector3>();
    List<Vector3> checkPerps = new List<Vector3>();
    List<Vector3> correctPerps = new List<Vector3>();

    List<float> angles = new List<float>();
    List<Vector3> offAnglePoints = new List<Vector3>();

    bool swapped;

    List<Vector3> tempOrderedVerts = new List<Vector3>();
    List<Vector3> tmpVerts = new List<Vector3>();

    List<Vector3> sloep = new List<Vector3>();

    List<Vector3> gizzies = new List<Vector3>();
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
        if (i == (mesh.vertices.Length / cylinderDetail) - 2)
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
            Vector3 checkPerp;
            Vector3 correctPerp;
            Vector3 firstPerpRev;

            if (j < curveDetail)
            {
                Vector3 p = curvePoints[j] + new Vector3(0, 0, 10);
                firstPerp = curvePoints[j] - (p - curvePoints[j]).normalized;
                firstPerpRev = new Vector3(0, 0, 0);
                tmp = new Vector3(0, 0, 0);
                finalperp = curvePoints[j] - (p - curvePoints[j]).normalized;
                checkPerp = new Vector3(0, 0, 0);
                correctPerp = new Vector3(0, 0, 0);
            }
            else
            {
                /*  Vector3 baseLine = curvePoints[j + 1] - curvePoints[j]; //first vector of orthogonal basis
                                                                          //inversing the baseline vector, making one negative (x=-z, z=x), and dividing it by the baseline's magnitude
                  Vector2 perp2D = new Vector2(-baseLine.z, baseLine.x) / Mathf.Sqrt(Mathf.Pow(baseLine.x, 2) + Mathf.Pow(baseLine.z, 2)); //;
                  firstPerp = new Vector3(perp2D.x, curvePoints[j].y, perp2D.y).normalized; //Direction of the perpendicular vector
                  firstPerp = curvePoints[j] + (firstPerp * 0.2f); //offsetting the perpendicular vector from circle origin
                  firstPerp.y = curvePoints[j].y; //resetting the y axis, bc this is a 2d perpendicular vector

                  baseLine = curvePoints[j - 1] - curvePoints[j];
                  perp2D = new Vector3(-baseLine.z, baseLine.x) / Mathf.Sqrt(Mathf.Pow(baseLine.x, 2) + Mathf.Pow(baseLine.z, 2));
                  tmp = new Vector3(perp2D.x, curvePoints[j].y, perp2D.y).normalized;
                  tmp = curvePoints[j] + (tmp * 0.2f * -1.0f); //offsetting the perpendicular vector from circle origin
                  tmp.y = curvePoints[j].y; //resetting the y axis, bc this is a 2d perpendicular vector*/


                /* finalperp = (((firstPerp + tmp) / 2.0f) - curvePoints[j]).normalized;
                 checkPerp = curvePoints[j] - (finalperp * 0.1f);
                 finalperp = curvePoints[j] + (finalperp * 0.1f);
                 finalperp.y = curvePoints[j].y;*/
                correctPerp = new Vector3(0, 0, 0);
                firstPerp = Vector3.Cross((curvePoints[j] - curvePoints[j + 1]).normalized, Vector3.up.normalized).normalized;
                firstPerpRev = curvePoints[j] - (firstPerp * 0.3f);
                firstPerp = curvePoints[j] + (firstPerp * 0.3f);
                firstPerp.y = curvePoints[j].y;
                firstPerpRev.y = curvePoints[j].y;

                tmp = Vector3.Cross((curvePoints[j] - curvePoints[j - 1]).normalized, Vector3.up.normalized).normalized;
                tmp = curvePoints[j] - (tmp * 0.3f);
                tmp.y = curvePoints[j].y;
                finalperp = (((firstPerp + tmp) / 2.0f) - curvePoints[j]).normalized;
                checkPerp = curvePoints[j] - (finalperp * 0.25f);
                checkPerp.y = curvePoints[j].y;
                finalperp = curvePoints[j] + (finalperp * 0.25f);
                finalperp.y = curvePoints[j].y;

                float angle = Vector3.Angle(curvePoints[j] - firstPerp, curvePoints[j] - tmp);
                float otherangle = Vector3.Angle(curvePoints[j] - tmp, curvePoints[j] - firstPerp);
                if (angle > 60.0f || otherangle > 60.0f)
                {
                    // tmps.Add(tmp);
                    // perpVectors.Add(firstPerp);
                    Debug.Log("angle: " + angle);
                    offAnglePoints.Add(curvePoints[j]);
                }

                float dist = (finalPerps[j - 1] - finalperp).magnitude;
                if (dist > (checkPerps[j - 1] - finalperp).magnitude)
                {
                    if (!swapped)
                    {
                        correctPerp = checkPerp;
                        swapped = true;
                    }
                    else if (swapped)
                    {
                        correctPerp = finalperp;
                        swapped = false;
                    }
                }
                else
                {
                    if (!swapped) correctPerp = finalperp;
                    if (swapped) correctPerp = checkPerp;
                }
            }

            tmps.Add(tmp);
            perpVectors.Add(firstPerp);
            perpVectorsReversed.Add(firstPerpRev);
            finalPerps.Add(finalperp);
            checkPerps.Add(checkPerp);
            correctPerps.Add(correctPerp);

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
                Vector3 normal = Mathf.Cos(i) * (center - secondPerp).normalized + Mathf.Sin(i) * (center - correctPerp).normalized;

                tempOrderedVerts.Add(p);
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
              //  sloep.Add(verts[checkIndex]);
                //Debug.Log(checkVerts.Count);
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
                //gizzies.Add(verts[checkIndex]);
                gizzies.Add(checkVerts[shortestDistIndex]);

                //   Debug.Log("shortestDistIndex: " + shortestDistIndex);
                //Reorder vertices
                index = 0;
                foreach (Vector3 v in checkVerts)
                {
                    if (shortestDistIndex == cylinderDetail) shortestDistIndex = 0;
                    //Debug.Log("index: " + shortestDistIndex);
                    sloep.Add(checkVerts[shortestDistIndex]);
                    normals.Add(checkNormals[shortestDistIndex]);
                    shortestDistIndex++;
                    //if()
                    //checkVerts[shortestDistIndex]
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
           /* else if (j == curveCount / 2 - 1)
            {
                yOffset = 0;
            }*/
            else if (j == curveCount / 2)
            {
                yOffset = -1;
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
                y += (yStep * yOffset);

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

    void OnDrawGizmos()
    {
        /* Gizmos.color = Color.blue;
         foreach (Vector3 v in gizzies)
         {
             Gizmos.DrawCube(v * this.transform.localScale.y + this.transform.localPosition, new Vector3(0.005f, 0.005f, 0.005f));
         }*/

        for (int i = 0; i < sloep.Count; i++)
        {
            int circleNum = i % 3;
            if(i % cylinderDetail == 0)
            {
                Gizmos.color = Color.magenta;
            }
            else if (circleNum == 0)
            {
                Gizmos.color = Color.blue;
            }
            else if (circleNum == 1)
            {
                Gizmos.color = Color.green;
            }
            else if (circleNum == 2)
            {
                Gizmos.color = Color.yellow;
            }
            else { Gizmos.color = Color.red; }



            /*  switch (circleNum)
              {
                  case 0:
                      Gizmos.color = Color.blue;
                      break;
                  case 1:
                      Gizmos.color = Color.clear;
                      break;
                  case 2:
                      Gizmos.color = Color.cyan;
                      break;
                  case 3:
                      Gizmos.color = Color.gray;
                      break;
                  case 4:
                      Gizmos.color = Color.magenta;
                      break;
                  case 5:
                      Gizmos.color = Color.white;
                      break;
                  case 6:
                      Gizmos.color = Color.yellow;
                      break;
                  case 7:
                      Gizmos.color = Color.cyan;
                      break;
                  case 8:
                      Gizmos.color = Color.magenta;
                      break;
                  default:
                      Gizmos.color = Color.red;
                      break;
              }*/

            Gizmos.DrawSphere(sloep[i] * this.transform.localScale.y + this.transform.localPosition, 0.002f);
        }
        /* int index = 0;
         foreach(Vector3 p in perpVectors)
         {
             Gizmos.color = Color.green;
             Gizmos.DrawLine(p * this.transform.localScale.y + this.transform.localPosition, curvePoints[index] * this.transform.localScale.y + this.transform.localPosition);
             Gizmos.color = Color.blue;
             Gizmos.DrawLine(tmps[index] * this.transform.localScale.y + this.transform.localPosition, curvePoints[index] * this.transform.localScale.y + this.transform.localPosition);
             index++;
         }

         index = 0;
         foreach(Vector3 v in offAnglePoints)
         {
             Gizmos.color = Color.red;
             Gizmos.DrawSphere(v * this.transform.localScale.y + this.transform.localPosition, 0.01f);
         }*/
        /*  int index = 0;
          foreach (Vector3 v in finalPerps)
          {
              Gizmos.color = Color.grey;
              Gizmos.DrawLine(v * this.transform.localScale.y + this.transform.localPosition, checkPerps[index] * this.transform.localScale.y + this.transform.localPosition);
              Gizmos.color = Color.green;
              Gizmos.DrawSphere(v * this.transform.localScale.y + this.transform.localPosition, 0.01f);
              Gizmos.color = Color.cyan;
              Gizmos.DrawSphere(checkPerps[index] * this.transform.localScale.y + this.transform.localPosition, 0.01f);
              Gizmos.color = Color.magenta;
              Gizmos.DrawSphere(correctPerps[index] * this.transform.localScale.y + this.transform.localPosition, 0.005f);
              index++;

          }

          foreach (Vector3 p in offAnglePoints)
          {
              Gizmos.color = Color.red;
              Gizmos.DrawSphere(p * this.transform.localScale.y + this.transform.localPosition, 0.0125f);
          }

          index = 0;
          foreach (Vector3 v in tmps)
          {
              Gizmos.color = Color.green;
              Gizmos.DrawSphere(v * this.transform.localScale.y + this.transform.localPosition, 0.01f);
              Gizmos.color = Color.blue;
              Gizmos.DrawSphere(perpVectors[index] * this.transform.localScale.y + this.transform.localPosition, 0.011f);

              index++;
          }*/
    }
}
