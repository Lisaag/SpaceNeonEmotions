using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField]
    GameObject checkPoint = null;

    [SerializeField]
    GameObject wire = null;

    Vector3 ringRotatePoint;
    WireMeshGeneration wmg;

    bool moved;

    void Start()
    {
        wmg = wire.GetComponent<WireMeshGeneration>();
        ringRotatePoint = wmg.ringDir;
    }

    void Update()
    {
        if(wmg.ringDir == null)
        {
            ringRotatePoint = wmg.ringDir;
        }
        else if (Input.GetKeyDown(KeyCode.V) && !moved)
        {
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
