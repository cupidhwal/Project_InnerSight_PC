using Noah;
using UnityEngine;

namespace Seti
{
    public class Condition_Player : Condition_Actor
    {
        // 필드
        private Move move;
        public bool CanDash { get; set; } = true;
        public bool IsDash { get; set; } = false;

        // 라이프 사이클
        #region Life Cycle
        protected override void Start()
        {
            base.Start();

            if (TryGetComponent<Damagable>(out var damagable))
                damagable.OnDeath += ReviveInvoke;
        }

        private void OnEnable()
        {
            StageManager.Instance.stageStartEvent += DisablePlayer;
            StageManager.Instance.stageEndEvent += EnablePlayer;
        }

        private void OnDisable()
        {
            //if (StageManager.Instance.stageStartEvent != null)
            //    StageManager.Instance.stageStartEvent -= DisablePlayer;

            //if (StageManager.Instance.stageEndEvent != null)
            //    StageManager.Instance.stageEndEvent -= EnablePlayer;
        }
        #endregion

        // 메서드
        #region Methods
        protected override void Die()
        {
            base.Die();
            //Destroy(gameObject, 2);
        }

        private void ReviveInvoke()
        {
            Invoke("Revive", 7);
        }

        public void Revive()
        {
            IsDead = false;
            inAction = true;

            Actor.Controller_Animator.Animator.Rebind();
        }

        public override void Initialize()
        {
            // 저장된 현재 장비
            primaryWeaponType = WeaponType.Sword;

            base.Initialize();

            // 초기 장비 설정
            ChangeWeapon(primaryWeaponType);

            if (Actor.Controller.BehaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
                if (moveBehaviour is Move)
                    move = moveBehaviour as Move;
        }

        // 플레이어 제어권 여부
        public void EnablePlayer() => PlayerSetActive(true);
        public void DisablePlayer() => PlayerSetActive(false);
        public void PlayerSetActive(bool inAction)
        {
            Debug.Log($"inAction : {inAction}");

            this.inAction = inAction;
            CanMove = inAction;
            StopRigidBody();

            // 제어권 박탈 해제 시 초기화
            if (inAction)
            {
                IsGrounded = true;
                IsStagger = false;
                IsAttack = false;
                IsMagic = false;
                IsMove = false;
                IsDash = false;
                CanDash = true;

                //move.OnMove(Vector2.zero, false);
                //Actor.CoroutineStopper();
            }
        }

        // 초기 장비 설정
        public void ChangeWeapon(WeaponType weaponType)
        {
            currentWeaponType = weaponType;
            switch (weaponType)
            {
                case WeaponType.Sword:
                    currentWeapon = primaryWeapon;
                    break;
            }
        }
        #endregion
    }
}