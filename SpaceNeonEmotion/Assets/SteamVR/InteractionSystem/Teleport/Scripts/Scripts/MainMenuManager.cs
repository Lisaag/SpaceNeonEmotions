using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string sceneToLoad;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
