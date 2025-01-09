using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace JungBin
{
    /// <summary>
    /// 보스의 애니메이션을 제외한 기능 구현
    /// </summary>
    public class FirstBossController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Transform player_Transform;    //플레이어의 위치
        [SerializeField] private GameObject attackBox;      //보스의 공격 박스
        private Animator m_Animator;


        [SerializeField] private float turnSpeed = 1200;    //보스의 회전 속도

        public static bool isAttack { get; set; } = false; // 공격중인지 여부
        public static bool isDashing { get; set; } = false; // 돌진중인지 여부
        private int lastAttack = -1; //직전 공격 패턴
        #endregion

        private void Start()
        {
            m_Animator = this.GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            //플레이어와의 거리
            Vector3 direction = player_Transform.position - transform.position;
            direction.y = 0;
            float distance = direction.magnitude;

            if (!isAttack)
            {
                TurnBossToPlayer(direction);
            }

            CloseDistanceToPlayer(distance);

            if (m_Animator.GetBool("Idle") == true && m_Animator.GetBool("IsFar") == false)
            {
                NextAttack();
            }
            AttackBoxActive();

        }

        //보스의 회전 관련 코드
        public void TurnBossToPlayer(Vector3 direction)
        {
            // 플레이어의 방향 계산
            Vector3 dir = direction.normalized;

            // 현재 방향과 목표 방향을 회전 값으로 변환
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // 현재 회전을 목표 회전으로 보간
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }

        //일정 거리 이상 멀어졌을때 보스가 간격을 좁히는 코드
        private void CloseDistanceToPlayer(float distance)
        {
            if (distance > 15f)
            {
                m_Animator.SetBool("IsFar", true);
            }
            else if (distance < 5f)
            {
                m_Animator.SetBool("IsFar", false);
            }
        }

        //다음 공격 정하는 코드
        public void NextAttack()
        {
            if (m_Animator == null)
                return;

            int attackIndex;

            //첫번째 공격인지 확인
            if (lastAttack == -1)
            {
                //첫 공격일경우 3개의 공격중 하나 랜덤
                attackIndex = Random.Range(1, 4);
            }
            else
            {
                do
                {
                    attackIndex = Random.Range(1, 4);
                } while (lastAttack == attackIndex);
            }

            //선택된 공격 애니메이션 실행
            TriggerAttackAnimation(attackIndex);

            //중복 공격 방지
            lastAttack = attackIndex;

            m_Animator.SetBool("Idle", false);
        }

        // 공격 애니메이션 호출
        private void TriggerAttackAnimation(int attackIndex)
        {
            string triggerName = $"Attack0{attackIndex}";
            m_Animator.SetTrigger(triggerName);
        }

        private void AttackBoxActive()
        {
            if (m_Animator.GetBool("IsAttack02") == true)
            {
                attackBox.SetActive(true);
            }
            else
            {
                attackBox.SetActive(false);
            }
        }

    }
}