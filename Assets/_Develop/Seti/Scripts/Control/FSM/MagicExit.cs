using UnityEngine;

namespace Seti
{
    public class MagicExit : StateMachineBehaviour
    {
        private Actor actor;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!animator.TryGetComponent<Actor>(out var actor))
                actor = animator.GetComponentInParent<Actor>();
            this.actor = actor;

            // 마법 사용 중에는 이동 금지
            actor.Controller_Animator.CantMoveDurAtk();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // 마법 사용 중에는 이동 금지
        //    //actor.Controller_Animator.CantMoveDurAtk();
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (actor)
            {
                actor.Controller_Animator.CanMoveAfterAtk();
            }

            if (!animator.TryGetComponent<Controller_Base>(out var controller))
                controller = animator.GetComponentInParent<Controller_Base>();

            actor.Condition.IsMagic = false;
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