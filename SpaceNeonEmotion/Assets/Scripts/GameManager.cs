using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int baselineHeartrate = 80;
    int heartChange = 10;
    public static GameManager _instance;
    public Material neonMat;
    public Material hologramMat;
    public static GameManager Instance { get { return _instance; } }
    public bool cubePlaced;
    public bool trianglePlaced;
    public bool spherePlaced;
    public bool moveDoors;
    XYStreamReader reader;
    public int heartrate;

    public AudioSource doorsMoving;

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

    private void Start()
    {
        if (SoundManager.instance.baseHeartrate != 0)
        {
            baselineHeartrate = SoundManager.instance.baseHeartrate;
        } else
        {
            baselineHeartrate = 80;
        }

        InvokeRepeating("Read", 1.0f, 1.0f);
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
        SoundManager.instance.PlaySound(doorsMoving, upperDoor, false, 0);
        SoundManager.instance.PlaySound(doorsMoving, lowerDoor, false, 0);
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

    void Read()
    {
        reader = XYStreamReader.FromFile("");
        print("Read...");
        reader.Read();
        reader.Dispose();
        heartrate = reader.heartrate;
        if (heartrate <= baselineHeartrate)
        {
            Debug.Log("Set lower heartrate");
            neonMat.SetColor("_EmissionColor", new Color(0, 255, 255));
            hologramMat.SetFloat("_ScanSpeed", 0f);
        } 
        else if (heartrate >= (baselineHeartrate + heartChange) && heartrate < (baselineHeartrate + heartChange * 2))
        {
            Debug.Log("Set 1 up");
            neonMat.SetColor("_EmissionColor", new Color(0, 0, 255));
            hologramMat.SetFloat("_ScanSpeed", 0.5f);
        } 
        else if (heartrate >= (baselineHeartrate + (heartChange * 2)) && heartrate < (baselineHeartrate + (heartChange * 3)))
        {
            Debug.Log("Set 2 up");
            neonMat.SetColor("_EmissionColor", new Color(255, 0, 255));
            hologramMat.SetFloat("_ScanSpeed", 1f);
        } 
        else if (heartrate >= baselineHeartrate + heartChange * 3)
        {
            Debug.Log("Set Highest");
            neonMat.SetColor("_EmissionColor", new Color(255, 0, 0));
            hologramMat.SetFloat("_ScanSpeed", 2f);
        }
        Debug.Log(reader.heartrate);
    }
}
