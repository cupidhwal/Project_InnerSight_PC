using System;
using UnityEngine;

namespace Noah
{ 
    public class PlayerUseSkill : MonoBehaviour
    {
        private bool isStartAttack = false;
        public InGameUI_Skill setSkill;
        [SerializeField] private GameObject skillRange_Circle;
        [SerializeField] private GameObject skillRange_Cube;

        private GameObject effectGo;
        private GameObject skillef;

        private bool isReadySkill = false;
        private bool isChange = false;

        private int index = 0;

        public float rotationSpeed = 100f; // 회전 속도

        public float y_SkillRot = 100f;

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
            //skillRange_Circle = transform.GetChild(0).gameObject;
            //skillRange_Cube = transform.GetChild(1).gameObject;
        }

        void UsePlayerSkill(int _index)
        {
            if (Input.GetMouseButton(1))
            {
                isReadySkill = false;
                isChange = false;


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
            if (Input.GetKeyDown("1") && setSkill.skillSlots[0].isSkillOn)
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
            else if (Input.GetKeyDown("2") && setSkill.skillSlots[1].isSkillOn)
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
            else if (Input.GetKeyDown("3") && setSkill.skillSlots[2].isSkillOn)
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
            else if (Input.GetKeyDown("4") && setSkill.skillSlots[3].isSkillOn)
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

                    // Y축 회전만 적용
                    Quaternion fullRotation = RayManager.Instance.UpdateSkillRangeRotation();
                    Vector3 eulerRotation = fullRotation.eulerAngles; // 모든 축의 회전 값 가져오기
                    Quaternion yOnlyRotation = Quaternion.Euler(0f, eulerRotation.y, 0f); // Y축 회전만 적용

                    if (skill.rangeType == SkillRangeType.Circle)
                    {
                        skillPos = new Vector3(RayManager.Instance.RayToScreen().x, skill.skillPrefab.transform.position.y, RayManager.Instance.RayToScreen().z);

                        skillef = Instantiate(skill.skillPrefab, skillPos, skill.skillPrefab.transform.rotation);
                    }
                    else if (skill.rangeType == SkillRangeType.Cube)
                    {
                        float yRot;

                        yRot = transform.position.z + skill.skillPos.z;

                        skillPos = new Vector3(transform.position.x + skill.skillPos.x, skill.skillPrefab.transform.position.y, yRot);

                        skillef = Instantiate(skill.skillPrefab, skillPos, yOnlyRotation);
                    }

                    //skillef.transform.GetChild(0).GetComponent<SkillAttack>().damage = skill.damage;

                    StartCoroutine(skill.SkillCoolTime());
                    Destroy(skillef, skill.skillAtkTime);
                    effectGo.SetActive(false);
                    effectGo = null;
                }

            }
        }

        void CheckSkillRange(GameObject prefab, SkillBase skill)
        {
            Vector3 pos = Vector3.zero;

            if (skill != null && skill.rangeType == SkillRangeType.Circle)
            {
                pos = RayManager.Instance.RayToScreen() + new Vector3(0f, 0.1f, 0f);

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
                }

                if (effectGo != null)
                {
                    effectGo.transform.position = transform.position;

                    // Y축 회전만 적용
                    Quaternion fullRotation = RayManager.Instance.UpdateSkillRangeRotation();
                    Vector3 eulerRotation = fullRotation.eulerAngles; // 모든 축의 회전 값 가져오기
                    Quaternion yOnlyRotation = Quaternion.Euler(0f, eulerRotation.y, 0f); // Y축 회전만 적용

                    effectGo.transform.rotation = yOnlyRotation;
                }
            }



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