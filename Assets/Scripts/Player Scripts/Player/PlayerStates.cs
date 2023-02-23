using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerAttributes playerAttributes;
    private PlayerController playerController;

    //isGrounded check
    private bool isGrounded; // true if player is touching the ground
    public Transform GroundCheck; // The position of a GameObject that will mark the player's feet
    public LayerMask groundLayer; // determines which layers count as the ground

    //states
    private bool isMoving;
    private bool isJumping;
    private bool isPlunging;
    private bool isAttacking;

    private bool isImmune;
    private bool isStaggerResistant;

    //logic
    private bool canMove;
    private bool canJump;
    private bool canPlunge;

    //other
    private int dir;
    private RaycastHit2D hit;

    //if inputs are pressed
    private bool movePressed;
    private bool jumpPressed;
    private bool attackPressed;
    private bool plungePressed;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAttributes = GetComponent<PlayerAttributes>();
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!hit)
            canPlunge = true;
        else
            canPlunge = false;
        Debug.Log(canPlunge);

        if(isPlunging || isAttacking)
            canMove = false;
        else
            canMove = true;
        if(isGrounded)
            canJump = true;
        else 
            canJump = false;
    }

    private void FixedUpdate()
    {
        ///things to check: move, can plunge, 
        // checks if you are within 0.05 position in the Y of the ground layer
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, 0.05f, groundLayer);
        Debug.Log("isGrounded " + isGrounded);

        hit = Physics2D.Raycast(transform.position, Vector3.down, playerAttributes.GetMaxRayLength(), groundLayer.value);
        if(isGrounded)
            isPlunging = false;
        
        //Debug.Log("isMoving in PlayerStates is " + isMoving);
        if(movePressed && canMove)
        {
            playerMovement.Move(dir);
            //isMoving = true;
        }
        else
        {
            //isMoving = false;
            //playerMovement.Move(0);
        }
        if(jumpPressed && canJump)
        {
            playerMovement.Jump();
            isJumping = true;
        }
        else
            isJumping = false;

        if(isPlunging || (plungePressed && canPlunge))
        {
            playerMovement.Plunge();
            isPlunging = true;
        }
    }

    //setter functions
    public void SetMovePressed(bool moveButtonPressed, int dir)
    {
        movePressed = moveButtonPressed;
        this.dir = dir;
    }
    public void SetJumpPressed(bool jumpButtonPressed)
    {
        jumpPressed = jumpButtonPressed;
    }
    public void SetAttackPressed(bool attackButtonPressed)
    {
        if(isGrounded)
            attackPressed = true;
        else
            plungePressed = true;

        ///remember to reset these
    }

    public void SetIsMoving(bool isMove)
    {
        isMoving = isMove;
    }
}
