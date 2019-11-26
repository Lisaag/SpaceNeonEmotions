using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    public bool cubePlaced;
    public bool trianglePlaced;
    public bool spherePlaced;
    public bool moveDoors;

    public GameObject upperDoor;
    public GameObject lowerDoor;
    public GameObject wire;
    // Start is called before the first frame update
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

    public void CheckPlacement()
    {
        if (cubePlaced && spherePlaced && trianglePlaced)
        {
            StartCoroutine(StartMoveDoors());
        }
    }

    IEnumerator StartMoveDoors()
    {
        moveDoors = true;
        yield return new WaitForSeconds(2f);
        wire.SetActive(true);
        moveDoors = false;
    }
    private void LateUpdate()
    {
        if (moveDoors)
        {
            Vector3 upperPos = upperDoor.transform.position;
            upperDoor.transform.position = new Vector3(upperPos.x, upperPos.y + (1f * Time.deltaTime), upperPos.z);
            Vector3 lowerPos = lowerDoor.transform.position;
            lowerDoor.transform.position = new Vector3(lowerPos.x, lowerPos.y - (1f * Time.deltaTime), lowerPos.z);
        }
    }
}
