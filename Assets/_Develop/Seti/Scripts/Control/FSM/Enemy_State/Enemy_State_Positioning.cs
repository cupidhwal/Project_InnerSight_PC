using System;
using UnityEngine;

namespace Seti
{
    public class Enemy_State_Positioning : Enemy_State
    {
        bool positioningCompleted = false;

        // 오버라이드
        #region Override
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized() => base.OnInitialized();

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter()
        {
            base.OnEnter();

            enemy.Agent.speed = enemy.Rate_Movement;
            enemy.Agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            enemy.OnTargetMove += PathFindToPositioning;
            PathFindToPositioning();

            positioningCompleted = false;
            context.Actor.Condition.IsMove = true;
        }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit()
        {
            base.OnExit();

            try
            {
                if (enemy.Agent.enabled)
                    enemy.Agent.ResetPath();
            }
            catch { }
            enemy.Agent.speed = enemy.Rate_Movement * enemy.Magnification_WalkToRun;
            enemy.OnTargetMove -= PathFindToPositioning;
        }

        // 상태 전환 조건 메서드
        public override Type CheckTransitions()
        {
            if (damagable.CurrentHitPoints <= 0)
                return typeof(Enemy_State_Dead);

            if (!condition.InAction)
                return typeof(Enemy_State_Stagger);

            if (positioningCompleted)
                return typeof(Enemy_State_Attack_Magic);

            if (!enemy.CanMagic)
                return typeof(Enemy_State_Patrol);

            if (enemy.Detected)
                return typeof(Enemy_State_Chase);

            return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime)
        {
            IsPositioning();
        }
        #endregion

        // 메서드
        #region Methods
        private void PathFindToPositioning()
        {
            // 현재 플레이어의 위치(Vector2)
            Vector2 currentPlayerPos = new(enemy.Player.transform.position.x, enemy.Player.transform.position.z);

            float rad = Mathf.Deg2Rad * UnityEngine.Random.Range(0, 360);

            Vector2 tempTarget = MathUtility.GetCirclePos(currentPlayerPos, enemy.MagicRange - 1f, rad);
            Vector3 targetPos = new(tempTarget.x, enemy.transform.position.y, tempTarget.y);

            // 해당 위치로 이동
            condition.IsPositioning = true;
            enemy.Agent.SetDestination(targetPos);

            //// 현재 플레이어의 위치(Vector2)
            //Vector2 currentPlayerPos = new(enemy.Player.transform.position.x, enemy.Player.transform.position.z);

            //// 기준 벡터 - 현재 플레이어의 위치로부터 rad = 0, dis = MagicRange - 1f인 점의 좌표
            //Vector2 criteria = currentPlayerPos - MathUtility.GetCirclePos(currentPlayerPos, enemy.MagicRange - 1f, 0);

            //// 현재 Player로부터 Enemy의 방향
            //Vector3 temp = enemy.transform.position - enemy.Player.transform.position;
            //Vector2 dir = new(temp.x, temp.z);

            //// 기준점과 방향의 사잇각
            //float tempRad = Vector2.Angle(criteria, dir);

            //// 포지셔닝은 rad +- 10~30도로 한다
            //float rad1 = Mathf.Deg2Rad * UnityEngine.Random.Range(tempRad - 30, tempRad - 10);
            //float rad2 = Mathf.Deg2Rad * UnityEngine.Random.Range(tempRad + 10, tempRad + 30);
            //int rand = UnityEngine.Random.Range(0, 2);
            //float rad = rand == 0 ? rad1 : rad2;

            //// 포지셔닝 할 위치
            //Vector2 tempTargetPos = MathUtility.GetCirclePos(currentPlayerPos, enemy.MagicRange - 1f, rad);
            //Vector3 targetPos = new(tempTargetPos.x, enemy.transform.position.y, tempTargetPos.y);

            //// 해당 위치로 이동
            //condition.IsPositioning = true;
            //enemy.Agent.SetDestination(targetPos);
        }
        private void IsPositioning()
        {
            if (enemy.Agent.remainingDistance < 0.1f || enemy.Detected)
            {
                condition.IsPositioning = false;
                positioningCompleted = true;
                try
                {
                    if (enemy.Agent.enabled)
                        enemy.Agent.ResetPath();
                }
                catch { }

                context.Actor.Condition.IsMove = false;
            }
        }
        #endregion
    }
}