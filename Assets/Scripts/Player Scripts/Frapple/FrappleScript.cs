using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrappleScript : MonoBehaviour
{
    // frapple variables
    private Rigidbody2D rb;
    private DistanceJoint2D distanceJoint; // store the distancejoint

    //Daehyun variables
    private GameObject daehyun;

    // inspector variables
    [SerializeField] float frappleSpeed = 30f;
    [SerializeField] private Vector2 offSet = new Vector2(0, 1); // offset of frapple's starting point relative to character

    // changes on runtime
    private bool launched = false;
    private Vector3 targetPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // rigidbody
        distanceJoint = GetComponent<DistanceJoint2D>(); // distance joint
        distanceJoint.enabled = false; // disable initially

        // daehyun references
        daehyun = transform.parent.GetChild(0).gameObject; // get a reference to daehyun's game object
    }

    private void FixedUpdate()
    {
        if (launched)
        {
            rb.velocity = (targetPos - transform.position).normalized * frappleSpeed; // move frapple toward a position

            if (Vector2.Distance(transform.position, targetPos) <= 1) // if the distance is small enough
            {
                RetractFrapple(); // move the frapple back
            }

            //TODO: DIFFERENCE BETWEEN REACHED TARGET VS NEED TO RETRACT
        }

    }

    private void Toggle(bool toggle)
    {
        launched = toggle; // it is now launched
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
        targetPos = pos; // implicitly convert vector 2 to vector 3 (so it's easier to compare to the transform)
        Toggle(true);
    }

    private void RetractFrapple() // retract frapple to original location
    {
        Vector2 daehyunPos = new Vector2(daehyun.transform.position.x, daehyun.transform.position.y);
        ShootFrapple(daehyunPos + offSet); // return to offset
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("triggering");
        Toggle(false); // stop the frapple
        if (collision.transform.CompareTag("Frappable")) // if it is frappable
        {
            Frapple();
        }
        else if (collision.transform.CompareTag("Player"))
        {
            Vector2 currOffset = daehyun.transform.position - transform.position;
            if(Vector2.Distance(currOffset, offSet) < 0.1) // if close enough to starting offset
            {
                Toggle(false); // set inactive once returned to player
            }
        }
        else
        {
            RetractFrapple();
        }
    }

    private void Frapple() // frapple interactions
    {
        Toggle(false);
    }
}
