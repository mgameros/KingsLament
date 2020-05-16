using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCredits : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // check to see if the enter button has been pressed to move back to the main menu

        float start = Input.GetAxis("Submit");

        if (start != 0)
        {
            SceneManager.LoadScene(sceneName: "MainMenu");
        }
    }
}
