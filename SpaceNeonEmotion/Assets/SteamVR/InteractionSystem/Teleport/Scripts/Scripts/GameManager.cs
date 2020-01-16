﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    WorldTimer worldtimer;

    [SerializeField]
    GameObject survey;

    [SerializeField]
    Transform newSurveyPosition;

    public int baselineHeartrate = 80;
    int heartChange = 10;
    public static GameManager _instance;
    public Material neonMat;
    public Material hologramMat;
    public static GameManager Instance { get { return _instance; } }
    public bool cubePlaced;
    public bool trianglePlaced;
    public bool spherePlaced;
    public GameObject bigHeart;
    public GameObject bigHeart2;
    Animator heartAnim;
    Animator heartAnim2;

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

    public bool isShapePlaced()
    {
        if (trianglePlaced || cubePlaced || spherePlaced)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void Start()
    {
        XYStreamReader.Reset();
        neonMat.SetColor("_EmissionColor", new Color(0, 255, 255));
        hologramMat.SetFloat("_ScanSpeed", 0f);
        if (bigHeart && bigHeart2)
        {
            heartAnim = bigHeart.GetComponent<Animator>();
            heartAnim2 = bigHeart2.GetComponent<Animator>();
        }
        if (SoundManager.instance.baseHeartrate != 0)
        {
            baselineHeartrate = SoundManager.instance.baseHeartrate;
        }
        else
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
            worldtimer.currentTime += 120;
            survey.transform.position = newSurveyPosition.position;
            survey.transform.localEulerAngles = new Vector3(0, 270, 0);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)){
            CloseDoors();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(StartMoveDoors());
            worldtimer.currentTime += 120;
            survey.transform.position = newSurveyPosition.position;
            survey.transform.eulerAngles += new Vector3(0, 270, 0);
        }
    }

    public IEnumerator StartMoveDoors()
    {
        SoundManager.instance.PlaySound(doorsMoving, doors, false, 0);
        doors.GetComponent<Animator>().SetFloat("speed", 1f);
        doors.GetComponent<Animator>().SetTrigger("OpenDoors");
        yield return new WaitForSeconds(2f);
    }

    public void CloseDoors()
    {
        SoundManager.instance.PlaySound(doorsMoving, doors, false, 0);
        doors.GetComponent<Animator>().SetFloat("speed", -1f);
        doors.GetComponent<Animator>().SetTrigger("OpenDoors");
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
            if (heartrate != 0)
            {
                heartAnim.SetFloat("Heartrate", ((float)heartrate / 60f) /2f);
                heartAnim2.SetFloat("Heartrate", ((float)heartrate / 60f) /2f);
            }
            //if (heartrate <= baselineHeartrate)
            //{
            //    Debug.Log("Set lower heartrate");
            //    neonMat.SetColor("_EmissionColor", new Color(0, 255, 255));
            //    hologramMat.SetFloat("_ScanSpeed", 0f);
            //}
            //else if (heartrate >= (baselineHeartrate + heartChange) && heartrate < (baselineHeartrate + heartChange * 2))
            //{
            //    Debug.Log("Set 1 up");
            //    neonMat.SetColor("_EmissionColor", new Color(0, 0, 255));
            //    hologramMat.SetFloat("_ScanSpeed", 0.5f);
            //}
            //else if (heartrate >= (baselineHeartrate + (heartChange * 2)) && heartrate < (baselineHeartrate + (heartChange * 3)))
            //{
            //    Debug.Log("Set 2 up");
            //    neonMat.SetColor("_EmissionColor", new Color(255, 0, 255));
            //    hologramMat.SetFloat("_ScanSpeed", 1f);
            //}
            //else if (heartrate >= baselineHeartrate + heartChange * 3)
            //{
            //    Debug.Log("Set Highest");
            //    neonMat.SetColor("_EmissionColor", new Color(255, 0, 0));
            //    hologramMat.SetFloat("_ScanSpeed", 2f);
            //}
        }
        Debug.Log(reader.heartrate);
    }
}
