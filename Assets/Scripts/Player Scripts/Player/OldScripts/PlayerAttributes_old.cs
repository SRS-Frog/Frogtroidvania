using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes_old : MonoBehaviour
{
    [SerializeField] private float maxRayLength;
    [SerializeField] private float plungeSpeed;
    
    private PlayerMovement playerMovement;
    private PlayerStates playerStates;
    private PlayerController playerController;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerStates = GetComponent<PlayerStates>();
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //getter functions
    public float GetMaxRayLength()
    {
        return maxRayLength;
    }
    public float GetPlungeSpeed()
    {
        return plungeSpeed;
    }
}
