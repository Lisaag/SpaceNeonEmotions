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

    public bool isPressed = false;

    public List<Tuple<int, float>> surveyData = new List<Tuple<int, float>>();


    void Start()
    {
        buttons.SetActive(true);
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
        //buttons.SetActive(true);
        isPressed = false;
    }

    public void DisableButtons(bool isActive)
    {
        foreach (Transform t in buttons.transform)
        {
            t.gameObject.GetComponent<MeshRenderer>().enabled = isActive;
            t.gameObject.GetComponent<BoxCollider>().enabled = isActive;
        }
    }
}