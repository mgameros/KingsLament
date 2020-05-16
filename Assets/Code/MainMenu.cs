using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // check to see if the enter button has been pressed to go to the controls menu

        float start = Input.GetAxis("Submit");

        if (start != 0)
        {
            SceneManager.LoadScene(sceneName: "Controls");
        }
    }
}
