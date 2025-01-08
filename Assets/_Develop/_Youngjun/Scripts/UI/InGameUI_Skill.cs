using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

namespace Noah
{
    public class InGameUI_Skill : MonoBehaviour
    {
        public static InGameUI_Skill instance;

        private Transform skliiObject;
        [SerializeField] private GameObject selectUI;

        public List<SkillBase> skills = new List<SkillBase>();
        private List<SkillBase> randomSkills = new List<SkillBase>(); // 랜덤으로 선택된 스킬 목록
        private List<Image> skillUIList = new List<Image>(); // UI 슬롯 리스트
        private List<SkillBase> skillSlots = new List<SkillBase>();

        public SkillBase firstSkill;
        public SkillBase secondSkill;
        public SkillBase thirdSkill;
        public SkillBase fourthSkill;

        public Button btn1, btn2, btn3;
        public Button selectBtn1, selectBtn2;

        private List<int> randonNum = new List<int>();

        private int selectSkillNum;

        public Image firstSkillUI, secondSkillUI, thridSkillUI, fourthSkillUI;

        // 플레이어 스킬
        public FireWallSkill fireWall;
        public Meteoros meteo;
        public FireHand fireHand;
        public FireBomb fireBomb;
        public FireNapalm fireNapalm;
        public FireIncendiary fireIncendiary;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            Init();
        }

        void Init()
        {
            skliiObject = transform.GetChild(0);

            skills.Add(fireWall);
            skills.Add(meteo);
            skills.Add(fireHand);
            skills.Add(fireBomb);
            skills.Add(fireNapalm);
            skills.Add(fireIncendiary);

            // UI 슬롯을 초기화 (Unity 에디터에서 설정한 UI Image 배열)
            skillUIList.Add(firstSkillUI);
            skillUIList.Add(secondSkillUI);
            skillUIList.Add(thridSkillUI); // 스킬 UI를 추가로 늘릴 수 있음
            skillUIList.Add(fourthSkillUI); // 스킬 UI를 추가로 늘릴 수 있음

        }

        // 만약 x 키에 데이터가 있으면 c 키에 스킬 넣기
        // 동일한 스킬 선택 시 데미지 증가 => 선택 확인 여부 UI
        // 이미 x키와c에 스킬이 있을 때 스킬 선택 시 둘과 다른 스킬 선택 시 스킬 변경 => 선택 확인 여부 UI
        // 기존 스킬 데이터는 남기고 변경
        // 데이터를 PlayerAttack 스크립트에 적용

        public void GetRandomSkill()
        {
            // 랜덤 숫자 index 3개를 뽑음
            // 리스트에 있는 인덱스 번호 3개를 각 버튼으로 넘기기

            while (randomSkills.Count < 3)
            {
                int randomIndex = UnityEngine.Random.Range(0, skills.Count);

                if (!randomSkills.Contains(skills[randomIndex]))
                {
                    randomSkills.Add(skills[randomIndex]);
                }
            }

            btn1.onClick.AddListener(() => AssignSkillToKey(0)); // 첫 번째 랜덤 스킬을 버튼1에 할당
            btn2.onClick.AddListener(() => AssignSkillToKey(1)); // 두 번째 랜덤 스킬을 버튼2에 할당
            btn3.onClick.AddListener(() => AssignSkillToKey(2)); // 세 번째 랜덤 스킬을 버튼3에 할당

            // 랜덤 스킬 이미지 할당
            btn1.transform.GetChild(1).GetComponent<Image>().sprite = randomSkills[0].skillImage;
            btn2.transform.GetChild(1).GetComponent<Image>().sprite = randomSkills[1].skillImage;
            btn3.transform.GetChild(1).GetComponent<Image>().sprite = randomSkills[2].skillImage;
        }

        // 선택된 스킬을 X와 C 키에 할당
        //void AssignSkillToKey(int skillIndex)
        //{
        //    if (firstSkill == null)
        //    {
        //        firstSkill = randomSkills[skillIndex];
        //        SetSkillUI(firstSkillUI, firstSkill);
        //        ResetBtnData();

        //        Debug.Log("Skill assigned to X key: " + firstSkill.GetType().Name);
        //    }
        //    else if (secondSkill == null && randomSkills[skillIndex] != firstSkill)
        //    {
        //        secondSkill = randomSkills[skillIndex];
        //        SetSkillUI(secondSkillUI, secondSkill);
        //        ResetBtnData();

        //        Debug.Log("Skill assigned to C key: " + secondSkill.GetType().Name);
        //    }
        //    else if (firstSkill != null && randomSkills[skillIndex] == firstSkill)
        //    {
        //        Debug.Log($"{firstSkill} + {randomSkills[skillIndex]} 업그레이드");
        //        firstSkill.damage += firstSkill.upgradeDamage;
        //        ResetBtnData();
        //    }
        //    else if (firstSkill != null && randomSkills[skillIndex] == secondSkill)
        //    {
        //        Debug.Log($"{secondSkill} + {randomSkills[skillIndex]} 업그레이드");
        //        secondSkill.damage += secondSkill.upgradeDamage;
        //        ResetBtnData();
        //    }
        //    else if (firstSkill != null && secondSkill != null
        //            && randomSkills[skillIndex] != firstSkill && randomSkills[skillIndex] != secondSkill)
        //    {
        //        selectSkillNum = skillIndex;
        //        SetSelectUI();
        //    }
        //}

