using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CleanBezierCurve : MonoBehaviour
{
    Mesh mesh;

    [Tooltip("Amount of detail of each curve the wire consists of")]
    [SerializeField]
    int curveDetail = 15;

    [Tooltip("Amount of curves the wire consists of")]
    [SerializeField]
    int curveCount = 14;

    [Tooltip("The bounds relative to the x-axis within the wire will be generated")]
    [SerializeField]
    float boundsX = 0;

    [Tooltip("The bounds relative to the z-axis within the wire will be generated")]
    [SerializeField]
    float boundsZ = 0;

    [Tooltip("Detail of the circles (the width) of the wire")]
    [SerializeField]
    int cylinderDetail = 0;

    [Tooltip("Radius of the wire")]
    [SerializeField]
    float radius = 0.1f;

    [Tooltip("Object that holds all the wire's checkpoints")]
    [SerializeField]
    GameObject checkpointsParent = null;

    [Tooltip("Object with collider that detects whether the player has finished the wire")]
    [SerializeField]
    GameObject wireEnding = null;

    [Tooltip("The amount every point of the curve will be offset relative to the y-axis")]
    [SerializeField]
    float yStep = 0.0f;

    [Tooltip("Determines the amount of time it takes for the wire to despawn/respawn")]
    [SerializeField]
    float generateSpeed = 0;

    [Tooltip("Determines the rotation of the chakram/checkpoints on the wire")]
    public Vector3[] ringDir;

    [Tooltip("Determines the offset between the wire teleport point and the start&ending point of the wire")]
    public float zOffsetPp = 0;

    List<Vector3> perpVectors = new List<Vector3>();

    List<Vector3> orthogonal = new List<Vector3>();

    List<Vector3> sloep = new List<Vector3>();

    List<Vector3> gizzies = new List<Vector3>();

    List<Vector3> curvePoints = new List<Vector3>(); //list containing all the points of the wire's curve

    List<int> triangles = new List<int>();

    bool isFirstCurve = true;
    int wireIndex = 0; //The index of the wire that is generated

    public float playerHeight = 1.0f; //height of the player, used to determine the height of the wire

    void Start()
    {
        curveCount -= curveCount % 2; //to make the curveCount an even number
        CalculateWire(); //Calculate the first wire of the game
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            placeNewWire();
        }
    }

    public void placeNewWire()
    {
        for (int i = 0; i < checkpointsParent.transform.childCount; i++)
        {
            checkpointsParent.transform.GetChild(i).transform.GetComponent<MeshRenderer>().enabled = false; //disable checkpoints before removing the wire
        }
        StartCoroutine(RemoveWire());
    }

    IEnumerator RemoveWire()
    {
        if (triangles.Count == 0)
        {
            CalculateWire(); //If the wire is completely removed, calculate a new wire
            yield break;
        }
        triangles.RemoveRange(triangles.Count - (cylinderDetail) * 6, (cylinderDetail) * 6); //remove one ring of triangles
        mesh.triangles = triangles.ToArray(); //update triangle data with the removed triangles

        yield return new WaitForSeconds(generateSpeed);
        StartCoroutine(RemoveWire()); //repeat
    }

    void CalculateWire()
    {
        float wireConstant = 1.0f;
        if (wireIndex == 1)
        {
            curveCount = 7;
            wireConstant = 0.051f;
        }
        else if (wireIndex >= 2)
        {
            curveCount = 14;
            wireConstant = 0.027f;
        }

        yStep = wireConstant * playerHeight;


        Reset(); //Remove all data of the previous wire
        InitializeMesh(); //get all the mesh components
        CalculateCurve(); //calculate all the points on the curve
        CreateCircle(); //create circles around the curve points, and add the points of these circles to the vertex array of the mesh
        int triangleIndex = 0;
        StartCoroutine(WaitDrawTriangle(triangleIndex)); //calculate the triangle data of the mesh
        PlaceWireEnding(); //Add collision box to the end of the wire, to make it possible to detect when the player has finished the puzzle
        wireIndex++;
    }

    void PlaceWireEnding()
    {
        wireEnding.transform.localPosition = new Vector3(0.0f, 0.0f, zOffsetPp);
    }

    void Reset()
    {
        if (isFirstCurve) return; //if its the first wire, there is no data yet, so return
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
        ringDir = new Vector3[checkpointsParent.transform.childCount]; //the direction the checkpoint should face = the next curve point
        int wireDivisions = checkpointsParent.transform.childCount + 1; //wire gets divided to the amount of checkpoints to determine the positions

        for (int i = 0; i < checkpointsParent.transform.childCount; i++)
        {
            checkpointsParent.transform.GetChild(i).transform.localPosition =
                curvePoints[(curvePoints.Count / wireDivisions) * (i + 1)] * transform.localScale.y + transform.position; //position of checkpoint
            ringDir[i] = curvePoints[(curvePoints.Count / wireDivisions) * (i + 1) + 1] * transform.localScale.y + transform.position;

            Vector3 relativePos = ringDir[i] - checkpointsParent.transform.GetChild(i).position; //direction to face

            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            checkpointsParent.transform.GetChild(i).rotation = rotation;
            checkpointsParent.transform.GetChild(i).transform.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    IEnumerator WaitDrawTriangle(int i)
    {
        if ((i == ((mesh.vertices.Length - cylinderDetail) / cylinderDetail)))
        {
            GetComponent<MeshCollider>().sharedMesh = mesh;
            isFirstCurve = false;
            PlaceCheckPoints();
            yield break;
        }

        for (int j = 0; j < cylinderDetail; j++)
        {
            triangles.Add(0 + j + (i * cylinderDetail));
            triangles.Add(cylinderDetail + j + (i * cylinderDetail));
            triangles.Add(1 + j + (i * cylinderDetail));


            if (j == cylinderDetail - 1) //Last triangle should be connected to first triangle again bc its a cylinder shape
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

        yield return new WaitForSeconds(generateSpeed);

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
        float tStep = 1.0f / (curveDetail - 1); //this number determines the point on the curve
        float y = 0; //Y position of point on the curve
        int zDir = -1; //Determines on which side of the teleport point the curve is
        float yOffset = 1; //Determines whether the wire goes up or down;

        Vector2 ContrPtMinMaxOffset = new Vector2(1.0f, 1.5f); //minimal & maximum offset of the controllpoints relative to the points

        Vector3 FirstPoint = new Vector3(0, 0, 0); ;
        Vector3 SecondPoint = new Vector3(0, 0, 0);
        Vector3 FirstControlPoint = new Vector3(0, 0, 0);
        Vector3 SecondControlPoint = new Vector3(0, 0, 0);
        Vector3 lastPointDir = new Vector3(0, 0, 0);

        bool wireMiddle = false; //set to true when the curve in exactly the middle of the wire is being calculated (the highest curve of the wire)

        Vector3 curvePoint;

        if (isFirstCurve) //Code for the arch-shaped first curve
        {
            float t = 0;

            for (int i = 0; i < curveDetail * 2; i++)
            {
                float height = 6.05f * playerHeight;
                float offset = 2.0f;
                float yPosition = -0.35f;
                curvePoint = Mathf.Pow((1 - t), 3) * new Vector3(0.0f, yPosition, -offset) + 3 * Mathf.Pow((1 - t), 2) * t * new Vector3(0.0f, height, -offset)
                + 3 * (1 - t) * Mathf.Pow(t, 2) * new Vector3(0.0f, height, offset) + Mathf.Pow(t, 3) * new Vector3(0.0f, yPosition, offset);
                t += (tStep / 2);
                curvePoints.Add(curvePoint);
            }
            return;
        }

        float smallOffset = 0.005f;

        for (int j = 0; j < curveCount; j++)
        {
            Vector3 ControlPointOffset = new Vector3(RandomTwoRanges(-ContrPtMinMaxOffset.x, -ContrPtMinMaxOffset.y, ContrPtMinMaxOffset.x, ContrPtMinMaxOffset.y),
                                          0, RandomTwoRanges(-ContrPtMinMaxOffset.x, -ContrPtMinMaxOffset.y, ContrPtMinMaxOffset.x, ContrPtMinMaxOffset.y));

            if (j == 0) //The points of the first curve of the wire
            {
                FirstPoint = new Vector3(0, 0, zDir * zOffsetPp);
                SecondPoint = new Vector3(smallOffset, 0, zDir * zOffsetPp);

                float controlPointOffset = smallOffset / 3.0f;

                FirstControlPoint = new Vector3(controlPointOffset, 0, zDir * zOffsetPp);
                SecondControlPoint = new Vector3(controlPointOffset * 2.0f, 0, zDir * zOffsetPp);
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
                SecondControlPoint.z -= 0.005f;

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


    void OnDrawGizmos()
    {
        for(int i = 0; i < mesh.vertices.Length; i++)
        {
            Gizmos.DrawSphere(mesh.vertices[i], 0.01f);
        }
    }
}
