using System;
using UnityEngine;

namespace Noah
{ 
    public class PlayerUseSkill : MonoBehaviour
    {
        private bool isStartAttack = false;
        public InGameUI_Skill setSkill;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            ActiveSkill();
        }

        void ActiveSkill()
        {
            if (Input.GetKeyDown("1"))
            {
                if (setSkill.firstSkill != null && setSkill.firstSkill.isSkillOn)
                {
                    UseSkill(() => setSkill.firstSkill);
                    StartCoroutine(setSkill.SkillCoolTime(setSkill.firstSkill));
                }

            }
            else if (Input.GetKeyDown("2"))
            {
                if (setSkill.secondSkill != null && setSkill.secondSkill.isSkillOn)
                {
                    UseSkill(() => setSkill.secondSkill);
                    StartCoroutine(setSkill.SkillCoolTime(setSkill.secondSkill));
                }
            }
            else if (Input.GetKeyDown("3"))
            {
                if (setSkill.thirdSkill != null && setSkill.thirdSkill.isSkillOn)
                {
                    UseSkill(() => setSkill.thirdSkill);
                    StartCoroutine(setSkill.SkillCoolTime(setSkill.thirdSkill));
                }
            }
            else if (Input.GetKeyDown("4"))
            {
                if (setSkill.fourthSkill != null && setSkill.fourthSkill.isSkillOn)
                {
                    UseSkill(() => setSkill.fourthSkill);
                    StartCoroutine(setSkill.SkillCoolTime(setSkill.fourthSkill));
                }
            }


        }

        void UseSkill(Func<SkillBase> factoryMethod)
        {
            SkillBase skill = factoryMethod();

            if (skill != null)
            {
                if (skill.isSkillOn)
                {
                    //isSkillAtk = true;
                    isStartAttack = true;

                    Vector2 skillPos = Vector2.zero;

                    if (transform.rotation.y == 0f)
                    {
                        skillPos = new Vector2(transform.position.x + skill.skillPos.x, transform.position.y + skill.skillPos.y);
                    }
                    else
                    {
                        skillPos = new Vector2(transform.position.x - skill.skillPos.x, transform.position.y + skill.skillPos.y);
                    }

                    skill.Activate();
                    //animator.SetBool("isSkill", true);
                    GameObject skillef = Instantiate(skill.skillPrefab, skillPos, transform.rotation);

                    skillef.transform.GetChild(0).GetComponent<SkillAttack>().damage = skill.damage;

                    StartCoroutine(skill.SkillCoolTime());
                    Destroy(skillef, skill.skillAtkTime);
                }

            }
        }
    }
}