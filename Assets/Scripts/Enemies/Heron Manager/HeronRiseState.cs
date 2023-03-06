using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeronRiseState : StateMachineBehaviour
{
    Transform player; 
    Rigidbody2D rb;
    public float speed = 2.5f;
    public static float rand;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       player = GameObject.FindGameObjectWithTag("Player").transform;
       rb = animator.GetComponent<Rigidbody2D>();
       
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 risenPosition = new Vector2(rb.position.x, rb.position.y + 1f);
        float playerDistance = Vector2.Distance(player.position, rb.position);
        Vector2 newPos = Vector2.MoveTowards(rb.position, risenPosition, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
        rand = Random.value;
        //Debug.Log(rand);

       if (rand > 0.3)
       {
        animator.SetTrigger("Dive");
       }
       else if (playerDistance > 4f)
       {
        animator.SetBool("riseState", false);
        Debug.Log("un-rising");
       }

    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.ResetTrigger("Dive");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}


}
