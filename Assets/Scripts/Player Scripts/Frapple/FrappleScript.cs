using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Yarn.Compiler.BasicBlock;

public class FrappleScript : MonoBehaviour
{
    // frapple variables
    private Rigidbody2D rb;
    private DistanceJoint2D rope; // store the distancejoint
    private SpriteRenderer spriteRenderer; // store the spriterenderer
    private LineRenderer lineRenderer; // store the linerenderer

    //Daehyun variables
    private GameObject daehyun;
    private Rigidbody2D daehyunRB;

    // inspector variables
    [SerializeField] float frappleLength, frappleSpeed = 10f, retractSpeed = 30f, shortenSpeed = 0.5f; // retract speed for when nothing was hit, shorten acceleration for when hooked
    [SerializeField] private Vector2 offset = new Vector2(0, 1); // offset of frapple's starting point relative to character

    // changes on runtime
    private bool isLaunched = false;
    private bool isRetracting = false;
    private bool isHooked = false;
    private Vector2 targetPos;
    private Vector2 startingPos;


    // bandaid solution
    private HumanAttributes attributes;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // rigidbody
        rope = GetComponent<DistanceJoint2D>(); // distance joint
        rope.distance = frappleLength; // set the joint distance to frapple length

        spriteRenderer = GetComponent<SpriteRenderer>(); // sprite renderer
        lineRenderer= GetComponent<LineRenderer>(); // line renderer

        Toggle(false); // toggle the frapple off

        // daehyun references
        daehyun = transform.parent.GetChild(0).gameObject; // get a reference to daehyun's game object
        daehyunRB = daehyun.GetComponent<Rigidbody2D>(); // daehyun's rigidbody

