using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeronIdleState : StateMachineBehaviour
{
    //Vector3 dist = Vector3.Distance(this.transform.position, player.transform.position);
    // Use dist to get the distance between player and boss then use that to determine states

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }
}
