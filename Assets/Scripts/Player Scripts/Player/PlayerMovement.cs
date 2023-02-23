using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; ////

/* PlayerMovement.cs
   
   This is the code for player movement: moving left and right, jumping, flipping the player, dying

   Animations for moving are connected to the code here

   Author: Grace

*/

public class PlayerMovement : MonoBehaviour
{
    //// General: if there is no modifier before a field or method, it is default private. 
    //// By making a field public, we will be able to edit what is stored in it through the Unity editor.
    //// [SerializeField] makes private variables visible in Unity editor

    private Rigidbody2D rb; // the Rigidbody2D component of the player 
    private PlayerStates playerStates;
    private PlayerAttributes playerAttributes;
    private PlayerController playerController;

    private float horDir; // -1 means move left, 1 means move right, 0 means stop
    private float vertDir; // 1 means move up, 0 means stop
    public float moveSpeed = 5.59f; // horizontal speed of player. ////We can edit this value later in the Unity editor.

    ////add these after you have showed movement left and right on Unity
    [SerializeField] private float jumpForce = 500f; // jump force of player

                            ////you could also type public float jumpForce
    private bool jump; // true if player is on the ground and about to jump, false otherwise
    private bool isGrounded; // true if player is touching the ground
    public Transform GroundCheck; // The position of a GameObject that will mark the player's feet
    public LayerMask groundLayer; // determines which layers count as the ground

    private bool facingRight = true; // true if player is facing right

    private bool isDead = false; // true if player is dead

    //public Animator animator; // the animator of the player
    public GameObject player; // the player
    public GameObject lowestBound; // an empty GameObject that marks the lowest point in your game

    ////this is similar to the Start() method; both are normally used to initialize variables.
    ////there are differences, but they are not important for this class
    // intialize variables
    void Awake()
    {
        // find the Rigidbody2D component of the object that this script is attached to
        rb = GetComponent<Rigidbody2D>();
        playerStates = GetComponent<PlayerStates>();
        playerAttributes = GetComponent<PlayerAttributes>();
        playerController = GetComponent<PlayerController>();
    }

    // similar to Update(), but is better for physics and movement
   /* private void FixedUpdate()
    {
        // checks if you are within 0.05 position in the Y of the ground layer
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, 0.05f, groundLayer);

        if(!isDead) 
            Move(); 
        else
            Die();
    }*/

    // deals with the the velocity of the player, and calls Flip() and Jump() when applicable
    public void Move(int dir)
    {
        // changes horizontal velocity of player
        ////Time.deltaTime makes the speed more constant between different computers with different frames per second

        rb.velocity = new Vector2(dir * moveSpeed * Time.deltaTime, rb.velocity.y);

        if(rb.velocity.x != 0)
            playerStates.SetIsMoving(true);
        else 
            playerStates.SetIsMoving(false);

        // flip the player if needed
        if ((facingRight && dir == -1) || (!facingRight && dir == 1))
            Flip();

        
    }

    // add a vertical force to the player
    public void Jump()
    {
        rb.AddForce(new Vector2(0f, jumpForce));
    }

    // flip the player
    private void Flip()
    {
        facingRight = !facingRight;

        // flips the sprite AND its colliders
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Plunge()
    {
        rb.velocity = new Vector2(0, -playerAttributes.GetPlungeSpeed() * Time.deltaTime);
    }

    // detects if the player has collided with an enemy
    private void OnTriggerEnter2D(Collider2D other)
    {
        /*if (other.gameObject.CompareTag("Enemy"))
        {
            //animator.SetBool("IsDead", true);
            isDead = true;
        }*/
    }
    
    // stop the player midair when it dies
    /*public void Stasis()
    {
        rb.velocity = Vector2.zero; // set player's velocity to zero
        rb.gravityScale = 0;
    }*/

    // the player dies --> reload scene, reset score
    public void Die()
    {
        // reset score
        //ScoreManager.instance.ResetScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
