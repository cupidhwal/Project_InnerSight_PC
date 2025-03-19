using InnerSight_Kys;
using Seti;
using System;
using UnityEngine;

namespace Noah
{ 
    public class PlayerUseSkill : MonoBehaviour
    {
        private bool isStartAttack = false;

        private Transform skillUI;

        [SerializeField] private GameObject skillRange_Circle;
        [SerializeField] private GameObject skillRange_Cube;

        private GameObject effectGo;
        private GameObject skillef;

        private bool isReadySkill = false;
        private bool isChange = false;

        private int index = 0;

        public float rotationSpeed = 100f; // 회전 속도

        public float y_SkillRot = 100f;

        Vector3 skillPos = Vector3.zero;
        Quaternion yOnlyRotation;

        InGameUI_Skill setSkill;

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
            skillUI = FindAnyObjectByType<InGameUI_Skill>().transform;
            setSkill = skillUI.GetComponent<InGameUI_Skill>();
        }

        public void UseSkillAnimation()
        {
            //UsePlayerSkill(index);
            if (setSkill.skillSlots[index] != null && setSkill.skillSlots[index].isSkillOn)
            {
                UseSkill(() => setSkill.skillSlots[index]);
                StartCoroutine(setSkill.SkillCoolTime(setSkill.skillSlots[index], setSkill.skillUIList[index]));
            }
        }

        void UsePlayerSkill(int _index)
        {
            if (setSkill.skillSlots[_index].isSkillOn)
            {
                if (Input.GetMouseButton(1) && setSkill.skillSlots[_index].rangeType != SkillRangeType.Nomal
                    && setSkill.skillSlots[_index].rangeType != SkillRangeType.Proximity)
                {
                    isReadySkill = false;
                    isChange = false;

                    if (effectGo != null)
                    {
                        effectGo.SetActive(false);
                        effectGo = null;
                    }

                    SkillPosition();
                }
                else if (setSkill.skillSlots[_index].rangeType == SkillRangeType.Proximity)
                {
                    isStartAttack = true;

                    isReadySkill = false;
                    isChange = false;

                    SkillPosition();

                    setSkill.skillSlots[_index].PlayerAnimation(transform, setSkill.skillSlots[_index].animationNum);
                }
                else if (setSkill.skillSlots[_index].rangeType == SkillRangeType.Nomal)
                {
                    isStartAttack = true;

                    isReadySkill = false;
                    isChange = false;

                    if (setSkill.skillSlots[_index] != null && setSkill.skillSlots[_index].isSkillOn)
                    {
                        UseSkill(() => setSkill.skillSlots[_index]);
                        StartCoroutine(setSkill.SkillCoolTime(setSkill.skillSlots[_index], setSkill.skillUIList[_index]));
                    }
                }
            }


        }

