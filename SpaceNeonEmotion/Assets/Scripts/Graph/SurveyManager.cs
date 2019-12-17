using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveyManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> disableObjects = new List<GameObject>();

    [SerializeField]
    GameObject buttons;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            foreach (GameObject g in disableObjects)
            {
                g.SetActive(false);
            }
            buttons.SetActive(true);
        }
    }
}