using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class HeronCutsceneManager : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private Transform secondCamPoint;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject fakeHeron;
    [SerializeField] private GameObject heron;
    [SerializeField] private InputActionAsset PI;

    private bool started;

    private bool shaking;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            if (shaking)
            {
                mainCam.transform.position = secondCamPoint.position + new Vector3(Random.Range(-0.2f, 0.2f),
                    Random.Range(-0.2f, 0.2f), 0);
            }
            else
            {
                mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, secondCamPoint.position, Time.deltaTime);
            }
        }
    }

    [YarnCommand("Start")]
    public void StartScreaming()
    {
        started = true;
        GetComponent<Animator>().Play("New Animation");
        PI.FindActionMap("Player").Disable();
    }

    public void StartScreenshake()
    {
        shaking = true;
    }

    public void StopScreenshake()
    {
        shaking = false;
    }
    
    public void Done()
    {
        started = false;
        healthBar.SetActive(true);
        mainCam.GetComponent<CalculateCameraBox>().enabled = true;
        mainCam.GetComponent<CameraFollow>().enabled = true;
        fakeHeron.SetActive(false);
        heron.SetActive(true);
        PI.FindActionMap("Player").Enable();
    }
}
