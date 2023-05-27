using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class FrappleController : MonoBehaviour
{
    // frapple variables
    private FrappleScript frappleScript; //frapple script
    private Camera cam; // camera being used

    //private PlayerStates playerStates;
    private PlayerInput playerInput;

    //store our controls
    private InputAction frappleAction;
    private InputAction releaseAction;
    private InputAction dashAction; // can be collapsed into playerController
    private InputAction plungeAction;

    // player movement control

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        frappleAction = playerInput.actions["Frapple"];
        releaseAction = playerInput.actions["Release"];
        dashAction = playerInput.actions["Dash"];
        plungeAction = playerInput.actions["Plunge"];

        frappleScript = transform.parent.GetChild(1).gameObject.GetComponent<FrappleScript>(); // reference the frapple script of the frappleEnd

        cam = Camera.main; //set the camera to the main camera
    }

    private void OnEnable()
    {
        frappleAction.performed += FrappleControl;

        releaseAction.performed += FrappleRelease;

        dashAction.performed += FrappleFullRetract;

        plungeAction.performed += FrappleFullRetract;
    }

    private void OnDisable()
    {
        frappleAction.performed -= FrappleControl;

        releaseAction.performed -= FrappleRelease;

        dashAction.performed -= FrappleFullRetract;

        plungeAction.performed -= FrappleFullRetract;
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

    public void FrappleFullRetract(InputAction.CallbackContext context)
    {
        frappleScript.ReturnToStartPos();
        frappleScript.RetractFrapple();
    }
    
    public void FrappleFullRetract()
    {
        frappleScript.ReturnToStartPos();
        frappleScript.RetractFrapple();
    }

}
