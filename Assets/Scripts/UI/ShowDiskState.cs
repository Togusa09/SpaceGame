﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class ShowDiskState : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var control = animator.GetComponent<Control>();
        var moveDisk = control.GetMoveDisk();
        var selectionManager = SelectionManager.Instance;
        moveDisk.Activate(selectionManager.GetSelectedShip().gameObject);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool("ShowDisk", false);
        }

        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("ShowDisk", false);
            animator.SetBool("Moving", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var control = animator.GetComponent<Control>();
        var moveDisk = control.GetMoveDisk();
        var position = moveDisk.HitPoint;
        control.MovementInformation.SetDestination(position);
        moveDisk.Deactivate();
    }

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
