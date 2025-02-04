using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Weapon 추상 클래스
    /// </summary>
    public class Weapon : MonoBehaviour
    {
        /// <summary>
        /// 무기 공격 시 상대에게 입히는 피해의 구성
        /// </summary>
        [System.Serializable]
        public class AttackPoint
        {
            public float radius;
            public Vector3 offset;
            public Transform attackRoot;

#if UNITY_EDITOR
            public List<Vector3> previousPositions = new();
#endif
        }

        // 필드
        #region Variables
        protected SphereCollider hitBall;

        //[SerializeField]
        protected int damage = 1;      // hit 시 데미지

        public AttackPoint[] attackPoints = new AttackPoint[0];     //
        public TimeEffect[] effects;                                //

        public ParticleSystem hitParticlePrefab;                    //
        //public AttackPoint attackPoint;
        public LayerMask targetLayers;

        [SerializeField]
        protected GameObject m_Owner;
        protected Actor m_Owner_Actor;

        protected Vector3[] m_PreviousPos = null;
        protected Vector3 m_Direction;

        protected bool m_IsThrowingHit = false;
        [SerializeField]
        protected bool m_InAttack = false;

        protected const int PARTICLE_COUNT = 10;
        protected ParticleSystem[] m_ParticlesPool = new ParticleSystem[PARTICLE_COUNT];
        protected int m_CurrentParticle;

        protected static RaycastHit[] s_RaycastHitCache = new RaycastHit[32];
        protected static Collider[] s_ColliderCache = new Collider[32];
        #endregion

        // 속성
        #region Properties
        public int Damage => damage;
        public bool ThrowingHit
        {
            get { return m_IsThrowingHit; }
            set { m_IsThrowingHit = value; }
        }
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected void FixedUpdate()
        {
            /*if (m_InAttack)
            {
                // 어택 포인트별 히트 판정
                for (int i = 0; i < attackPoints.Length; i++)
                {
                    AttackPoint atp = attackPoints[i];
                    Vector3 worldPos = attackPoints[i].attackRoot.position +
                        attackPoints[i].attackRoot.TransformVector(attackPoints[i].offset);

                    Vector3 attackVector = worldPos - m_PreviousPos[i];
                    if (attackVector.magnitude < 0.001f)
                    {
                        attackVector = Vector3.forward * 0.0001f;
                    }

                    Ray r = new(worldPos, attackVector.normalized);
                    int contacts = Physics.SphereCastNonAlloc(r,
                                                              atp.radius,
                                                              s_RaycastHitCache,
                                                              attackVector.magnitude,
                                                              ~0,
                                                              QueryTriggerInteraction.Ignore);
                    for (int j = 0; j < contacts; j++)
                    {
                        //Debug.Log("FixedUpdate_Ray");
                        Collider col = s_RaycastHitCache[i].collider;
                        try
                        {
                            if (col != null && col.gameObject != m_Owner)
                            {
                                //Debug.Log("FixedUpdate_CheckDamage");
                                CheckDamage(col, atp);
                            }
                        }
                        catch
                        {
                            Debug.Log("해당 Actor는 이미 토벌되었습니다.");
                        }
                    }

                    m_PreviousPos[i] = worldPos;
#if UNITY_EDITOR
                    attackPoints[i].previousPositions.Add(m_PreviousPos[i]);
#endif
                }
            }*/
        }

        private void Awake()
        {
            hitBall = GetComponent<SphereCollider>();

            // 타격 이펙트 풀 생성
            GenEffectPool();
        }
        #endregion

        // 메서드
        #region Methods
        // 무기의 주인
        public void SetOwner(GameObject owner)
        {
            m_Owner = owner;
            m_Owner_Actor = m_Owner.GetComponent<Actor>();
        }

        public void BeginAttack(bool throwingAttack)
        {
            if (m_Owner_Actor.Condition.IsDead) return;

            ThrowingHit = throwingAttack;
            m_PreviousPos = new Vector3[attackPoints.Length];

            for (int i = 0; i < attackPoints.Length; i++)
            {
                Vector3 worldPos = attackPoints[i].attackRoot.position +
                    attackPoints[i].attackRoot.TransformVector(attackPoints[i].offset);
                m_PreviousPos[i] = worldPos;

#if UNITY_EDITOR
                attackPoints[i].previousPositions.Clear();
                attackPoints[i].previousPositions.Add(m_PreviousPos[i]);
#endif
            }

            m_InAttack = true;
            if (hitBall == null)
                hitBall = GetComponent<SphereCollider>();
            hitBall.radius = 2f;
        }

        public void EndAttack()
        {
            if (hitBall != null)
                hitBall.radius = 0.00001f;
            m_InAttack = false;

#if UNITY_EDITOR
            for (int i = 0; i < attackPoints.Length; i++)
            {
                attackPoints[i].previousPositions.Clear();
            }
#endif
        }

        // 콜라이더 확인 후 데미지 주기
        protected void CheckDamage(Collider other, AttackPoint atp)
        {
            // 콜라이더 확인 후
            if (!other.TryGetComponent<Damagable>(out var d))
                return;

            // 셀프 데미지 체크
            if (d.gameObject == m_Owner)
                return;

            // 타겟 레이어 체크
            if ((targetLayers.value & (1 << other.gameObject.layer)) == 0)
                return;

            // 적 위치
            Vector3 hitDirection = other.transform.position - m_Owner.transform.position;

            // 데미지 데이터 가공 후 데미지 주기
            Damagable.DamageMessage data = new()
            {
                damager = this,
                owner = m_Owner.GetComponent<Actor>(),
                amount = (int)(Damage * m_Owner.GetComponent<Actor>().Attack),
                direction = hitDirection.normalized,
                damageSource = m_Owner.transform.position,
                throwing = ThrowingHit,
                stopCamera = false
            };

            d.TakeDamage(data);

            // 타격 이펙트
            if (hitParticlePrefab != null)
            {
                m_ParticlesPool[m_CurrentParticle].transform.position = atp.attackRoot.transform.position;
                m_ParticlesPool[m_CurrentParticle].time = 0;
                m_ParticlesPool[m_CurrentParticle].Play();
                m_CurrentParticle = (m_CurrentParticle + 1) % PARTICLE_COUNT;
            }
        }

        // 타격 이펙트 풀 생성
        protected void GenEffectPool()
        {
            if (hitParticlePrefab != null)
            {
                for (int i = 0; i < PARTICLE_COUNT; i++)
                {
                    m_ParticlesPool[i] = Instantiate(hitParticlePrefab);
                    m_ParticlesPool[i].Stop();
                }
            }
        }
        #endregion

        // 이벤트 메서드
        #region Event Methods
        private void OnTriggerEnter(Collider other)
        {
            if (!m_InAttack) return;

            switch (m_Owner_Actor)
            {
                case Player:
                    if (other.CompareTag("Enemy"))
                    {
                        CheckDamage(other, attackPoints[0]);
                    }
                    break;

                case Enemy:
                    if (other.CompareTag("Player"))
                    {
                        CheckDamage(other, attackPoints[0]);
                    }
                    break;
            }
        }
        #endregion

        // 유틸리티
        #region Utilities
#if UNITY_EDITOR
        protected void OnDrawGizmosSelected()
        {
            for (int i = 0; i < attackPoints.Length; i++)
            {
                AttackPoint attackPoint = attackPoints[i];

                if (attackPoint.attackRoot != null)
                {
                    Vector3 worldPos = attackPoint.attackRoot.TransformVector(attackPoint.offset);
                    Gizmos.color = new(1.0f, 1.0f, 1.0f, 0.4f);
                    Gizmos.DrawSphere(attackPoint.attackRoot.position + worldPos, attackPoint.radius);
                }

                if (attackPoint.previousPositions.Count > 0)
                {
                    UnityEditor.Handles.DrawAAPolyLine(10, attackPoint.previousPositions.ToArray());
                }
            }
        }
#endif
        #endregion
    }
}