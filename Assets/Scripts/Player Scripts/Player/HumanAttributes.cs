using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAttributes : MonoBehaviour
{
    public Rigidbody2D rb; // the Rigidbody2D component of the player 
    //private PlayerStates playerStates;
    //private PlayerAttributes playerAttributes;
    //private PlayerController playerController;

    //private float horDir; // -1 means move left, 1 means move right, 0 means stop
    //private float vertDir; // 1 means move up, 0 means stop
    public float moveSpeed = 5.59f; // horizontal speed of player. ////We can edit this value later in the Unity editor.

    ////add these after you have showed movement left and right on Unity
    public float jumpForce = 500f; // jump force of player

                            ////you could also type public float jumpForce
    private bool jump; // true if player is on the ground and about to jump, false otherwise
    public bool isGrounded; // true if player is touching the ground
    public Transform GroundCheck; // The position of a GameObject that will mark the player's feet
    public LayerMask groundLayer; // determines which layers count as the ground

    [HideInInspector] public bool facingRight = true; // true if player is facing right

    private bool isDead = false; // true if player is dead

    private RaycastHit2D hit;

    [SerializeField] private float maxRayLength;
    [SerializeField] private float plungeSpeed;


    //attacking
    public GameObject attackArea;
    public float timeToAttack = 0.15f;

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
        attackArea = transform.GetChild(1).gameObject;
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
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, 0.05f, groundLayer);
        //Debug.Log("isGrounded " + isGrounded);

        hit = Physics2D.Raycast(transform.position, Vector3.down, maxRayLength, groundLayer.value);
    }
}
