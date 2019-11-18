using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public GameObject player, portal, firstRoomParent;
    public GameObject[] shapes = new GameObject[3];
    public float requiredDistancePlayerObject;

    public void CheckSpawnPortal(bool[] spawnPortals)
    {
        for (int i = 0; i < shapes.Length; i++)
        {
            if (spawnPortals[i])
            {
                if (Vector3.Distance(player.transform.position, shapes[i].transform.position) < requiredDistancePlayerObject)
                {
                    GameObject newPortal = Instantiate(portal, firstRoomParent.transform);
                    newPortal.GetComponent<Portal>().shape = shapes[i];
                }
            }
        }
    }
}
