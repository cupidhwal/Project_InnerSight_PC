using System;
using TMPro;
using UnityEngine;

namespace Noah
{ 
    public class PlayerUseSkill : MonoBehaviour
    {
        private bool isStartAttack = false;
        public InGameUI_Skill setSkill;
        public GameObject skillRange;
        public LayerMask groundLayerMask;
        private Camera mainCamera;

        private GameObject effectGo;

        private bool isReadySkill = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Init();
        }

        // Update is called once per frame
        void Update()
        {
            if (isReadySkill)
            {
                CheckSkillRange();

                if (Input.GetMouseButton(0))
                {
                    isReadySkill = false;

                    Destroy(effectGo);

                    if (setSkill.skillSlots[0] != null && setSkill.skillSlots[0].isSkillOn)
                    {
                        UseSkill(() => setSkill.skillSlots[0]);
                        StartCoroutine(setSkill.SkillCoolTime(setSkill.skillSlots[0], setSkill.skillUIList[0]));
                    }
                }
            }



            ActiveSkill();
        }

        void Init()
        {
            mainCamera = Camera.main;
        }

        void UsePlayerSkill()
        { 
            
        }

        void ActiveSkill()
        {
            if (Input.GetKeyDown("1"))
            {
                isReadySkill = true;


            }
            else if (Input.GetKeyDown("2"))
            {
                if (setSkill.skillSlots[1] != null && setSkill.skillSlots[1].isSkillOn)
                {
                    UseSkill(() => setSkill.skillSlots[1]);
                    StartCoroutine(setSkill.SkillCoolTime(setSkill.skillSlots[1], setSkill.skillUIList[1]));
                }
            }
            else if (Input.GetKeyDown("3"))
            {
                if (setSkill.skillSlots[2] != null && setSkill.skillSlots[2].isSkillOn)
                {
                    UseSkill(() => setSkill.skillSlots[2]);
                    StartCoroutine(setSkill.SkillCoolTime(setSkill.skillSlots[2], setSkill.skillUIList[2]));
                }
            }
            else if (Input.GetKeyDown("4"))
            {
                if (setSkill.skillSlots[3] != null && setSkill.skillSlots[3].isSkillOn)
                {
                    UseSkill(() => setSkill.skillSlots[3]);
                    StartCoroutine(setSkill.SkillCoolTime(setSkill.skillSlots[3], setSkill.skillUIList[3]));
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
                    GameObject skillef = Instantiate(skill.skillPrefab, RayToScreen(), Quaternion.identity);

                    //skillef.transform.GetChild(0).GetComponent<SkillAttack>().damage = skill.damage;

                    StartCoroutine(skill.SkillCoolTime());
                    Destroy(skillef, skill.skillAtkTime);
                }

            }
        }

        void CheckSkillRange()
        {
            Vector3 pos = RayToScreen() + new Vector3(0f, 0.1f, 0f);

            if (skillRange != null && effectGo == null)
            {
                effectGo = Instantiate(skillRange, pos, skillRange.transform.rotation);
            }

            if (effectGo != null)
            {
                effectGo.transform.position = pos;
            }
        }

        Vector3 RayToScreen()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, groundLayerMask))
            {
                return hit.point;
            }

            return hit.point;
        }
    }
}