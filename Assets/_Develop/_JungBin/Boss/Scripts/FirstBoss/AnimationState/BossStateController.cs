using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace JungBin
{

    public enum BossState
    {
        None,
        Attack
    }

    public class BossStateController : StateMachineBehaviour
    {
        [Header("파라미터 설정"), SerializeField]
        public string enterParameter;
        public string exitParameter;

        public bool enterParameterBool;
        public bool exitParameterBool;

        [Header("FirstBossManager 인트턴스 설정"), SerializeField]
        public BossState enterStateFir;
        public BossState exitStateFir;

        public bool enterBoolFir;
        public bool exitBoolFir;

        [Header("SecondBossManager 인트턴스 설정"), SerializeField]
        public BossState enterStateSec;
        public BossState exitStateSec;

        public bool enterBoolSec;
        public bool exitBoolSec;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!string.IsNullOrEmpty(enterParameter))
            {
                animator.SetBool(enterParameter, enterParameterBool);
            }

            if (enterStateFir == BossState.Attack)
            {
                FirstBossManager.isAttack = enterBoolFir;
            }

            if (enterStateSec == BossState.Attack)
            {
                SecondBossManager.isAttack = enterBoolSec;
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
                animator.SetBool(exitParameter, exitParameterBool);
            }

            if (exitStateFir == BossState.Attack)
            {
                FirstBossManager.isAttack = exitBoolFir;
            }

            if (exitStateSec == BossState.Attack)
            {
                SecondBossManager.isAttack = exitBoolSec;
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
}