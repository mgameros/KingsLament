using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    PlayableDirector director;

    // Start is called before the first frame update
    void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        // check to see if the cutscene is done to go to the end credits

        if(director.state != PlayState.Playing)
        {
            SceneManager.LoadScene(sceneName: "End Credits");
        }
    }
}
