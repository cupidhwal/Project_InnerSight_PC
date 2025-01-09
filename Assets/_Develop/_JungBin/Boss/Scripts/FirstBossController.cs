using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;

namespace JungBin
{
    /// <summary>
    /// 보스의 애니메이션을 제외한 기능 구현
    /// </summary>
    public class FirstBossController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private GameObject rushAttackBox;      //보스의 공격 박스
        [SerializeField] private BoxCollider throwAttackBox;      //보스의 공격 박스
        private Animator m_Animator;


        [SerializeField] private float turnSpeed = 1200;    //보스의 회전 속도

        public static bool isAttack { get; set; } = false; // 공격중인지 여부
        public static bool isDashing { get; set; } = false; // 돌진중인지 여부
        private int lastAttack = -1; //직전 공격 패턴
        private float jumpMoveSpeed = 10f;

        //점프 공격 패턴
        public GameObject rockPrefab;          // 돌 프리팹
        public GameObject warningEffectPrefab; // 경고 표시 프리팹
        public Transform spawnPointsParent;   // 고정된 스폰 포인트
        private List<Transform> spawnPoints = new List<Transform>(); // 자식 스폰 포인트 리스트
        public float spawnHeight = 10f;        // 돌 생성 높이
        public int rocksPerBatch = 2;          // 한 번에 생성될 돌의 개수
        public float spawnInterval = 0.4f;     // 생성 간격
        public int totalBatches = 2;           // 총 배치 수
        public float warningDuration = 0.6f;   // 경고 표시 지속 시간
        #endregion

        private void Start()
        {
            m_Animator = this.GetComponent<Animator>();

            // 부모의 모든 자식을 가져와 스폰 포인트 리스트에 추가
            foreach (Transform child in spawnPointsParent)
            {
                if (child != spawnPointsParent) // 부모 자신은 제외
                {
                    spawnPoints.Add(child);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            //플레이어와의 거리
            Vector3 direction = GameManager.Instance.PlayerTransform.position - transform.position;
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

            Vector3 dir = PlayerPosition();

            if(m_Animator.GetBool("IsAttack01"))
            {
                JumpToPlayer(dir);

            }

            AttackBoxActive();

        }

        //보스의 회전 관련 코드
        public void TurnBossToPlayer(Vector3 direction)
        {
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);

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
            else if (distance < 3f)
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

        public Vector3 PlayerPosition()
        {
            Vector3 player_Position = GameManager.Instance.PlayerTransform.position - transform.position;

            return player_Position;
            
        }

        public void JumpToPlayer(Vector3 distance)
        {
            Vector3 dir = distance.normalized;
            float player_Distance = distance.magnitude;

            transform.position += dir * jumpMoveSpeed * Time.deltaTime;

            if (player_Distance < 1f)
            {
                m_Animator.SetBool("IsAttack01", false);
            }
        }

        public void OnLanding() 
        {
            Debug.Log("!!");
            StartCoroutine(SpawnRocksWithInterval());
        }

        private IEnumerator SpawnRocksWithInterval()
        {
            int batchCount = 0;

            while (batchCount < totalBatches)
            {
                // 경고 표시 후 돌 생성
                yield return StartCoroutine(ShowWarningsAndSpawnRocks());
                batchCount++;

                // 간격 대기
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        private IEnumerator ShowWarningsAndSpawnRocks()
        {
            int spawnedCount = 0;
            

            foreach (Transform spawnPoint in spawnPoints)
            {
                if (spawnedCount >= rocksPerBatch) break; // 한 번에 rocksPerBatch 개까지만 생성

                // 경고 표시 생성
                GameObject warning = Instantiate(
                    warningEffectPrefab,
                    new Vector3(spawnPoint.position.x, 0, spawnPoint.position.z), // 지면에 생성
                    Quaternion.identity
                );

                // 일정 시간 후 경고 표시 제거
                Destroy(warning, warningDuration);

                spawnedCount++;
            }

            // 경고 지속 시간 대기
            yield return new WaitForSeconds(warningDuration);

            // 돌 생성
            SpawnRocks();
        }

        private void SpawnRocks()
        {
            int spawnedCount = 0;

            foreach (Transform spawnPoint in spawnPoints)
            {
                if (spawnedCount >= rocksPerBatch) break; // 한 번에 rocksPerBatch 개까지만 생성

                // 돌 생성 위치 계산
                Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnHeight, spawnPoint.position.z);

                // 돌 생성
                GameObject rock = Instantiate(rockPrefab, spawnPosition, Quaternion.identity);

                spawnedCount++;
            }
        }



        private void AttackBoxActive()
        {
            if (m_Animator.GetBool("IsAttack02") == true)
            {
                rushAttackBox.SetActive(true);
            }
            else
            {
                rushAttackBox.SetActive(false);
            }

            if (m_Animator.GetBool("IsAttack03") == true)
            {
                throwAttackBox.enabled = true;
            }
            else
            {
                throwAttackBox.enabled = false;
            }
        }

    }
}