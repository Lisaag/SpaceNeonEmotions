using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TutorialManager : MonoBehaviour
{
    public List<Phase> phases;
    public int currentPhase = 0;

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

        if (Input.GetKeyDown(KeyCode.Space))
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
    }
}
