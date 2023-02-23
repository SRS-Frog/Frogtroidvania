using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class FrappleScript : MonoBehaviour
{
    // inspector variables
    [SerializeField] private int frappleDistance = 10;
    
    //private PlayerStates playerStates;
    private PlayerInput playerInput;

    //store our controls
    private InputAction frappleAction;

    //bool for if keys are pressed
    private bool frapplePressed = false;

    //specific for movement
    private int dir;

    private void Awake()
    {
        //playerStates = GetComponent<PlayerStates>();

        playerInput = GetComponent<PlayerInput>();
        frappleAction = playerInput.actions["Frapple"];
    }

    private void OnEnable()
    {
        frappleAction.started += FrappleControl;
        frappleAction.performed += FrappleControl;
        frappleAction.canceled += FrappleControl;
    }

    private void OnDisable()
    {
        frappleAction.started -= FrappleControl;
        frappleAction.performed -= FrappleControl;
        frappleAction.canceled -= FrappleControl;
    }

    private void FrappleControl(InputAction.CallbackContext context)
    {
        //frappleAction.ReadValue<>
        if (context.performed)
        {
            Debug.Log("Frappled");
        }
    }

    public bool IsGrapplePressed()
    {
        return frapplePressed;
    }
}
