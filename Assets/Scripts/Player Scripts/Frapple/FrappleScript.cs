using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Yarn.Compiler.BasicBlock;

public class FrappleScript : MonoBehaviour
{
    // frapple variables
    private Rigidbody2D rb;
    private DistanceJoint2D distanceJoint; // store the distancejoint

    //Daehyun variables
    private GameObject daehyun;

    // inspector variables
    [SerializeField] float frappleLength;
    [SerializeField] float frappleSpeed = 30f;
    [SerializeField] private Vector2 offset = new Vector2(0, 1); // offset of frapple's starting point relative to character

    // changes on runtime
    // private enum States { launched, retracting }; // UNUSED RN
    private bool launched = false;
    private bool retracting = false;
    private Vector3 targetPos;
    private Vector3 startingPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // rigidbody
        distanceJoint = GetComponent<DistanceJoint2D>(); // distance joint
        distanceJoint.enabled = false; // disable initially
        distanceJoint.distance = frappleLength; // set the joint distance to frapple length

        // daehyun references
        daehyun = transform.parent.GetChild(0).gameObject; // get a reference to daehyun's game object
    }

    private void FixedUpdate()
    {
        Vector2 daehyunPos = new Vector2(daehyun.transform.position.x, daehyun.transform.position.y);
        startingPos = daehyunPos + offset;

        if (retracting) // if frapple is in retracting state
        {
            rb.velocity = (startingPos - transform.position).normalized * frappleSpeed; // move frapple toward starting position
        } else if (launched) // frapple is being launched outward
        {
            rb.velocity = (targetPos - transform.position).normalized * frappleSpeed; // move frapple toward a position

            if (Vector2.Distance(transform.position, targetPos) <= 0.1) // if the distance is small enough, target reached
            {
                retracting = true; // retract frapple
            }
        } 
    }

    private void Toggle(bool toggle)
    {
        launched = toggle; // toggle whether it is launched
        transform.gameObject.SetActive(toggle);
        distanceJoint.enabled = toggle;

        if (toggle) // if activate
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // set rb to dynamic (can move)
        } else
        {
            rb.bodyType = RigidbodyType2D.Kinematic; // set rb to kinematic (can't move)
        }
    }

    /// <summary>
    /// Shoots the frapple toward a target position. Called from FrappleController.
    /// </summary>
    /// <param name="pos"></param> the position to shoot the frapple to
    public void ShootFrapple(Vector2 pos)
    {
        if(!launched) // if not already launched, prevents spamming
        {
            targetPos = pos; // implicitly convert vector 2 to vector 3 (so it's easier to compare to the transform)
            transform.position = startingPos; // move to the starting position

            // clamp frapple to max length
            if (Vector2.Distance(targetPos, transform.position) >= frappleLength) // if the point is too far
            {
                // shoot frapple in the direction of that point, but not exceeding the frapple length
                Vector3 distanceBtwn = targetPos - transform.position; // the vector between the two points
                distanceBtwn = Vector3.ClampMagnitude(distanceBtwn, frappleLength - 0.5f); // clamp the distance to slightly less than the maximum length, so player isn't pulled along
                targetPos = transform.position + distanceBtwn; //new target position to shoot toward
            }
            Toggle(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Frappable")) // if it is frappable
        {
            Toggle(false); // stop the frapple
            Frapple();
        }
        else if (collision.transform.CompareTag("Player"))
        {
            if (Vector2.Distance(startingPos, transform.position) < 0.1f) // if close enough to starting position
            {
                Toggle(false); // set inactive once returned to player
                retracting = false; // no longer retracting the tongue
            }
        }
        else
        {
            retracting = true;
        }
    }

    private void Frapple() // frapple interactions
    {
        Toggle(false);
    }
}
