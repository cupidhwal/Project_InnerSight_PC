//using Enemy;
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
            switch(effect)
            {
                case SkillEffect.Pull:

                    break;
            }
        }


        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    switch(type)
        //    {
        //        case SkillType.Single:
        //            if (collision.CompareTag("Enemy"))
        //            {
        //                EnemyMainController ec = collision.GetComponent<EnemyMainController>();

        //                if (ec != null)
        //                {
        //                    ec.TakeDamage(damage);
        //                }

        //            }
        //            break;
        //    }       
        //}

        //private void OnTriggerStay2D(Collider2D collision)
        //{
        //    switch (type)
        //    {
        //        case SkillType.Dot:
        //            if (collision.CompareTag("Enemy"))
        //            {
        //                EnemyMainController ec = collision.GetComponent<EnemyMainController>();

        //                if (ec != null)
        //                {
        //                    ctime += Time.deltaTime;

        //                    while (ctime > 0.5f)
        //                    {
        //                        ctime = 0;
        //                        ec.TakeDamage(damage);
        //                    }            
        //                }
        //            }
        //            break;
        //    }
        //}
    }
}