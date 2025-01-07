using System;
using UnityEngine;
using System.Collections;

namespace Noah
{
    // 공통 부모 클래스 정의 (비제네릭)
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
        

        // 스킬 사용 시 실행되는 메서드
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
        // 스킬 사용 시 실행되는 메서드
        public override abstract void Activate(/*T skillData*/);

        // 스킬 사용 후 풀로 반환
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
