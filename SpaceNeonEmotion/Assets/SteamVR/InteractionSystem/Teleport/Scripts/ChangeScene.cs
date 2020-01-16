using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

public class ChangeScene : MonoBehaviour
{
    public string nextScene;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Destroy(GameObject.Find("SoundManager"));
            var interactables = FindObjectsOfType<Interactable>();
            foreach (Interactable obj in interactables)
            {
                Destroy(obj.gameObject);
            }
            SceneManager.LoadScene(nextScene);
        }
    }
}
