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
    private InputAction releaseAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        frappleAction = playerInput.actions["Frapple"];
        releaseAction = playerInput.actions["Release"];

        frappleEnd = transform.parent.GetChild(1).gameObject; // get the frapple game object
        frappleScript = frappleEnd.GetComponent<FrappleScript>(); // reference the frapple script of the frappleEnd

        cam = Camera.main; //set the camera to the main camera
    }

    private void OnEnable()
    {
        frappleAction.performed += FrappleControl;

        releaseAction.performed += FrappleRelease;
    }

    private void OnDisable()
    {
        frappleAction.performed -= FrappleControl;

        releaseAction.performed += FrappleRelease;
    }

    private void FrappleControl(InputAction.CallbackContext context)
    {
        Vector2 pos = cam.ScreenToWorldPoint(context.ReadValue<Vector2>()); // position of the click in world space
        frappleScript.ShootFrapple(pos); // shoot the frapple toward target location
    }

    private void FrappleRelease(InputAction.CallbackContext context)
    {
        Debug.Log("Right click");
        frappleScript.RetractFrapple(); // retracts the frapple
    }
}
