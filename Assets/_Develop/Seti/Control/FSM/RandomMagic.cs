using UnityEngine;

namespace Seti
{
    public class RandomMagic : StateMachineBehaviour
    {
        private Player player;
        [SerializeField]
        int RandomCount = 2;
        readonly int Hash_RandomMagic = Animator.StringToHash("RandomMagic");

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!animator.TryGetComponent<Actor>(out var actor))
                actor = animator.GetComponentInParent<Actor>();
            if (actor is not Player player) return;
            this.player = player;

            // 마법 사용 중에는 이동 금지
            player.Controller_Animator.CantMoveDurAtk();

            int rand = Random.Range(0, RandomCount);
            animator.SetInteger(Hash_RandomMagic, rand);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

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
}