        //bandaid solution
        attributes = rope.connectedBody.GetComponent<HumanAttributes>(); // get the human attributes from the connected body
    }

    private void Update()
    {
        // render a line between the frapple end and the character
        Vector3[] positions = {startingPos, transform.position};
        lineRenderer.SetPositions(positions); 
    }

    private void FixedUpdate()
    {
        Vector2 daehyunPos = new Vector2(daehyun.transform.position.x, daehyun.transform.position.y);
        startingPos = daehyunPos + offset;

        if (!isHooked)
        {
            if (isRetracting) // if frapple is in retracting state
            {
                rb.velocity = (startingPos - rb.position).normalized * retractSpeed; // move frapple toward starting position
            }
            else if (isLaunched) // frapple is being launched outward
            {
                rb.velocity = (targetPos - rb.position).normalized * frappleSpeed; // move frapple toward a position

                if (Vector2.Distance(rb.position, targetPos) <= 0.3f) // if the distance is small enough, target reached
                {
                    RetractFrapple(); // retract frapple
                }
            }
            else
            {
                rb.position = startingPos;
            }
        } else
        {
            // shorten the rope
            rope.distance -= shortenSpeed * Time.deltaTime;

            // if too close to the top or daehyun is above the frapple, release
            float tooClose = 1.5f;
            Debug.Log("Distance between daehyun and target pos " + Vector2.Distance(targetPos, daehyunPos));
            if (Vector2.Distance(rb.position, startingPos) <= tooClose || startingPos.y > rb.position.y)
            {
                ReturnToStartPos();
            }
        }
    }

    private void Toggle(bool toggle)
    {
        // deactivate components
        isLaunched = toggle; // toggle whether it is launched
        lineRenderer.enabled = toggle;
        spriteRenderer.enabled = toggle;
        rope.enabled = false;

        if (toggle) // if activate
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // set rb to dynamic (can move)
        } else
        {
            rb.bodyType = RigidbodyType2D.Static; // set rb to static (can't move)
        }
    }

    // change the state of the frapple when trigger enters or stays
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("Stay " + collision);
        FrappleStateChange(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Enter " + collision);
        FrappleStateChange(collision);
    }

    // frapples if collides with a frappable block, turns off if collides with player, retracts if it collides with anything not frappable
    private void FrappleStateChange(Collider2D collision)
    {
        if (!isRetracting && isLaunched && collision.transform.CompareTag("Frappable")) // if it is frappable
        {
            // Debug.Log("collided with " + collision);
            Frapple();
        }
        else if (isRetracting && collision.transform.CompareTag("Player"))
        {
            if (Vector2.Distance(startingPos, rb.position) < 0.5f) // if close enough to starting position
            {
                ReturnToStartPos();
                // Debug.Log("fully retracted");
            }
        }
        else if (!collision.transform.CompareTag("Frappable") && !collision.transform.CompareTag("Player"))
        {
            RetractFrapple();
        }
    }

    private void Frapple() // frapple interactions
    {
        Debug.Log("frapple");
        isLaunched = false;
        isRetracting = false;
        isHooked = true;
        rope.enabled = true;
        rb.bodyType = RigidbodyType2D.Static; // stop moving

        // rope distance is distance from hooked point to the startingpos (or maximum of rope length)
        rope.distance = Mathf.Clamp(Vector2.Distance(targetPos, startingPos), 0f, frappleLength);

        //bandaid solution
        attributes.isHooked = isHooked; // this is only changed when the frapple is hooked, and turned off when character hits the ground
    }

    // returns the frapple to starting position WITHOUT retraction animation
    private void ReturnToStartPos()
    {
        // move back to starting pos and toggle everything false
        rb.position = startingPos;
        Toggle(false); // set inactive once returned to player
        isRetracting = false; // no longer retracting the tongue
    }

    /// <summary>
    /// Shoots the frapple toward a target position. Called from FrappleController.
    /// </summary>
    /// <param name="pos"></param> the position to shoot the frapple to
    public void ShootFrapple(Vector2 pos)
    {
        isHooked = false;

        if (!isLaunched) // if not already launched, prevents spamming
        {
            targetPos = pos; // implicitly convert vector 2 to vector 3 (so it's easier to compare to the transform)
            rb.position = startingPos; // move to the starting position

            // CHEATING ON PLAYER'S BEHALF, FOR SMOOTHER FRAPPLE
            Vector3 virtualStartingPos = startingPos; // starting pos to be modified if player is moving
            if (Mathf.Abs(daehyunRB.velocity.x) > 2f && daehyunRB.velocity.y > 1f) // if daehyun is moving a particular direction at a certain speed in the air
            {
                // Debug.Log("Cheating");
                Vector2 veloc = daehyunRB.velocity;

                float cheatFactor = 0.5f;
                targetPos = new Vector2((veloc.x + targetPos.x) * cheatFactor, targetPos.y); // move the target position slightly to match the velocity
                virtualStartingPos = new Vector2((veloc.x + targetPos.x) * cheatFactor, startingPos.y); // moving starting pos according to daehyun's velocity
            }

            // clamp frapple to max length
            if (Vector2.Distance(targetPos, virtualStartingPos) >= frappleLength) // if the point is too far
            {
                // shoot frapple in the direction of that point, but not exceeding the frapple length
                Vector2 distanceBtwn = targetPos - rb.position; // the vector between the two points
                distanceBtwn = Vector3.ClampMagnitude(distanceBtwn, frappleLength - 0.2f); // clamp the distance to slightly less than the maximum length
                targetPos = rb.position + distanceBtwn; //new target position to shoot toward
            }
            Toggle(true);
        }
    }

    /// <summary>
    /// Retracts the frapple.
    /// </summary>
    public void RetractFrapple()
    {
        isRetracting = true;
        isLaunched = false;
        isHooked = false;
        rope.enabled = false;
        rb.bodyType = RigidbodyType2D.Dynamic; // start moving again
    }

    /// <summary>
    /// Whether this distance is frappable.
    /// </summary>
    /// <param name="point">The point to compare the distance to.</param>
    /// <returns>bool of whether it is frappable</returns>
    public bool Frappable(Vector2 point)
    {
        return (Vector2.Distance(point, startingPos) <= frappleLength);
    }

    ///// <summary>
    ///// Returns whether the frapple is currently hooked to an object.
    ///// </summary>
    ///// <returns>isHooked</returns>
    //public bool IsHooked()
    //{
    //    return isHooked;
    //}
}
