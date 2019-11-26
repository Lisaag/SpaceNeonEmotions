using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public static PortalManager _instance;
    public static PortalManager Instance { get { return _instance; } }
    public GameObject player, portal, firstRoomParent;
    public GameObject[] shapes = new GameObject[3];
    public float requiredDistancePlayerObject;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void CheckSpawnPortal(bool[] spawnPortals)
    {
        for (int i = 0; i < shapes.Length; i++)
        {
            if (spawnPortals[i])
            {
                if (Vector3.Distance(player.transform.position, shapes[i].transform.position) < requiredDistancePlayerObject && !shapes[i].GetComponent<Rigidbody>().isKinematic && !shapes[i].GetComponent<Collider>().isTrigger)
                {
                    GameObject newPortal = Instantiate(portal, firstRoomParent.transform);
                    newPortal.GetComponent<Portal>().shape = shapes[i];
                }
            }
        }
    }
}