        void AssignSkillToKey(int skillIndex)
        {
            int maxSkillSlots = skillUIList.Count; // UI 슬롯 개수에 따라 최대 슬롯 개수 결정

            // 이미 슬롯에 스킬이 있는 경우 업그레이드 처리
            for (int i = 0; i < skillSlots.Count; i++)
            {
                if (skillSlots[i] == randomSkills[skillIndex])
                {
                    Debug.Log($"{skillSlots[i]} + {randomSkills[skillIndex]} 업그레이드");
                    skillSlots[i].damage += skillSlots[i].upgradeDamage;
                    ResetBtnData();
                    return;
                }
            }

            // 빈 슬롯이 있는 경우 스킬 추가
            if (skillSlots.Count < maxSkillSlots)
            {
                skillSlots.Add(randomSkills[skillIndex]);
                SetSkillUI(skillUIList[skillSlots.Count - 1], randomSkills[skillIndex]); // UI 갱신
                ResetBtnData();
                //Debug.Log($"Skill assigned to slot {skillSlots.Count}: {randomSkills[skillIndex].GetType().Name}");
            }
            else
            {
                // 슬롯이 가득 찬 경우, 선택 UI 표시
                selectSkillNum = skillIndex;
                SetSelectUI();
            }
        }

        void SkillCheck(SkillBase skill, Image skillUI ,int index)
        {
            if (skill == null)
            {
                skill = randomSkills[index];
                SetSkillUI(skillUI, skill);
                ResetBtnData();
                Debug.Log("Skill assigned to key: " + skill.GetType().Name);
            }
        }

        void UpgradeCheck()
        { 
            
        }

        void ResetBtnData()
        {
            Time.timeScale = 1f;

            randomSkills.Clear();
            RemoveListener();
            skliiObject.gameObject.SetActive(false);
        }

        void RemoveListener()
        {
            btn1.onClick.RemoveAllListeners();
            btn2.onClick.RemoveAllListeners();
            btn3.onClick.RemoveAllListeners();

        }

        void RemoveChangeListener()
        {
            selectBtn1.onClick.RemoveAllListeners();
            selectBtn2.onClick.RemoveAllListeners();
        }

        public void UIBack()
        {
            Time.timeScale = 1f;
            randomSkills.Clear();
            RemoveListener();
            skliiObject.gameObject.SetActive(false);
        }


        void SetSelectUI()
        {
            selectUI.SetActive(true);
            selectBtn1.transform.GetChild(1).GetComponent<Image>().sprite = firstSkill.skillImage;
            selectBtn2.transform.GetChild(1).GetComponent<Image>().sprite = secondSkill.skillImage;

            selectBtn1.onClick.AddListener(() => ChangeSkill(ref firstSkill));
            selectBtn2.onClick.AddListener(() => ChangeSkill(ref secondSkill));
        }

        void ChangeSkill(ref SkillBase skill)
        {
            skill.damage = skill.upgradeDamage;

            skill = randomSkills[selectSkillNum];

            if(skill == firstSkill)
            {
                SetSkillUI(firstSkillUI, skill);
            }
            else if(skill == secondSkill)
            {
                SetSkillUI(secondSkillUI, skill);
            }


            Debug.Log(randomSkills[selectSkillNum].ToString());
            Debug.Log(skill);

            selectUI.SetActive(false);
            UIBack();
            RemoveChangeListener();
        }

        public void SelectUIBack()
        {
            selectUI.SetActive(false);
            RemoveChangeListener();
        }
        

        void SetSkillUI(Image skillUI ,SkillBase skill)
        {
            skillUI.sprite = skill.UISprite;
        }


        public IEnumerator SkillCoolTime(SkillBase skill)
        {
            float countdown = 0f;
            float coolText;
            Image skillUI;


            if (skill.isSkillOn == false)
            {
                //isCooldown = true;

                //Debug.Log("스킬사용");

                if (skill == firstSkill)
                {
                    skillUI = firstSkillUI;
                }
                else
                {
                    skillUI = secondSkillUI;
                }

                skillUI.transform.GetChild(0).GetComponent<Image>().fillAmount = 1;

                coolText = skill.cooldown;

                skillUI.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

                while (coolText > countdown)
                {
                    coolText -= Time.deltaTime;

          
                    skillUI.transform.GetChild(0).GetComponent<Image>().fillAmount = coolText / skill.cooldown;

                    skillUI.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = coolText.ToString("0");



                    yield return null;
                }


                skillUI.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);

                //countdown = 0f;
                //isCooldown = false;
            }
        }
    }
}