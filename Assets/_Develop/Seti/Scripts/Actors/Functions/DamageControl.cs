using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Noah;
using Yoon;
using InnerSight_Kys;

namespace Seti
{
    public class DamageControl : MonoBehaviour, IMessageReceiver
    {
        // 필드
        #region Variables
        private Actor actor;

        // 데미지 처리
        protected Damagable m_Damagable;

        [Header("Criteria: Hit")]
        [SerializeField]
        private float KnockbackCoefficient = 4f;
        [SerializeField]
        private float destroyDelay = 2f;

        [Header("Dissolve : Enemy")]
        [SerializeField]
        private Renderer bodyRenderer;
        [SerializeField]
        private Material[] dissolve;
        #endregion

        public Renderer BodyRenderer => bodyRenderer;
        public Material[] Dissolve => dissolve;

        // 인터페이스
        #region Interface
        /*public bool IsRelevant(DamageControl damageControl)
        {
            Actor actor = GetComponent<Actor>();
            switch (actor)
            {
                case Player:
                    return actor is not NPC && actor != this;

                case Player_Alter:
                    return actor is Player;

                case NPC:
                    return actor is not Player && actor != this;

                case Enemy:
                    return actor is Player || actor is NPC;

                default:
                    return false;
            }
        }*/

        public void OnReceiveMessage(GameMessageType type, object sender, object msg)
        {
            Damagable.DamageMessage damageData = (Damagable.DamageMessage)msg;
            switch (type)
            {
                case GameMessageType.Damaged:
                    Damaged(damageData);
                    break;

                case GameMessageType.Dead:
                    Die(damageData);
                    break;
            }
        }
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void OnEnable()
        {
            actor = GetComponent<Actor>();

            m_Damagable = GetComponent<Damagable>();
            m_Damagable.OnDamageMessageReceivers.Add(this);
            m_Damagable.IsInvulnerable = true;
        }

        private void OnDisable()
        {
            m_Damagable.OnDamageMessageReceivers.Remove(this);
            m_Damagable = null;
        }
        #endregion

        // 메서드
        #region Methods
        // 데미지 처리, 애니메이션, 연출, ...
        void Damaged(Damagable.DamageMessage damageMessage)
        {
            // 참조
            if (actor)
            {
                // 넉백 기능
                Knockback(Knockback(damageMessage));

                if (actor is Player)
                {
                    AudioManager.Instance.Play("Player Hitting Sound");
                }
                else
                {
                    AudioManager.Instance.Play("PlayerAtackSound");
                }
            }

            if (TryGetComponent<DamageText>(out var damageText))
            {
                damageText.OnTakeDamage(damageMessage);
            }
        }

        // 사망 처리, 애니메이션, 연출, ...
        void Die(Damagable.DamageMessage damageMessage)
        {
            // 최후의 데미지
            if (TryGetComponent<DamageText>(out var damageText))
            {
                damageText.OnTakeDamage(damageMessage);
            }

            // 더 이상 플레이어와 충돌하지 않도록 처리
            if (actor is Enemy enemy)
            {
                AudioManager.Instance.Play("PlayerAtackSound");
                AudioManager.Instance.Play("EnemyDeath");

                Collider collider = GetComponent<Collider>();
                collider.excludeLayers = LayerMask.GetMask("Player");
                try
                {
                    enemy.Agent.ResetPath();
                }
                catch { }
                
                enemy.Agent.enabled = false;

                if (dissolve.Length > 0)
                {
                    Material[] newMaterials = new Material[dissolve.Length];
                    for (int i = 0; i < dissolve.Length; i++)
                    {
                        newMaterials[i] = new Material(dissolve[i]);
                    }
                    bodyRenderer.SetMaterials(new List<Material>(newMaterials));
                    StartCoroutine(DeathComposition(destroyDelay));
                }
                else
                {
                    Destroy(gameObject, destroyDelay);
                }

                if (enemy.magicCurrent)
                    Destroy(enemy.magicCurrent);
            }

            // 플레이어 사망 시 재시작
            if (actor is Player)
            {
                // 죽음 횟수 +1 / 저장
                DataManager.Instance.deathCount++;

                // 원흉 이벤트를 안 본 경우 카운트 제한
                if (!SaveLoadManager.Instance.scenarioSaveData.sinEvent[0])
                    DataManager.Instance.deathCount = Mathf.Clamp(DataManager.Instance.deathCount, 0, 1);
                if (!SaveLoadManager.Instance.scenarioSaveData.sinEvent[1])
                    DataManager.Instance.deathCount = Mathf.Clamp(DataManager.Instance.deathCount, 0, 2);

                SaveLoadManager.Instance.SaveScenario(DataManager.Instance.DialogueData);

                // 재시작
                StageManager.Instance.ReStartGame();
            }
        }

        // 씬 내의 대적자 액터 가져오기
        /*public List<DamageControl> GetRelevantActors(IMessageReceiver filter)
        {
            return FindObjectsByType<DamageControl>(FindObjectsSortMode.None)
                .Where(filter.IsRelevant)
                .ToList();
        }*/
        #endregion

        // 유틸리티
        // 넉백
        private void Knockback(IEnumerator knockbackCor)
        {
            StopAllCoroutines();
            StartCoroutine(knockbackCor);
        }
        IEnumerator Knockback(Damagable.DamageMessage damageMessage)
        {
            // 애니메이션의 Root Motion을 쓰지 않을 경우에만 실행
            if (actor.Controller_Animator.Animator.applyRootMotion) yield break;

            // 피해자
            Condition_Actor condition = GetComponent<Condition_Actor>();
            condition.HitDirection = damageMessage.direction.normalized;

            // 가해자
            if (damageMessage.owner)
            {
                Actor antagonist = damageMessage.owner.GetComponent<Actor>();

                // 초기 속도 설정 - 가해자 기준
                float elapsedTime = 0f;
                float atkDuration = 0.16f;
                float currentSpeed = KnockbackCoefficient *
                                     antagonist.Rate_Movement_Default;
                while (elapsedTime < atkDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / atkDuration;

                    // Ease In-Out 적용
                    currentSpeed = Mathf.Lerp(currentSpeed, 0, Mathf.SmoothStep(0f, 1f, t));
                    actor.transform.Translate(currentSpeed * Time.deltaTime * antagonist.transform.forward, Space.World);

                    yield return null;
                }
            }

            yield break;
        }
        IEnumerator DeathComposition(float delay)
        {
            float dissolveDegree = 0.6f;

            // 모든 머티리얼 가져오기
            Material[] materials = bodyRenderer.materials;

            // 처음 `_Degree` 값 설정
            foreach (var mat in materials)
            {
                mat.SetFloat("_Degree", dissolveDegree);
            }

            yield return new WaitForSeconds(delay - 1);

            while (dissolveDegree >= -0.4f)
            {
                dissolveDegree -= Time.deltaTime;

                // 모든 머티리얼에 `_Degree` 값 적용
                foreach (var mat in materials)
                {
                    mat.SetFloat("_Degree", dissolveDegree);
                }

                yield return null;
            }

            Destroy(gameObject);
        }
    }
}