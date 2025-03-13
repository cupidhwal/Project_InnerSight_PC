//using Enemy;
using Seti;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noah
{
    public enum SkillType
    {
        Single,
        Dot
    }

    public enum SkillEffect
    {
        Nomal,
        Pull
    }

    public class SkillAttack : MonoBehaviour
    {
        public SkillType type;
        public SkillEffect effect;

        public float damage;

        [SerializeField] private float ctime = 0f;
        [SerializeField] private float attackDur = 0.5f;

        private Dictionary<Transform, float> enemyTimers = new(); // 각 적의 ctime을 저장하는 딕셔너리

        void HitSkill(Transform enemy)
        {
            switch (effect)
            {
                case SkillEffect.Pull:

                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (type)
            {
                case SkillType.Single:
                    if (other.CompareTag("Enemy"))
                    {
                        Damagable ec = other.GetComponent<Damagable>();

                        if (ec != null)
                        {
                            // 데미지 데이터 가공 후 데미지 주기
                            Damagable.DamageMessage data = new();
                            data.amount = damage;

                            ec.TakeDamage(data);
                        }

                    }
                    break;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            switch (type)
            {
                case SkillType.Dot:
                    if (other.CompareTag("Enemy"))
                    {
                        Damagable ec = other.GetComponent<Damagable>();

                        if (ec != null)
                        {
                            Transform enemyTransform = other.transform;

                            // 해당 적이 Dictionary에 없으면 추가
                            if (!enemyTimers.ContainsKey(enemyTransform))
                            {
                                enemyTimers[enemyTransform] = 0f;
                            }

                            // 해당 적의 타이머 증가
                            enemyTimers[enemyTransform] += Time.deltaTime;

                            if (enemyTimers[enemyTransform] >= attackDur)  // 개별 타이머 체크
                            {
                                Damagable.DamageMessage data = new();
                                data.amount = damage;

                                ec.TakeDamage(data);

                                enemyTimers[enemyTransform] -= attackDur;  // attackDur 만큼 감소
                            }
                        }
                    }
                    break;
            }
        }
    }
}