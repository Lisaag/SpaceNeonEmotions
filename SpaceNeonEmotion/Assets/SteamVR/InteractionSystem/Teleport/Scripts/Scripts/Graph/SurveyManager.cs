using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SurveyManager : MonoBehaviour
{
    [SerializeField]
    float respawnTime;

    [SerializeField]
    GameObject buttons;

    public List<Tuple<int, float>> surveyData = new List<Tuple<int, float>>();


    void Start()
    {
        buttons.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            surveyData.Add(new Tuple<int, float>(1, Time.time));
        }
    }

    public void CallActivateSurvey()
    {
        StartCoroutine(ActivateSurvey());
    }

    IEnumerator ActivateSurvey()
    {
        yield return new WaitForSeconds(respawnTime);

        buttons.SetActive(true);
    }
}