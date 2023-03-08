using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    [Header("Movement")]
    public Rigidbody2D rb; // the Rigidbody2D component of the player 
    public CapsuleCollider2D humanCollider;
    public CapsuleCollider2D frogCollider;
    //private PlayerStates playerStates;
    //private PlayerAttributes playerAttributes;
    //private PlayerController playerController;

    //private float horDir; // -1 means move left, 1 means move right, 0 means stop
    //private float vertDir; // 1 means move up, 0 means stop
    //------------------------------------------------------------------------------------------------------------
    // All attributes that change between frog and human
    // Need to code the change into StateManager.SwitchState
    public const float humanTopSpeed = 10f; ////We can edit this value later in the Unity editor.
    public const float frogTopSpeed = 20f;
   [HideInInspector] public float topSpeed = humanTopSpeed; // horizontal speed of player. 
    public const float humanAcceleration = 40f; // can be editied via inspector
    public const float frogAcceleration = 60f;
    [HideInInspector] public float acceleration = humanAcceleration; // acceleration to pick up speed, and actual acceleration depends on GRAVITY and FRICTION
    //add these after you have showed movement left and right on Unity
    public const float humanJumpStrength = 10f;
    public const float frogJumpStrength = 20f;
    [HideInInspector] public float jumpStrength = humanJumpStrength; // jump force of player
    //-------------------------------------------------------------------------------------------------------------

    // added for airstate to check whether it is hooked to limit speed to top speed
    [HideInInspector] public bool isHooked = false; // TODO: bandaid solution (this is only true when the frapple is hooked, and false when character hits the ground)
    public float frappleTopSpeed = 15f; // the top speed when frappling is > than when on the ground

    ////add these after you have showed movement left and right on Unity
    
    // coyote time
    float coyoteTime = 0.1f;
    float coyoteTimer = 0f;
    ////you could also type public float jumpForce

    private bool jump; // true if player is on the ground and about to jump, false otherwise
    [HideInInspector] public bool isGrounded; // true if player is touching the ground
    public Transform GroundCheck; // The position of a GameObject that will mark the player's feet
    public LayerMask groundLayer; // determines which layers count as the ground

    [HideInInspector] public bool facingRight = true; // true if player is facing right

    private bool isDead = false; // true if player is dead

    private RaycastHit2D hit;

    [Header("Dash")]
    public float dashStrength = 30f;
    public float dashTime = 0.2f;
    public float baseGravity;
    [HideInInspector] public bool canDash = false; // this is reset every time you touch the ground, and you can only dash in the air

    [Header("Plunge")]
    [SerializeField] private float maxRayLength;
    public float plungeSpeed = 30f;
    [HideInInspector] public bool canPlunge;

    [Header("Attack")]
    //attacking
    public GameObject attackArea;
    public float timeToAttack = 0.15f;

    // Sprites
    public SpriteRenderer spriteRenderer;
    public Sprite humanSprite;
    public Sprite frogSprite;

    //public Animator animator; // the animator of the player
    //[HideInInspector] public GameObject player; // the player
    //[HideInInspector] public GameObject lowestBound; // an empty GameObject that marks the lowest point in your game

    ////this is similar to the Start() method; both are normally used to initialize variables.
    ////there are differences, but they are not important for this class
    // intialize variables
    void Awake()
    {
        // find the Rigidbody2D component of the object that this script is attached to
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackArea = transform.GetChild(1).gameObject;

        baseGravity = rb.gravityScale; // set the base gravity to the rb's gravity scale
        //playerStates = GetComponent<PlayerStates>();
        //playerAttributes = GetComponent<PlayerAttributes>();
        //playerController = GetComponent<PlayerController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (coyoteTimer > Mathf.Epsilon) // aka 0f
        {
            coyoteTimer -= Time.deltaTime;
        }

        if (Physics2D.OverlapCircle(GroundCheck.position, 0.05f, groundLayer)) // if touching ground
        {
            coyoteTimer = coyoteTime; // coyote timer begins
        }
        if (coyoteTimer > Mathf.Epsilon) // if the coyote timer isn't over
        {
            isGrounded = true; // it counts as being grounded
        } else
        {
            isGrounded = false; // it doesn't count as being grounded
        }

        //Debug.Log("isGrounded " + isGrounded);


        // bandaid solution: for human attributes, between flying on / from frapple and landing, airstate is clamped at a higher speed than usual
        if (isGrounded) // when grounded, it is no longer hooked
        {
            isHooked = false; // no longer hooked
            canDash = true;
            canPlunge = false;
            // Debug.Log("Grounded");
        }

        hit = Physics2D.Raycast(transform.position, Vector3.down, maxRayLength, groundLayer.value);
    }
}
