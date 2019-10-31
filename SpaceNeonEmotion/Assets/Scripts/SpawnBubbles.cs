using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBubbles : MonoBehaviour
{
    public int amountToSpawn = 10;
    public GameObject bubble;
    public Vector3 offset;

    public float horMin, horMax;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            offset.x = Random.Range(horMin, horMax);
            offset.z = Random.Range(horMin, horMax);
            Instantiate(bubble, transform.position += offset, Quaternion.identity);
        }
    }
}
