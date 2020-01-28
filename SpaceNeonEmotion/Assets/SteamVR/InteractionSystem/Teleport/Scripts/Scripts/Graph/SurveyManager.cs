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

    [SerializeField]
    AudioSource fillInSurveySound;

    public bool isPressed = false;

    public List<Tuple<int, float>> surveyData = new List<Tuple<int, float>>();


    void Start()
    {
        buttons.SetActive(true);
        fillInSurveySound.loop = true;

        StartCoroutine(ActivateSurvey());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            //surveyData.Add(new Tuple<int, float>(1, Time.time));
        }

    }

    public void CallActivateSurvey()
    {
        StartCoroutine(ActivateSurvey());
    }

    IEnumerator ActivateSurvey()
    {
        yield return new WaitForSeconds(respawnTime);
        DisableButtons(true);
        isPressed = false;
    }

    public void DisableButtons(bool isActive)
    {
        foreach (Transform t in buttons.transform)
        {
            t.gameObject.GetComponent<MeshRenderer>().enabled = isActive;
            t.gameObject.GetComponent<BoxCollider>().enabled = isActive;
        }

        if (isActive) fillInSurveySound.Play();
        else fillInSurveySound.Stop();
    }
}