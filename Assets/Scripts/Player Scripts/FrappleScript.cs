using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class FrappleScript : MonoBehaviour
{
    // frapple variables
    [SerializeField] private float frappleDistance = 3f;
    [SerializeField] private float frappleSpeed = 5f;
    [SerializeField] private Vector3 offSet = new Vector3(0, 1, 0); // offset of frapple relative to character
    private Camera cam; // main camera
    private GameObject frappleEnd; // end point of the frapple
    private Rigidbody2D frappleRB; //rb of frapple
    private DistanceJoint2D joint; // the source of the force of the frapple

    // updated on runtime
    Vector3 targetPos; // position to frapple hook to
    private bool isLaunched = false; // whether frapple has been launched
    private bool isHooked = false; // whether successfully hooked onto a platform

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
        playerInput = GetComponent<PlayerInput>();
        frappleAction = playerInput.actions["Frapple"];
        cam = Camera.main; //get main camera
        frappleEnd = GameObject.FindGameObjectWithTag("Frapple"); // get the frapple end
        frappleRB = frappleEnd.GetComponent<Rigidbody2D>(); // get frapple's rb
        joint = frappleEnd.GetComponent<DistanceJoint2D>(); // get the distance joint

        // set to default behavior
        joint.enabled = false;
    }

    private void FixedUpdate()
    {
        Vector3 startingPos = transform.position + offSet; // where the frapple should follow the player

        // frapple behaviors
        if (isLaunched) 
        {
            MoveFrapple(targetPos); // move frapple toward the target position
        } 
        else if (isHooked) // if grappling hook is hooked
        {
            // move character
        } 
        else if (frappleEnd.transform.localPosition != startingPos) // if neither launched nor hooked, and is not at starting position
        {
            MoveFrapple(startingPos); // move it back to the starting position in world space (based on its local pos)
            Debug.Log(startingPos);
        } else
        {
            frappleEnd.transform.position = startingPos; // follow player
        }
    }
    private void MoveFrapple(Vector3 pos)
    {
        frappleRB.velocity = (pos - frappleEnd.transform.position).normalized * frappleSpeed;
        if (frappleEnd.transform.position == targetPos)
        {
            frappleRB.bodyType = RigidbodyType2D.Kinematic; // set the rb to kinematic (don't move)
            isLaunched = false; // no longer launched
        } 
        Debug.Log(frappleRB.velocity);
        Debug.Log("isLaunched, position now: " + frappleEnd.transform.position);


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
            if (!isHooked) // if not already hooked to something
            {
                targetPos = cam.ScreenToWorldPoint(context.ReadValue<Vector2>()); // convert mouse screen position to world position
                if (!isLaunched)
                {
                    frappleRB.bodyType = RigidbodyType2D.Dynamic; // set the rb to dynamic
                    isLaunched = true; // begin moving frapple end
                }

                float distance = Vector2.Distance(targetPos, transform.position); // distance between self and frapple head
                if (distance <= frappleDistance)
                {
                    isHooked = true;
                    joint.distance = distance; // set the distance joint's distance to be frapple distance
                    Debug.Log("Frapple");
                }
            }
        }
    }

    public bool IsGrapplePressed()
    {
        return frapplePressed;
    }
}
