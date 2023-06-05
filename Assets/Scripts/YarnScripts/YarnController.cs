using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class YarnController : MonoBehaviour
{
    public DialogueRunner dialogueRunner;

    [YarnCommand("CameraFade")]
    public static void FadeCamera() 
    {
        Debug.Log("Fading the camera!");
    }

    [YarnCommand("SetPortrait")]
    public static void ChangePortrait()
    {
        Debug.Log("Changing Portrait");
    }
/*
    // Start is called before the first frame update
    void Awake()
    {
        // Create a new command called 'camera_look', which looks at a target. 
        // Note how we're listing 'GameObject' as the parameter type.
        dialogueRunner.AddCommandHandler<GameObject>(
            "ZoneChange",     // the name of the command
            EnterZone() // the method to run
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterZone(string sceneName)
    {
        SceneManager.LoadScene.(sceneName);
    }
    */
}