        void ActiveSkill()
        {
            if (!isStartAttack)
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

                if (Input.GetMouseButton(0))
                {
                    isReadySkill = false;
                    isChange = false;

                    if (effectGo != null)
                    {
                        effectGo.SetActive(false);
                        effectGo = null;
                    }

                    if (transform.GetComponent<Controller_Input>().BehaviourMap.TryGetValue(typeof(Attack), out var attackBehaviour))
                    {
                        if (attackBehaviour is Attack attack)
                        {
                            attack.isSkillAttack = false;
                        }
                    }

                    return;

                }
            }

        }

        void SetSkill(int _index)
        {

            if (setSkill.skillSlots.Count < (_index + 1))
            {
                isReadySkill = false;
                return;
            }

            if (setSkill.skillSlots[_index].isSkillOn)
            {
                isReadySkill = true;

                index = _index;
            }
        }

        void ChangeSkill(int _index)
        {
            isChange = true;

            if (setSkill.skillSlots.Count >= (index + 1))
            {
                if (isChange && !Input.GetKeyDown($"{index + 1}"))
                {
                    index = _index;

                    if(effectGo != null)
                    {
                        effectGo.SetActive(false);
                        effectGo = null;
                    }

                }
                else if (effectGo != null && Input.GetKeyDown($"{index + 1}"))
                {
                    isReadySkill = false;
                    isChange = false;

                    if (effectGo != null)
                    {
                        effectGo.SetActive(false);
                        effectGo = null;
                    }

                    return;
                }
            }
            else
            {

                isChange = false;
                isReadySkill = false;

                if (effectGo != null)
                {
                    effectGo.SetActive(false);
                }

                return;
            }
        }

        void SkillPosition()
        {
            // Y축 회전만 적용
            Quaternion fullRotation = RayManager.Instance.UpdateSkillRangeRotation();
            Vector3 eulerRotation = fullRotation.eulerAngles; // 모든 축의 회전 값 가져오기
            yOnlyRotation = Quaternion.Euler(0f, eulerRotation.y, 0f); // Y축 회전만 적용

            if (setSkill.skillSlots[index].rangeType == SkillRangeType.Circle)
            {
                skillPos = new Vector3(RayManager.Instance.RayToScreen().x, RayManager.Instance.RayToScreen().y + setSkill.skillSlots[index].skillPos.y, RayManager.Instance.RayToScreen().z);

            }
            else if (setSkill.skillSlots[index].rangeType == SkillRangeType.Cube)
            {
                float yRot;

                yRot = transform.position.z + setSkill.skillSlots[index].skillPos.z;
                skillPos = new Vector3(transform.position.x + setSkill.skillSlots[index].skillPos.x, RayManager.Instance.RayToScreen().y + setSkill.skillSlots[index].skillPos.y, yRot);
            }
        }

        void UseSkill(Func<SkillBase> factoryMethod, Transform _parent = null)
        {
            SkillBase skill = factoryMethod();

            if (skill != null)
            {
                if (skill.isSkillOn)
                {                                               
                    skill.Activate();

                    if (transform.GetComponent<Controller_Input>().BehaviourMap.TryGetValue(typeof(Attack), out var attackBehaviour))
                    {
                        if (attackBehaviour is Attack attack)
                        {
                            attack.isSkillAttack = false;
                        }
                    }


                    if (skill.rangeType == SkillRangeType.Circle)
                    {
                        skillef = Instantiate(skill.skillPrefab, skillPos, skill.skillPrefab.transform.rotation);

                        AudioManager.Instance.Play("PlayerSkillSound");
                    }
                    else if (skill.rangeType == SkillRangeType.Cube)
                    {
                        //float yRot;

                        //yRot = transform.position.z + skill.skillPos.z;

                        //skillPos = new Vector3(transform.position.x + skill.skillPos.x, RayManager.Instance.RayToScreen().y + skill.skillPos.y, yRot);

                        skillef = Instantiate(skill.skillPrefab, skillPos, yOnlyRotation);

                        AudioManager.Instance.Play("PlayerSkillSound");
                    }
                    else if(skill.rangeType == SkillRangeType.Nomal || skill.rangeType == SkillRangeType.Proximity)
                    {
                        // 플레이어가 보는 방향으로 스킬 위치 설정
                        skillPos = transform.position + transform.forward * skill.skillPos.z
                                   + transform.right * skill.skillPos.x
                                   + transform.up * skill.skillPos.y;

                        skillef = Instantiate(skill.skillPrefab, skillPos, Quaternion.identity);

                        AudioManager.Instance.Play("PlayerAtackSound");
                    }

                    if (skillef.transform.GetChild(0).GetComponent<SkillAttack>() != null)
                    {
                        skillef.transform.GetChild(0).GetComponent<SkillAttack>().damage = skill.damage;
                    }

                    StartCoroutine(skill.SkillCoolTime());
                    Destroy(skillef, skill.skillAtkTime);

                    isStartAttack = false;
                }

            }
        }

        void CheckSkillRange(GameObject prefab, SkillBase skill)
        {
            if (skill.isSkillOn)
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

                if (transform.GetComponent<Controller_Input>().BehaviourMap.TryGetValue(typeof(Attack), out var attackBehaviour))
                {
                    if (attackBehaviour is Attack attack)
                    {
                        attack.isSkillAttack = true;
                    }
                }
            }
            else
            {
                return;
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
                case SkillRangeType.Nomal:
                    break;
                
            }
        }
    }
}