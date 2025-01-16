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

        private float ctime = 0.5f;

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
                            ctime += Time.deltaTime;

                            while (ctime > 0.5f)
                            {
                                // 데미지 데이터 가공 후 데미지 주기
                                Damagable.DamageMessage data = new();
                                data.amount = damage;

                                ec.TakeDamage(data);

                                ctime = 0;
                            }
                        }
                    }
                    break;
            }
        }
    }
}