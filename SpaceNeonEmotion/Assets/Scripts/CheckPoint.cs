using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class CheckPoint : MonoBehaviour
{
    [SerializeField]
    GameObject checkPoint = null;

    [SerializeField]
    GameObject wire = null;

    Vector3 ringRotatePoint;
    CleanBezierCurve wmg;

    CollisionBehaviour collisionBehaviour;

    private Vector3 startPos = new Vector3(0, 0, 0);

    bool moved;

    void Start()
    {
        wmg = wire.GetComponent<CleanBezierCurve>();
        collisionBehaviour = this.gameObject.transform.GetChild(0).GetComponent<CollisionBehaviour>();
        startPos =  new Vector3(0, 0.5f, -wire.GetComponent<CleanBezierCurve>().zOffsetPp) * wire.transform.localScale.y + wire.transform.position;
        this.transform.position = startPos;
    }

    private void Update()
    {
        if(transform.parent != null && collisionBehaviour.hasCollided)
        {
            if (transform.parent.GetComponent<Hand>().ObjectIsAttached(this.gameObject))
            {
                collisionBehaviour.hasCollided = false;
            }
        }
    }

    public void MoveRingToCheckpoint()
    {
        transform.parent.GetComponent<Hand>().DetachObject(gameObject);

        if (!collisionBehaviour.reachedCheckpoint)
        {
            this.transform.position = startPos;
            this.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            ringRotatePoint = wmg.ringDir;
            Debug.Log(ringRotatePoint);

            this.transform.position = checkPoint.transform.position;
            Debug.Log("Chakram moved");
            moved = true;

            Vector3 dir = ringRotatePoint - this.transform.position;
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = rot;

            Vector3 temp = transform.rotation.eulerAngles;
            temp.x += 90.0f;
            transform.rotation = Quaternion.Euler(temp);
        }
    }
}
