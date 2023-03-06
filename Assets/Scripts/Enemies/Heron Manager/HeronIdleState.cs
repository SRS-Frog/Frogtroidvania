using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeronIdleState : StateMachineBehaviour
{
    Transform player; 
    Rigidbody2D rb;
    //public float speed = 2f;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       player = GameObject.FindGameObjectWithTag("Player").transform;
       rb = animator.GetComponent<Rigidbody2D>();
       
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Vector2 risenPosition = new Vector2(rb.position.x, rb.position.y + 3f);
        float playerDistance = Vector2.Distance(player.position, rb.position);
        //Debug.Log(playerDistance);

       if (playerDistance <= 4f)
       {
        animator.SetBool("riseState", true);
        Debug.Log("rise successful");
       }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.ResetTrigger("Dive");
    }
}
