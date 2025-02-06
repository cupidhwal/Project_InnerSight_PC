using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 플레이어 공격 이펙트 플레이
    /// </summary>
    public class EllenStaffEffectSMB : StateMachineBehaviour
    {
        private Actor actor;
        public int effectIndex;         //이펙트 인덱스 지정
        
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            actor = animator.GetComponentInParent<Actor>();

            if (actor is Player)
            {
                //지정 이펙트 애니메이션 플레이
                Weapon weapon = actor.GetComponentInChildren<Weapon>();
                weapon.effects[effectIndex].Activate();
            }
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (actor)
            {
                actor.Controller_Animator.CantMoveDurAtk();
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (actor)
            {
                actor.Controller_Animator.CanMoveAfterAtk();
                actor.Controller_Animator.MeleeAttackEnd();
            }
        }
    }
}