using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
using System;
using UnityEngine.SceneManagement;

public class Survey : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    Interactable interactable;

    [SerializeField]
    int buttonIndex;

    [SerializeField]
    ButtonPresses buttonPresses;

    [SerializeField]
    SteamVR_Action_Boolean grabPinch;

    [SerializeField]
    SurveyManager surveyManager;

    SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;

    private bool isPressed;

    void Start()
    {
        if (grabPinch != null)
        {
            grabPinch.AddOnChangeListener(OnTriggerPressedOrReleased, inputSource);
        }
    }

    private void OnTriggerPressedOrReleased(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (interactable.isHovering && !isPressed)
        {
            isPressed = true;
            surveyManager.surveyData.Add(new Tuple<int, float>(buttonIndex, Time.time));
            //audioSource.Play();
            StartCoroutine(WaitAndDisable());
            buttonPresses.surveyResults.Add(buttonIndex);
        }
    }

    private IEnumerator WaitAndDisable()
    {
        yield return new WaitForSeconds(0.5f);
        transform.parent.gameObject.SetActive(false);
        surveyManager.CallActivateSurvey();
        isPressed = false;
    }
}
