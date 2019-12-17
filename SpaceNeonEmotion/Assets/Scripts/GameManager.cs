using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    XYStreamReader reader;
    public int heartrate;

    public AudioSource doorsMoving;
    public GameObject doors;
    public GameObject wire;
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
        neonMat.SetColor("_EmissionColor", new Color(0, 255, 255));
        hologramMat.SetFloat("_ScanSpeed", 0f);
        if (SoundManager.instance.baseHeartrate != 0)
        {
            baselineHeartrate = SoundManager.instance.baseHeartrate;
        } else
        {
            baselineHeartrate = 80;
        }

        //InvokeRepeating("Read", 1.0f, 1.0f);
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
        SoundManager.instance.PlaySound(doorsMoving, doors, false, 0);
        doors.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(2f);
        wire.SetActive(true);
    }

    void Read()
    {
        reader = XYStreamReader.FromFile("");
        print("Read...");
        reader.Read();
        reader.Dispose();
        heartrate = reader.heartrate;
        if (SceneManager.GetActiveScene().name != "Tutorial")
        {
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
        }
        Debug.Log(reader.heartrate);
    }
}
