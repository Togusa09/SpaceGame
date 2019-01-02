using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.UI;
using UnityEngine;

public class MoveState : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var selectionManager = SelectionManager.Instance;
        
        var control = animator.GetComponent<Control>();

        var movement = control.MovementInformation;
        switch (movement.MoveTargetType)
        {
            case MoveTargetType.Position:
                selectionManager.GetSelectedShip().MoveTo(movement.Location);
                break;
            case MoveTargetType.Ship:
                selectionManager.GetSelectedShip().MoveTo(movement.Ship.transform.position);
                break;
            case MoveTargetType.Target:
                selectionManager.GetSelectedShip().MoveTo(movement.Target.transform.position);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        animator.SetBool("Moving", false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
