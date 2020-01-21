using System.Collections.Generic;
using UnityEngine;

public class WorldTimer : MonoBehaviour
{
    [SerializeField]
    int timeMinutes = 5;

    [SerializeField]
    GameObject[] digitalNumbers = null;

    [SerializeField]
    List<GameObject> removeObjects = new List<GameObject>();

    [SerializeField]
    Graph graph = null;

    [SerializeField]
    AudioSource timeIsUpSound;
    [SerializeField]
    AudioSource seeResultsSound;
    [SerializeField]
    AudioSource oneMinuteLeftSound;

    float totalSeconds = 0;
    public float currentTime = 0;
    int currentTimeInt = 0;
    readonly int secondsInMinute = 60;

    bool firstTimeDisplay = false;
    int previousTimeSeconds = 0;
    int previousTimeMinutes = 0;

    private bool timeIsUp = false;

    void Start()
    {
        totalSeconds = timeMinutes * secondsInMinute;
        currentTime = totalSeconds;
    }

    public void Endgame()
    {
        StartCoroutine(GameManager.Instance.StartMoveDoors());

        foreach (GameObject o in removeObjects)
        {
            o.SetActive(false);
            CreateNumbers(-1, digitalNumbers[0]);
            CreateNumbers(-1, digitalNumbers[1]);
            CreateNumbers(-1, digitalNumbers[2]);
            CreateNumbers(-1, digitalNumbers[3]);
        }

        graph.DrawGraph();
        timeIsUp = true;
    }

    void Update()
    {
        if (timeIsUp)
        {
            return;
        }

        currentTime -= 1 * Time.deltaTime;
        currentTimeInt = (int)currentTime;
        int currentTimeSeconds = (int)(currentTime % secondsInMinute);
        int currentTimeMinutes = (int)((currentTime - currentTimeSeconds) / secondsInMinute);

        if (currentTimeSeconds <= 0 && currentTimeMinutes <= 0)
        {
            timeIsUpSound.Play();
            seeResultsSound.PlayDelayed(timeIsUpSound.clip.length);
            Endgame();
        }

        else if (!firstTimeDisplay || currentTimeMinutes < previousTimeMinutes || currentTimeSeconds < previousTimeSeconds)
        {
            if (currentTimeMinutes == 1 && currentTimeSeconds == 0)
            {
                oneMinuteLeftSound.Play();
            }

            int firstDigitSeconds = (currentTimeSeconds / 10) % 10;
            int secondDigitSeconds = currentTimeSeconds % 10;
            int firstDigitMinutes = (currentTimeMinutes / 10) % 10;
            int secondDigitMinutes = currentTimeMinutes % 10;

            CreateNumbers(firstDigitMinutes, digitalNumbers[0]);
            CreateNumbers(secondDigitMinutes, digitalNumbers[1]);
            CreateNumbers(firstDigitSeconds, digitalNumbers[2]);
            CreateNumbers(secondDigitSeconds, digitalNumbers[3]);

            previousTimeMinutes = currentTimeMinutes;
            previousTimeSeconds = currentTimeSeconds;
            firstTimeDisplay = true;


        }
    }

    void CreateNumbers(int number, GameObject dn)
    {
        GameObject[] dnParts = new GameObject[7];

        for (int i = 0; i < dnParts.Length; i++)
        {
            dnParts[i] = dn.transform.GetChild(i).gameObject;
            dnParts[i].SetActive(true);
        }

        switch (number)
        {
            case 0:
                dnParts[3].SetActive(false);
                break;
            case 1:
                dnParts[0].SetActive(false);
                dnParts[1].SetActive(false);
                dnParts[3].SetActive(false);
                dnParts[4].SetActive(false);
                dnParts[6].SetActive(false);
                break;
            case 2:
                dnParts[2].SetActive(false);
                dnParts[4].SetActive(false);
                break;
            case 3:
                dnParts[0].SetActive(false);
                dnParts[4].SetActive(false);
                break;
            case 4:
                dnParts[0].SetActive(false);
                dnParts[1].SetActive(false);
                dnParts[6].SetActive(false);
                break;
            case 5:
                dnParts[0].SetActive(false);
                dnParts[5].SetActive(false);
                break;
            case 6:
                dnParts[5].SetActive(false);
                break;
            case 7:
                dnParts[0].SetActive(false);
                dnParts[1].SetActive(false);
                dnParts[3].SetActive(false);
                dnParts[4].SetActive(false);
                break;
            case 9:
                dnParts[0].SetActive(false);
                break;
            default:
                break;
        }
    }
}
