using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class FrappleController : MonoBehaviour
{
    // frapple variables
    private FrappleIndicator frappleIndicator; // frapple indicator
    private FrappleScript frappleScript; //frapple script
    private Camera cam; // camera being used

    //private PlayerStates playerStates;
    private PlayerInput playerInput;

    //store our controls
    private InputAction frappleAction;
    private InputAction releaseAction;
    private InputAction pointAction;

    // player movement control
    private InputAction moveAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        frappleAction = playerInput.actions["Frapple"];
        releaseAction = playerInput.actions["Release"];
        pointAction = playerInput.actions["Point"];

        // sense player movement
        moveAction = playerInput.actions["Move"];

        frappleScript = transform.parent.GetChild(1).gameObject.GetComponent<FrappleScript>(); // reference the frapple script of the frappleEnd
        frappleIndicator = transform.parent.GetChild(2).gameObject.GetComponent<FrappleIndicator>(); // get the frapple indicator

        cam = Camera.main; //set the camera to the main camera
    }

    private void OnEnable()
    {
        frappleAction.performed += FrappleControl;

        releaseAction.performed += FrappleRelease;

        pointAction.performed += FrappleIndicate;
        pointAction.started += FrappleIndicate;
        pointAction.canceled += FrappleIndicate;

        moveAction.performed += ReleaseTension;
        moveAction.started += ReleaseTension;

    }

    private void OnDisable()
    {
        frappleAction.performed -= FrappleControl;

        releaseAction.performed -= FrappleRelease;

        pointAction.performed -= FrappleIndicate;
        pointAction.started -= FrappleIndicate;
        pointAction.canceled -= FrappleIndicate;

        moveAction.performed -= ReleaseTension;
        moveAction.started -= ReleaseTension;
    }

    private void FrappleControl(InputAction.CallbackContext context)
    {
        Vector2 pos = cam.ScreenToWorldPoint(context.ReadValue<Vector2>()); // position of the click in world space
        frappleScript.ShootFrapple(pos); // shoot the frapple toward target location
    }

    private void FrappleRelease(InputAction.CallbackContext context)
    {
        // Debug.Log("Right click");
        frappleScript.RetractFrapple(); // retracts the frapple
    }

    // DOESN'T WORK CONSISTENTLY
    private void ReleaseTension(InputAction.CallbackContext context)
    {
        frappleScript.ReleaseTension(context);
    }

    private void FrappleIndicate(InputAction.CallbackContext context)
    {
        Vector2 pos = cam.ScreenToWorldPoint(context.ReadValue<Vector2>()); // position the pointer is at
        frappleIndicator.Move(pos); // indicate whether that position is frappable
    }
}
