using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string newScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {;
            if (SceneManager.GetActiveScene().name == "FinalMenu")
            {
                Destroy(GameObject.Find("FirstRoom"));
                Destroy(GameObject.Find("SoundManager"));
            }
            SceneManager.LoadScene(newScene);
        }
    }
}
