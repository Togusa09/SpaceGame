using Assets.Scripts;
using Assets.Scripts.UI;
using Scripts.UI;
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
        moveDisk.SetDiskMode(MoveDisk.DiskMode.Horizontal);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool(UIAnimationControlParameters.ShowDisk, false);
        }

        var control = animator.GetComponent<Control>();
        var moveDisk = control.GetMoveDisk();

        if (Input.GetMouseButtonDown(0))
        {
            StartDebounce();
        }

        if ((Input.GetMouseButton(0) && HasDebounceCompleted()) || Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveDisk.SetDiskMode(MoveDisk.DiskMode.Vertical);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveDisk.SetDiskMode(MoveDisk.DiskMode.Horizontal);
        }

        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool(UIAnimationControlParameters.ShowDisk, false);
            animator.SetBool(UIAnimationControlParameters.Moving, true);
            moveDisk.Deactivate();
        }
    }

    private float _mouseButtonDownTime;
    private float _mouseButtonDebounceInterval = 0.2f;

    private void StartDebounce()
    {
        _mouseButtonDownTime = Time.time;
    }
    private bool HasDebounceCompleted()
    {
        return Time.time - _mouseButtonDownTime > _mouseButtonDebounceInterval;
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
