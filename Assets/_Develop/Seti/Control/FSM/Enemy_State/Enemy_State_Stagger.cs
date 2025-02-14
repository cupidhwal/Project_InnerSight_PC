using System;
using UnityEngine;

namespace Seti
{
    public class Enemy_State_Stagger : Enemy_State
    {
        // 오버라이드
        #region Override
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized()
        {
            base.OnInitialized();

            // 다른 행동을 참조해야 한다면 이런 양식으로 작성
            /*if (context.BehaviourMap.TryGetValue(typeof(Attack), out var attackBehaviour))
                attack = attackBehaviour as Attack;*/
        }

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter()
        {
            base.OnEnter();
            elapsedDuration = context.Actor.Stagger;
            context.Actor.Condition.IsStagger = true;
            enemy.SwitchState(Enemy.State.Stagger);
        }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit()
        {
            base.OnExit();

            // 행동 종료를 명시적으로 작성해야 한다면 이와 같은 양식으로 작성
            //attack?.FSM_AttackInput(false);
        }

        // 상태 전환 조건 메서드
        public override Type CheckTransitions()
        {
            if (damagable.CurrentHitPoints <= 0)
                return typeof(Enemy_State_Dead);

            else if (enemy.Detected && context.StateMachine.ElapsedTime > elapsedDuration)
                return typeof(Enemy_State_Chase);

            else if (!enemy.Detected && enemy.GoBackHome && context.StateMachine.ElapsedTime > elapsedDuration)
                return typeof(Enemy_State_BackOff);

            else if (!enemy.Detected && context.StateMachine.ElapsedTime > elapsedDuration)
                return typeof(Enemy_State_Idle);

            else return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime)
        {
            // Stagger 행동 AI Input
            /*if (Input_Attack(deltaTime))
                attack?.FSM_AttackInput(true);

            if (!enemy.ActorCondition.IsAttack)
                look?.FSM_LookInput();*/
        }
        #endregion
    }
}