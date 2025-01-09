using UnityEngine;

namespace Seti
{
    public abstract class Player_Base_AniState : MonoState<Controller_Animator>
    {
        // animator parameter
        protected int WhichAttack = Animator.StringToHash("WhichAttack");
        protected int isAttack = Animator.StringToHash("IsAttack");
        protected int isDeath = Animator.StringToHash("IsDeath");
        protected int isMove = Animator.StringToHash("IsMove");
        protected int isDash = Animator.StringToHash("IsDash");
    }
}