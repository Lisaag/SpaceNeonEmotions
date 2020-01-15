using UnityEngine;
using Valve.VR.InteractionSystem;

public class CheckPoint : MonoBehaviour
{
    [SerializeField]
    GameObject checkpointsParent = null;

    [SerializeField]
    GameObject wire = null;

    [SerializeField]
    GameObject wireStartPoint = null;

    Vector3 ringRotatePoint;
    CleanBezierCurve wmg;

    CollisionBehaviour collisionBehaviour;
    readonly bool isHovering = false;

    public Vector3 startPos = new Vector3(0, 0, 0);

    Animator animator;

    void Start()
    {
        wmg = wire.GetComponent<CleanBezierCurve>();
        collisionBehaviour = this.gameObject.transform.GetChild(0).GetComponent<CollisionBehaviour>();
        animator = this.gameObject.GetComponent<Animator>();

        startPos = new Vector3(0, 0.4f, -wire.GetComponent<CleanBezierCurve>().zOffsetPp) * wire.transform.localScale.y + wire.transform.position;
        wireStartPoint.transform.position = startPos;
        startPos.y += 0.035f;
        this.transform.position = startPos;
    }

    private void Update()
    {
        if (transform.parent != null && collisionBehaviour.hasCollided)
        {
            if (transform.parent.GetComponent<Hand>())
            {
                if (transform.parent.GetComponent<Hand>().ObjectIsAttached(this.gameObject))
                {
                    collisionBehaviour.hasCollided = false;
                }
            }
        }
    }

    public void MoveRingToStartPoint()
    {
        if (transform.parent != null && transform.parent.GetComponent<Hand>())
        {
            transform.parent.GetComponent<Hand>().DetachObject(gameObject);
        }
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        this.transform.position = startPos;
        Vector3 rot = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.Euler(rot);
    }

    public void MoveRingToCheckpoint(int id)
    {
        transform.parent.GetComponent<Hand>().DetachObject(gameObject);

        if (id == -1)
        {
            this.transform.position = startPos;
            this.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            Debug.Log("moving chackram to checkpoint: " + id);
            ringRotatePoint = wmg.ringDir[id];
            Debug.Log(ringRotatePoint);

            this.transform.position = checkpointsParent.transform.GetChild(id).position;
            // moved = true;

            Vector3 dir = ringRotatePoint - this.transform.position;
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = rot;

            Vector3 temp = transform.rotation.eulerAngles;
            temp.x += 90.0f;
            transform.rotation = Quaternion.Euler(temp);
        }
    }
}
