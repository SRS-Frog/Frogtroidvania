using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class FrappleController : MonoBehaviour
{
    // frapple variables
    private GameObject frappleEnd; // end of the frapple 
    private FrappleScript frappleScript; //frapple script
    private Camera cam; // camera being used

    //private PlayerStates playerStates;
    private PlayerInput playerInput;

    //store our controls
    private InputAction frappleAction;

    //bool for if keys are pressed
    private bool frapplePressed = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        frappleAction = playerInput.actions["Frapple"];

        frappleEnd = transform.parent.GetChild(1).gameObject; // get the frapple game object
        frappleScript = frappleEnd.GetComponent<FrappleScript>(); // reference the frapple script of the frappleEnd

        cam = Camera.main; //set the camera to the main camera
    }

    private void FixedUpdate()
    {

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
        if (context.performed)
        {
            Vector2 pos = cam.ScreenToWorldPoint(context.ReadValue<Vector2>()); // position of the click in world space
            // Debug.Log(pos);

            frappleScript.ShootFrapple(pos); // shoot the frapple toward target location
        }
    }

    public bool IsGrapplePressed()
    {
        return frapplePressed;
    }
}
