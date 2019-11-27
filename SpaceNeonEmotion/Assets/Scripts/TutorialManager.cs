using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

public class TutorialManager : MonoBehaviour
{
    public List<Phase> phases;
    public int currentPhase = 0;

    public TeleportArea teleportArea;

    public string playScene;

    public Vector3 tpPointPos1, tpPointPos2, tpPointPos3;

    public Material highlighted, regular;

    private float startTime;
    public float speed = 3;

    public GameObject thumbButton, indexButton, controller, spawnPoint;

    public Color startColor, endColor;

    public Teleport teleport;

    public AudioSource introducion, teleportInstruction ,firstTeleport, threeTeleports, pickUp, done;

    [System.Serializable]
    public struct Phase
    {
        public List<GameObject> phaseObjects;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < phases.Count; i++) // for each phase
        {
            for (int j = 0; j < phases[i].phaseObjects.Count; j++) // for each object in each phase
            {
                phases[i].phaseObjects[j].SetActive(false);
            }
        }
            
        for (int i = 0; i < phases[currentPhase].phaseObjects.Count; i++)
        {
            if (!phases[currentPhase].phaseObjects[i].activeSelf)
                phases[currentPhase].phaseObjects[i].SetActive(true);
        }

        thumbButton.GetComponent<Renderer>().material = highlighted;
        startTime = Time.time;

        SoundManager.instance.PlaySound(introducion, gameObject, false, 0);
        SoundManager.instance.PlaySound(teleportInstruction, gameObject, false, (int)introducion.clip.length + 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (teleport.timesTeleported == 1 && currentPhase == 0)
        {
            StopAllHints();
            SoundManager.instance.PlaySound(firstTeleport, gameObject, false, 0);
        }

        if (teleport.timesTeleported >= 3 && currentPhase == 0)
        {
            NextPhase();
        }

        if (teleport.hasAreaTeleported && currentPhase == 1)
        {
            NextPhase();
        }

        float time = (Mathf.Sin(Time.time - startTime) * speed);
        highlighted.color = Color.Lerp(startColor, endColor, time);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextPhase();
        }
    }

    public void NextPhase()
    {
        if (currentPhase <= 2)
        {
            for (int i = 0; i < phases[currentPhase].phaseObjects.Count; i++)
            {
                phases[currentPhase].phaseObjects[i].SetActive(false);
            }

            currentPhase++;

            for (int i = 0; i < phases[currentPhase].phaseObjects.Count; i++)
            {
                phases[currentPhase].phaseObjects[i].SetActive(true);
                if (phases[currentPhase].phaseObjects[i].GetComponent<TeleportArea>() != null)
                {
                    Destroy(phases[currentPhase].phaseObjects[i].GetComponent<TeleportArea>());
                }
            }

            switch (currentPhase)
            {
                case 0:
                    // use start method instead
                    break;
                case 1:
                    Instantiate(spawnPoint, tpPointPos1, Quaternion.identity, null);
                    Instantiate(spawnPoint, tpPointPos2, Quaternion.identity, null);
                    Instantiate(spawnPoint, tpPointPos3, Quaternion.identity, null);
                    StopAllHints();
                    SoundManager.instance.PlaySound(threeTeleports, gameObject, false, 0);
                    break;
                case 2:
                    thumbButton.GetComponent<Renderer>().material = regular;
                    indexButton.GetComponent<Renderer>().material = highlighted;
                    StopAllHints();
                    SoundManager.instance.PlaySound(pickUp, gameObject, false, 0);
                    break;
                case 3:
                    StopAllHints();
                    SoundManager.instance.PlaySound(done, gameObject, false, 0);
                    StartCoroutine(SwapScene());
                    break;
            }
        }
    }

    private void StopAllHints()
    {
        if (introducion.isPlaying)
            introducion.Stop();

        if (teleportInstruction.isPlaying)
            teleportInstruction.Stop();

        if (firstTeleport.isPlaying)
            firstTeleport.Stop();

        if (threeTeleports.isPlaying)
            threeTeleports.Stop();

        if (pickUp.isPlaying)
            pickUp.Stop();

        if (done.isPlaying)
            done.Stop();
    }

    private IEnumerator SwapScene()
    {
        SoundManager.instance.baseHeartrate = GameManager._instance.heartrate;
        yield return new WaitForSeconds(done.clip.length + 1);
        SceneManager.LoadScene(playScene);
    }
}
