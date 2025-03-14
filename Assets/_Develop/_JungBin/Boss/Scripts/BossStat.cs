using Noah;
using Seti;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace JungBin
{
    public enum BossType
    {
        FirstBoss,
        SecondBoss,
        SecondBoss2,
        LastBoss
        
    }

    public class BossStat : MonoBehaviour
    {
        // Public Properties
        [SerializeField] private string bossName; // 보스 이름

        [SerializeField] private BossType bossType; // 보스 타입 설정

        [SerializeField] private float maxHealth = 500f; // 최대 체력
        [SerializeField] private float invulnerabilityTime = 3.5f; // 무적 시간
        [SerializeField] private float bossAttack = 30f; // 보스 공격력
        [SerializeField] private GameObject relicPrefab; // 드랍할 유물
        [SerializeField] private CapsuleCollider capsuleCollider;
        [SerializeField] private float fadeDuration = 2f; // 서서히 사라지는 시간
        [SerializeField] private GameObject bossHealthBarUI;
        [SerializeField] private Material[] bossMaterials; // 여러 개의 머티리얼 관리
        private Color[] originalColors; // 머티리얼 초기 색상 저장
        [SerializeField] private GameObject berserkEffect;
        [SerializeField] private GameObject smokeParticlePrefab;
        [SerializeField] private Material smokeMaterial;
        [SerializeField] private BossStageManager bossStageManager;
        private LastBossManager lastBossManager;

        [SerializeField] private Animator animator; // 보스 애니메이션
        private bool isBerserk = false; // 버서커 모드 여부
        [SerializeField] private bool isInvulnerable = false; // 무적 여부
        private float timeSinceLastHit = 0f; // 무적 시간 관리

        private Damagable damagable; // Damagable 참조
        [SerializeField] private SecondBossConnect secondBossConnect;
        [SerializeField] private float smokeFadeDuration = 2f; // 서서히 사라지는 시간
        [SerializeField] private Transform slashSpawnPoint;

        // Events
        public UnityAction OnDeath; // 보스가 죽었을 때
        public UnityAction OnBecomeVulnerable; // 무적 해제 시

        public float AttackDamage => bossAttack;
        public float MaxHealth => maxHealth; // 보스의 최대 체력
        public float Health => damagable != null ? damagable.CurrentHitPoints : 0; // 현재 체력
        public string BossName => bossName; // 보스 이름 접근자

        private void Start()
        {
                       // 초기화
            animator = GetComponent<Animator>();
            damagable = GetComponent<Damagable>();
            lastBossManager = GetComponent<LastBossManager>();



            if (damagable != null)
            {
                            // Damagable 초기화
                damagable.OnDeath += HandleDeath;
                damagable.OnReceiveDamage += HandleReceiveDamage;
            }

                       // 머티리얼 초기 색상 저장
            originalColors = new Color[bossMaterials.Length];
            for (int i = 0; i < bossMaterials.Length; i++)
            {
                originalColors[i] = bossMaterials[i].color;
            }

            OnBecomeVulnerable += HandleBecomeVulnerable;
            ResetHealth();

            OnDeath += SpawnRelic;
            OnDeath += OnBossDeath;

        }

                  // 🎯 특정 애니메이션 파라미터가 존재하는지 확인하는 함수
        private bool HasAnimatorParameter(string paramName)
        {
            if (animator == null)
            {
                Debug.LogWarning("⚠️ Animator가 존재하지 않습니다!");
                return false;
            }

            if (animator.parameters == null)
            {
                Debug.LogWarning("⚠️ Animator에 파라미터가 없습니다.");
                return false;
            }

            return animator.parameters.Any(p => p.name == paramName);
        }


        private void Update()
        {
                       // 무적 상태 타이머 처리
            if (isInvulnerable)
            {
                timeSinceLastHit += Time.deltaTime;
                if (timeSinceLastHit >= invulnerabilityTime)
                {
                    OnBecomeVulnerable?.Invoke(); // 무적 해제 이벤트 호출
                    isInvulnerable = false;
                    timeSinceLastHit = 0f;
                    
                }
            }

            // 버서커 모드 전환
            if (!isBerserk && Health <= maxHealth / 2)
            {
                if (bossType == BossType.FirstBoss || bossType == BossType.LastBoss)
                {
                    EnterBerserkMode();
                }
            }
        }

        // 버서커 모드 진입
        private void EnterBerserkMode()
        {
            isBerserk = true;
            capsuleCollider.enabled = false;
            isInvulnerable = true;
            bossAttack *= 2f;
            if (bossType == BossType.LastBoss)
            {
                lastBossManager.FireOffAttack();
            }
            if (HasAnimatorParameter("IsBerserk"))
            {
                animator.SetBool("IsBerserk", true);
                animator.SetTrigger("Berserk");
            }
/*
            if (HasAnimatorParameter("IsWall"))
            {
                animator.SetBool("IsWall", false);
            }

            if (HasAnimatorParameter("IsPlayer"))
            {
                animator.SetBool("IsPlayer", false);
            }*/

            Debug.Log("🔥 버서커 모드로 전환됨: 무적 상태 활성화");

            if (bossType == BossType.FirstBoss)
            {
                berserkEffect.SetActive(true);
            }
        }


        // 보스 사망 처리
        private void HandleDeath()
        {
            if (Health > 0) return; // 이미 죽었으면 처리하지 않음

            animator.SetBool("IsDeath", true);
            animator.SetTrigger("Death");
            if (bossType == BossType.FirstBoss)
            {
                berserkEffect.SetActive(false);
            }
            OnDeath?.Invoke(); // 죽음 이벤트 호출
            capsuleCollider.enabled = false;
            Debug.Log("보스가 사망했습니다.");
        }

        // 보스가 데미지를 받을 때
        private void HandleReceiveDamage()
        {
            Debug.Log("보스가 공격을 받았습니다!");
        }

        // 무적 상태 해제
        private void HandleBecomeVulnerable()
        {
            Debug.Log("보스의 무적 상태가 해제되었습니다.");
            capsuleCollider.enabled = true;
        }

        public void OnBossDeath()
        {
            StartCoroutine(FadeOut());
        }

        private IEnumerator FadeOut()
        {
            SecondBossManager.isAttack = true;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

                foreach (var material in bossMaterials)
                {
                    if (material != null)
                    {
                        if (bossType == BossType.SecondBoss)
                        {
                            if (material != bossMaterials[7])
                            {
                                //Debug.Log("보스 Material 삭제");
                                Color newColor = material.color;
                                newColor.a = alpha;
                                material.color = newColor;
                            }
                        }
                        else
                        {
                           // Debug.Log("보스 Material 삭제");
                            Color newColor = material.color;
                            newColor.a = alpha;
                            material.color = newColor;
                        }
                        
                    }
                }

                yield return null;
            }

            Destroy(bossHealthBarUI);
            if (bossType == BossType.FirstBoss || bossType == BossType.SecondBoss2)
            {
                Destroy(gameObject);
            }
        }



        // 유물 드랍
        private void SpawnRelic()
        {
            if (relicPrefab == null) return;

            IRelic relicname = relicPrefab.GetComponent<IRelic>();

            foreach (var relic in SaveLoadManager.Instance.relicSaveData.relics)
            {
                if (relic.relicName == relicname.RelicName) return; // 중복 획득 방지
            }

            capsuleCollider.enabled = false;
            Instantiate(relicPrefab, transform.position, Quaternion.identity, this.transform.parent);
        }

        // 보스 체력 및 상태 초기화
        public void ResetHealth()
        {
            if (damagable != null)
            {
                damagable.ResetDamage();
            }

            isInvulnerable = false;
            isBerserk = false;
            timeSinceLastHit = 0f;
            if (bossType == BossType.FirstBoss || bossType == BossType.LastBoss)
            {
                animator.SetBool("IsBerserk", false);
            }
            animator.SetBool("IsDeath", false);

            if(bossType == BossType.SecondBoss2)
            {
                isInvulnerable = true;
                capsuleCollider.enabled = false;
            }

                        // 머티리얼 색상 초기화
            for (int i = 0; i < bossMaterials.Length; i++)
            {
                if (i <= 6)
                {
                    if (bossMaterials[i] != null)
                    {
                        Color newColor = originalColors[i];
                        newColor.a = 1f;
                        bossMaterials[i].color = newColor;
                    }
                }
                else
                {
                    if (bossMaterials[i] != null)
                    {
                        Color newColor = originalColors[i];
                        newColor.a = 0.5f;
                        bossMaterials[i].color = newColor;
                    }
                }
            }

        }
        

        public void Phase2Start()
        {
            StartCoroutine(PhaseChange());
        }

        private IEnumerator PhaseChange()
        {
            yield return new WaitForSeconds(2f);


            GameObject smokeObj = Instantiate(smokeParticlePrefab, slashSpawnPoint.position, Quaternion.Euler(-90, 0, 0), transform);
            Debug.Log("☁️ 안개 소환");

            yield return new WaitForSeconds(1f);

            smokeObj.transform.GetChild(0).gameObject.SetActive(true);
            Debug.Log("☁️ 안개 자식 활성화");

            yield return new WaitForSeconds(2f);

            if (secondBossConnect == null)
            {
                Debug.Log("🔥 SecondBossConnect 비활성화됨!");
                yield break;
            }

            if (!secondBossConnect.gameObject.activeInHierarchy)
            {
                Debug.LogWarning("⚠️ secondBossConnect가 비활성화 상태입니다. 활성화합니다.");
                secondBossConnect.gameObject.SetActive(true);
            }
            secondBossConnect.PhaseChange();
            
            Debug.Log("🔥 SecondBossConnect 활성화됨!");

            yield return new WaitForSeconds(1f);

            StartCoroutine(SmokeFadeOut());
            //ParticleSystem smokeParticle = smokeObj.GetComponent<ParticleSystem>();
            //smokeParticle.Stop();

            yield return new WaitForSeconds(2f);

            Destroy(smokeObj);
            Destroy(gameObject);

            Debug.Log("☁️ 안개 삭제 완료");
        }


        private IEnumerator SmokeFadeOut()
        {
            float elapsedTime = 0f;
            while (elapsedTime < smokeFadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(0.5f, 0f, elapsedTime / smokeFadeDuration);

                if (smokeMaterial != null)
                {
                    
                    Color newColor = smokeMaterial.color;
                    newColor.a = alpha;
                    smokeMaterial.color = newColor;
                }

                yield return null;
            }
            Debug.Log("안개 페이드 아웃");
        }
        

        public void BossIsInvulnerable()
        {
            isInvulnerable = true;
            capsuleCollider.enabled = false;
        }


        /*// 치트로 즉시 죽이기
        public void CheatDie()
        {
            if (damagable != null)
            {
                damagable.TakeDamage(new Damagable.DamageMessage
                {
                    amount = damagable.CurrentHitPoints, // 현재 체력을 모두 감소시켜 즉시 죽임
                    damager = this,
                    owner = this,
                    damageSource = transform.position,
                    direction = Vector3.zero,
                    throwing = false,
                    stopCamera = false
                });
            }
        }*/
    }
}
