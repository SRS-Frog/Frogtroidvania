using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Yarn.Compiler.BasicBlock;

public class FrappleScript : MonoBehaviour
{
    // frapple variables
    private Rigidbody2D rb;
    private DistanceJoint2D rope; // store the distancejoint
    //private SpriteRenderer spriteRenderer; // store the spriterenderer
    private LineRenderer lineRenderer; // store the linerenderer

    //Daehyun variables
    private GameObject daehyun;

    // inspector variables
    [SerializeField] float frappleLength, frappleSpeed = 10f, retractSpeed = 30f, shortenSpeed = 0.5f; // retract speed for when nothing was hit, shorten acceleration for when hooked
    [SerializeField] private Vector2 offset = new Vector2(0, 1); // offset of frapple's starting point relative to character

    // changes on runtime
    private bool isLaunched = false;
    private bool isRetracting = false;
    private bool isHooked = false;
    private Vector3 targetPos;
    private Vector3 startingPos;


    // bandaid solution
    private HumanAttributes attributes;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // rigidbody
        rope = GetComponent<DistanceJoint2D>(); // distance joint
        rope.distance = frappleLength; // set the joint distance to frapple length

        //spriteRenderer = GetComponent<SpriteRenderer>(); // sprite renderer
        lineRenderer= GetComponent<LineRenderer>(); // line renderer

        Toggle(false); // toggle the frapple off

        // daehyun references
        daehyun = transform.parent.GetChild(0).gameObject; // get a reference to daehyun's game object

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
                rb.velocity = (startingPos - transform.position).normalized * retractSpeed; // move frapple toward starting position
            }
            else if (isLaunched) // frapple is being launched outward
            {
                rb.velocity = (targetPos - transform.position).normalized * frappleSpeed; // move frapple toward a position

                if (Vector2.Distance(transform.position, targetPos) <= 0.3f) // if the distance is small enough, target reached
                {
                    RetractFrapple(); // retract frapple
                }
            }
            else
            {
                transform.position = startingPos;
            }
        } else
        {
            // shorten the rope
            rope.distance -= shortenSpeed * Time.deltaTime;

            // if too close to the top, release
            float tooClose = 3.5f;
            Debug.Log("Distance between daehyun and target pos " + Vector2.Distance(targetPos, daehyunPos));
            if(Vector2.Distance(targetPos, daehyunPos) <= tooClose)
            {
                // move back to starting pos and toggle everything false
                transform.position = startingPos;
                Toggle(false); // set inactive once returned to player
                isRetracting = false; // no longer retracting the tongue
            }
        }
    }

    private void Toggle(bool toggle)
    {
        // deactivate components
        isLaunched = toggle; // toggle whether it is launched
        lineRenderer.enabled = toggle;
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
            Debug.Log("collided with " + collision);
            Frapple();
        }
        else if (isRetracting && collision.transform.CompareTag("Player"))
        {
            if (Vector2.Distance(startingPos, transform.position) < 0.5f) // if close enough to starting position
            {
                Toggle(false); // set inactive once returned to player
                isRetracting = false; // no longer retracting the tongue

                Debug.Log("fully retracted");
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

        // rope distance is distance from hooked point to the player's position (or maximum of rope length)
        rope.distance = Mathf.Clamp(Vector2.Distance(targetPos, daehyun.transform.position), 0f, frappleLength);

        //bandaid solution
        attributes.isHooked = isHooked; // this is only changed when the frapple is hooked, and turned off when character hits the ground
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
            transform.position = startingPos; // move to the starting position

            // clamp frapple to max length
            if (Vector2.Distance(targetPos, transform.position) >= frappleLength) // if the point is too far
            {
                // shoot frapple in the direction of that point, but not exceeding the frapple length
                Vector3 distanceBtwn = targetPos - transform.position; // the vector between the two points
                distanceBtwn = Vector3.ClampMagnitude(distanceBtwn, frappleLength - 0.2f); // clamp the distance to slightly less than the maximum length
                targetPos = transform.position + distanceBtwn; //new target position to shoot toward
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

    ///// <summary>
    ///// Returns whether the frapple is currently hooked to an object.
    ///// </summary>
    ///// <returns>isHooked</returns>
    //public bool IsHooked()
    //{
    //    return isHooked;
    //}
}
