using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeronRise : StateMachineBehaviour
{
    public float moveSpeed = 1.5f;
    public float riseRange = 3f;
    Transform player;
    Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 target = new Vector2(rb.position.x, rb.position.y + 1);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, moveSpeed * Time.fixedDeltaTime);

        if (Vector2.Distance(player.position, rb.position) <= riseRange)
        {
            animator.SetTrigger("Idle");
            rb.MovePosition(newPos);
            Debug.Log("Idle");
        }
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }


}
