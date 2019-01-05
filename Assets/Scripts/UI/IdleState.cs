using Assets.Scripts;
using Assets.Scripts.UI;
using UnityEngine;

public class IdleState : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var control = animator.GetComponent<Control>();

        if ((control.ClickHitState.Ship != null && control.ClickHitState.Ship.IsHostile) || control.AttackOverride)
        {
            control.ShowAttackCursor();
        }
        else
        {
            control.ShowDefaultCursor();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (control.AttackOverride && control.ClickHitState.Ship != null )
            {
                animator.SetBool(UIAnimationControlParameters.Attacking, true);
            }
            else
            {
                if (control.ClickHitState.Ship != null)
                {
                    SelectionManager.Instance.SelectShip(control.ClickHitState.Ship);
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (control.ClickHitState.Ship != null && !control.ClickHitState.Ship.IsFixed)
            {
                if (control.ClickHitState.Ship.IsHostile)
                {
                    animator.SetBool(UIAnimationControlParameters.Attacking, true);
                }
                else
                {
                    animator.SetBool(UIAnimationControlParameters.Moving, true);
                }

                control.MovementInformation.SetDestination(control.ClickHitState.Ship);
            }
            else
            {
                animator.SetBool(UIAnimationControlParameters.ShowDisk, true);
            }
        }
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
