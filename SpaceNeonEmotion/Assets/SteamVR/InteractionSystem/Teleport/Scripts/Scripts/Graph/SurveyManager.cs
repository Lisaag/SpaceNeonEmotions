using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveyManager : MonoBehaviour
{
    [SerializeField]
    float respawnTime;

    [SerializeField]
    GameObject buttons;

    void Start()
    {
        buttons.SetActive(true);
    }


    public IEnumerator ActivateSurvey()
    {
        yield return new WaitForSeconds(respawnTime);

        buttons.SetActive(true);
    }
}