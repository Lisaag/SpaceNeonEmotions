using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TutorialManager : MonoBehaviour
{
    public List<Phase> phases;
    public int currentPhase = 0;

    public TeleportArea teleportArea;

    public List<TeleportPoint> teleportPoints;

    public Material highlighted, regular;

    public GameObject thumbButton, indexButton, controller;

    public Teleport teleport;

    [System.Serializable]
    public struct Phase
    {
        public List<GameObject> phaseObjects;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i < phases.Count; i++) // for each phase
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

        for (int i = 0; i < teleportPoints.Count; i++)
        {
            teleportPoints[i].gameObject.SetActive(false);
            teleportPoints[i].enabled = false;
        }

        thumbButton.GetComponent<Renderer>().material = highlighted;
    }

    // Update is called once per frame
    void Update()
    {
        if (teleport.timesTeleported >= 3 && currentPhase == 0)
        {
            NextPhase();
        }

        if (teleport.hasAreaTeleported && currentPhase == 1)
        {
            NextPhase();
        }
    }

    public void NextPhase()
    {
        for (int i = 0; i < phases[currentPhase].phaseObjects.Count; i++)
        {
            phases[currentPhase].phaseObjects[i].SetActive(false);
        }

        currentPhase++;

        for (int i = 0; i < phases[currentPhase].phaseObjects.Count; i++)
        {
            phases[currentPhase].phaseObjects[i].SetActive(true);
        }

        switch(currentPhase)
        {
            case 0:
                // use start method instead
                break;
            case 1:
                teleportArea.enabled = false;
                break;
            case 2:
                for (int i = 0; i < teleportPoints.Count; i++)
                {
                    teleportPoints[i].gameObject.SetActive(false);
                    teleportPoints[i].enabled = false;
                }

                thumbButton.GetComponent<Renderer>().material = regular;
                indexButton.GetComponent<Renderer>().material = highlighted;
                break;
            case 3:
                teleportArea.gameObject.SetActive(true);
                teleportArea.enabled = true;
                controller.SetActive(false);
                break;
            case 4:
                break;
        }
    }
}
