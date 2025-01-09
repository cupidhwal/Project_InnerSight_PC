using JungBin;
using UnityEngine;

public enum BossState
{
    None,
    Attack
}

public class AxeEnable : StateMachineBehaviour
{

    public string enterParameter;
    public string exitParameter;

    public BossState enterState;
    public BossState exitState;

    public bool enterBool;
    public bool exitBool;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!string.IsNullOrEmpty(enterParameter))
        {
            animator.SetBool(enterParameter, enterBool);
        }

        if (enterState == BossState.Attack)
        {
            FirstBossController.isAttack = enterBool;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!string.IsNullOrEmpty(exitParameter))
        {
            animator.SetBool(exitParameter, exitBool);
        }

        if (exitState == BossState.Attack)
        {
            FirstBossController.isAttack = exitBool;
        }
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
