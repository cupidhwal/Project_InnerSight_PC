using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using Seti;

namespace Noah
{
    public enum SkillRangeType
    { 
        Nomal,
        Circle,
        Cube,
        Proximity
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

        // 스킬 애니메이션 작동 번호 (고유 애니메이션 없으면 -1)
        public int animationNum = -1;

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

        public virtual void     PlayerAnimation(Transform _player, int _typeNum)
        {
            if (_player.GetComponent<Controller_Input>().BehaviourMap.TryGetValue(typeof(Attack), out var attackBehaviour))
            {
                if (attackBehaviour is Attack attack)
                {
                    attack.OnSlash(_typeNum);

                    _player.GetComponent<Actor>().Condition.AttackPoint = RayManager.Instance.RayToScreen();
                }
            }
        }

    }

    public abstract class Skill<T> : SkillBase where T : Skill<T>
    {
        // 스킬 사용 시 실행되는 메서드
        public override abstract void Activate();

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

    [Serializable]
    public class DoubleSlash : Skill<DoubleSlash>
    {
        public override void Activate()
        {
            isSkillOn = false;
            Debug.Log(damage + " / " + cooldown);

            ReturnToPool();
        }

    }

    [Serializable]
    public class MultipleSlash : Skill<MultipleSlash>
    {
        public override void Activate()
        {
            isSkillOn = false;
            Debug.Log(damage + " / " + cooldown);

            ReturnToPool();
        }
    }

    [Serializable]
    public class IceAge : Skill<IceAge>
    {
        public override void Activate()
        {
            isSkillOn = false;
            Debug.Log(damage + " / " + cooldown);

            ReturnToPool();
        }
    }


}
