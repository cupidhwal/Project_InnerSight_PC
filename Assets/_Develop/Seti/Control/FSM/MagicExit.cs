using UnityEngine;

namespace Seti
{
    public class MagicExit : StateMachineBehaviour
    {
        private Player player;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!animator.TryGetComponent<Actor>(out var actor))
                actor = animator.GetComponentInParent<Actor>();
            if (actor is not Player player) return;
            this.player = player;
            
            // 마법 사용 중에는 이동 금지
            player.Controller_Animator.CantMoveDurAtk();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (player)
            {
                player.Controller_Animator.CanMoveAfterAtk();
            }

            if (!animator.TryGetComponent<Controller_Base>(out var controller))
                controller = animator.GetComponentInParent<Controller_Base>();

            if (controller.BehaviourMap.TryGetValue(typeof(Attack), out var attackBehaviour))
            {
                if (attackBehaviour is Attack attack)
                {
                    attack.MagicExit();
                }
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