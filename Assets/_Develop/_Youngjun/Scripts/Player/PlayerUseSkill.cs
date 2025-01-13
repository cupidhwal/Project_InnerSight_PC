using System;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace Noah
{ 
    public class PlayerUseSkill : MonoBehaviour
    {
        private bool isStartAttack = false;
        public InGameUI_Skill setSkill;
        private GameObject skillRange_Circle;
        private GameObject skillRange_Cube;
        public LayerMask groundLayerMask;
        private Camera mainCamera;

        private GameObject effectGo;
        private GameObject skillef;

        public bool isReadySkill = false;
        public bool isChange = false;

        private int index = 0;

        public float rotationSpeed = 100f; // 회전 속도

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Init();
        }

        // Update is called once per frame
        void Update()
        {
            // 스킬이 없을 때
            if (setSkill.skillSlots.Count == 0)
                return;

            // 사용할 스킬 Index 확인
            ActiveSkill();

            // 없는 스킬 버튼을 눌렀을 때
            if (setSkill.skillSlots.Count < (index + 1))
            {
                isReadySkill = false;
                return;
            }


            // 스킬 사용 및 범위 지정
            if (isReadySkill && setSkill.skillSlots[index] != null)
            {
                RangeType(setSkill.skillSlots[index]);

                UsePlayerSkill(index);
            }
        }

        void Init()
        {
            mainCamera = Camera.main;
            skillRange_Circle = transform.GetChild(0).gameObject;
            skillRange_Cube = transform.GetChild(1).gameObject;
        }

        void UsePlayerSkill(int _index)
        {
            if (Input.GetMouseButton(1))
            {
                isReadySkill = false;
                isChange = false;

                effectGo.SetActive(false);
                effectGo = null;
                //Destroy(effectGo);

                if (setSkill.skillSlots[_index] != null && setSkill.skillSlots[_index].isSkillOn)
                {
                    UseSkill(() => setSkill.skillSlots[_index]);
                    StartCoroutine(setSkill.SkillCoolTime(setSkill.skillSlots[_index], setSkill.skillUIList[_index]));
                }
            }
        }

        void ActiveSkill()
        {
            if (Input.GetKeyDown("1"))
            {
                if (!isReadySkill)
                {
                    SetSkill(0);
                }
                else 
                {
                    ChangeSkill(0);
                }

            }
            else if (Input.GetKeyDown("2"))
            {
                if (!isReadySkill)
                {
                    SetSkill(1);
                }
                else
                {
                    ChangeSkill(1);
                }
            }
            else if (Input.GetKeyDown("3"))
            {
                if (!isReadySkill)
                {
                    SetSkill(2);
                }
                else
                {
                    ChangeSkill(2);
                }
            }
            else if (Input.GetKeyDown("4"))
            {
                if (!isReadySkill)
                {
                    SetSkill(3);
                }
                else
                {
                    ChangeSkill(3);
                }
            }
        }

        void SetSkill(int _index)
        {
            isReadySkill = true;

            index = _index;
        }

        void ChangeSkill(int _index)
        {
            isChange = true;

            if (setSkill.skillSlots.Count >= (index + 1))
            {
                if (isChange && !Input.GetKeyDown($"{index + 1}"))
                {
                    index = _index;

                    effectGo.SetActive(false);
                    effectGo = null;
                }
                else if (effectGo != null && Input.GetKeyDown($"{index + 1}"))
                {
                    isReadySkill = false;
                    isChange = false;
                    effectGo.SetActive(false);
                    effectGo = null;

                    Debug.Log("22");

                    return;
                }
            }
            else
            {
                isChange = false;
                isReadySkill = false;
                effectGo.SetActive(false);

                Debug.Log("33");
                return;
            }
        }

        //void UpdateChangeSkill()
        //{
        //    RangeType(setSkill.skillSlots[index]);

        //    UsePlayerSkill(index);

        //}

        void UseSkill(Func<SkillBase> factoryMethod)
        {
            SkillBase skill = factoryMethod();

            if (skill != null)
            {
                if (skill.isSkillOn)
                {
                    isStartAttack = true;

                    skill.Activate();
                    //animator.SetBool("isSkill", true);

                    Vector3 skillPos = Vector3.zero;

                    if (skill.rangeType == SkillRangeType.Circle)
                    {
                        skillPos = new Vector3(RayToScreen().x, skill.skillPrefab.transform.position.y, RayToScreen().z);      
                    }
                    else if (skill.rangeType == SkillRangeType.Cube)
                    {
                        skillPos = new Vector3(transform.position.x + 1f, skill.skillPrefab.transform.position.y, transform.position.z);
                    }

                    skillef = Instantiate(skill.skillPrefab, skillPos, skill.skillPrefab.transform.rotation);
                    //skillef.transform.GetChild(0).GetComponent<SkillAttack>().damage = skill.damage;

                    StartCoroutine(skill.SkillCoolTime());
                    Destroy(skillef, skill.skillAtkTime);
                }

            }
        }

        void CheckSkillRange(GameObject prefab, SkillBase skill)
        {
            Vector3 pos = Vector3.zero;

            if (skill != null && skill.rangeType == SkillRangeType.Circle)
            {
                pos = RayToScreen() + new Vector3(0f, 0.1f, 0f);

                if (prefab != null && effectGo == null)
                {
                    skillRange_Circle.SetActive(true);

                    effectGo = skillRange_Circle;
                }

                if (effectGo != null)
                {
                    effectGo.transform.position = pos;
                }
            }
            else if (skill != null && skill.rangeType == SkillRangeType.Cube)
            {
                if (prefab != null && effectGo == null)
                {
                    skillRange_Cube.SetActive(true);

                    effectGo = skillRange_Cube;

                    //effectGo = Instantiate(prefab, transform.position, UpdateSkillRangeRotation());
                }

                if (effectGo != null)
                {
                    effectGo.transform.position = transform.position;
                    effectGo.transform.rotation = UpdateSkillRangeRotation();
                }
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

        Quaternion UpdateSkillRangeRotation()
        {
            // 마우스 위치 가져오기
            Vector3 mousePosition = Input.mousePosition;
            Quaternion targetRotation = Quaternion.identity;

            // 마우스 위치를 월드 좌표로 변환
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // 방향 계산 (오브젝트 위치 -> 마우스 위치)
                Vector3 direction = hit.point - transform.position;

                // 방향에 따라 회전 적용
                targetRotation = Quaternion.LookRotation(direction);

                return targetRotation;
            }

            return targetRotation;
        }

        void RangeType(SkillBase skill)
        {
            switch (skill.rangeType)
            {
                case SkillRangeType.Circle:   
                    CheckSkillRange(skillRange_Circle, skill);
                    break;      
                case SkillRangeType.Cube:
                    CheckSkillRange(skillRange_Cube, skill);
                    break;
                
            }
        }
    }
}