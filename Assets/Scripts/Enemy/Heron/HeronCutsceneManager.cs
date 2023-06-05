using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class HeronCutsceneManager : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private Transform secondCamPoint;

    private bool started;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, secondCamPoint.position, Time.deltaTime);
        }
    }

    [YarnCommand("Start")]
    public void StartScreaming()
    {
        started = true;
    }

    public void Done()
    {
        
    }
}
