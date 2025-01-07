using System;
using UnityEngine;
using System.Collections;

namespace Noah
{
    // ���� �θ� Ŭ���� ���� (�����׸�)
    public abstract class SkillBase
    {
        public GameObject skillPrefab;
        public float cooldown;
        public float damage;
        public float upgradeDamage;
        public float attackRadius;
        public float skillAtkTime = 1.5f;
        public Vector2 skillPos;
        public Sprite skillImage;
        public Sprite UISprite;
        public bool isSkillOn = true;
        

        // ��ų ��� �� ����Ǵ� �޼���
        public abstract void Activate();

        public virtual IEnumerator SkillCoolTime()
        {
            if (isSkillOn == false)
            {
                yield return new WaitForSeconds(cooldown);

                isSkillOn = true;
            }
        }
    }

    public abstract class Skill<T> : SkillBase where T : Skill<T>
    {
        // ��ų ��� �� ����Ǵ� �޼���
        public override abstract void Activate(/*T skillData*/);

        // ��ų ��� �� Ǯ�� ��ȯ
        public void ReturnToPool()
        {
            SkillPool<T>.ReturnObject((T)this);
        }
    }

    [Serializable]
    public class FireWallSkill : Skill<FireWallSkill>
    {
        public override void Activate()
        {
            isSkillOn = false;
            Debug.Log(damage + " / " + cooldown);

            

            ReturnToPool();
        }

    }

    [Serializable]
    public class Meteoros : Skill<Meteoros>
    {
        public override void Activate()
        {
            isSkillOn = false;
            Debug.Log(damage + " / " + cooldown);

            ReturnToPool();
        }
    }

    [Serializable]
    public class FireHand : Skill<FireHand>
    {
        public override void Activate()
        {
            isSkillOn = false;
            Debug.Log(damage + " / " + cooldown);

            ReturnToPool();
        }
    }

    [Serializable]
    public class FireBomb : Skill<FireBomb>
    {
        public override void Activate()
        {
            isSkillOn = false;
            Debug.Log(damage + " / " + cooldown);

            ReturnToPool();
        }
    }

    [Serializable]
    public class FireNapalm : Skill<FireNapalm>
    {
        public override void Activate()
        {
            isSkillOn = false;
            Debug.Log(damage + " / " + cooldown);

            ReturnToPool();
        }
    }

    [Serializable]
    public class FireIncendiary : Skill<FireIncendiary>
    {
        public override void Activate()
        {
            isSkillOn = false;
            Debug.Log(damage + " / " + cooldown);

            ReturnToPool();
        }
    }
}
