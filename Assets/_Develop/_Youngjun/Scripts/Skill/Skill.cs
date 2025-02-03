using System;
using UnityEngine;
using System.Collections;

namespace Noah
{
    public enum SkillRangeType
    { 
        Nomal,
        Circle,
        Cube
    }

    // 공통 부모 클래스 정의 (비제네릭)
    public abstract class SkillBase
    {
        public SkillRangeType rangeType;

        public GameObject skillPrefab;
        public float cooldown;
        public float damage;
        public float upgradeDamage;
        public int skillUpgardeCount = 1;
        public float attackRadius;
        public float skillAtkTime = 1.5f;
        public Vector3 skillPos;
        public Sprite skillImage;
        public Sprite UISprite;
        public bool isSkillOn = true;

        public string skillName;
        [TextArea(5, 5)]
        public string skillDescription;

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
    public class FireSkill : Skill<FireSkill>
    {
        public override void Activate()
        {
            isSkillOn = false;
            Debug.Log(damage + " / " + cooldown);

            

            ReturnToPool();
        }

    }

    [Serializable]
    public class Kunai : Skill<Kunai>
    {
        public override void Activate()
        {
            isSkillOn = false;
            Debug.Log(damage + " / " + cooldown);

            ReturnToPool();
        }
    }

    [Serializable]
    public class MeteorRain : Skill<MeteorRain>
    {
        public override void Activate()
        {
            isSkillOn = false;
            Debug.Log(damage + " / " + cooldown);

            ReturnToPool();
        }
    }

    [Serializable]
    public class LaserFire : Skill<LaserFire>
    {
        public override void Activate()
        {
            isSkillOn = false;
            Debug.Log(damage + " / " + cooldown);

            ReturnToPool();
        }
    }

    [Serializable]
    public class Bomb : Skill<Bomb>
    {
        public override void Activate()
        {
            isSkillOn = false;
            Debug.Log(damage + " / " + cooldown);

            ReturnToPool();
        }
    }

    [Serializable]
    public class BloodSycthe : Skill<BloodSycthe>
    {
        public override void Activate()
        {
            isSkillOn = false;
            Debug.Log(damage + " / " + cooldown);

            ReturnToPool();
        }
    }
}
