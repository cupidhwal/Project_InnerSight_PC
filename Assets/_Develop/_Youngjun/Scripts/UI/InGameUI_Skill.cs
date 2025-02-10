using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Noah
{
    public class InGameUI_Skill : MonoBehaviour
    {
        public static InGameUI_Skill instance;

        // Test
        public Sprite reset;


        public Image changeNewSkill_Image;
        public TMP_Text changeNewSkill_Text;
        private Transform skillBtnsPar;
        private Transform skliiObject;
        [SerializeField] private GameObject selectUI;
        private Transform selectBtnPar;

        public List<SkillBase> skills = new List<SkillBase>();
        private List<SkillBase> randomSkills = new List<SkillBase>(); // 랜덤으로 선택된 스킬 목록
        public List<Image> skillUIList = new List<Image>(); // UI 슬롯 리스트
        public List<SkillBase> skillSlots = new List<SkillBase>();

        //public SkillBase firstSkill;
        //public SkillBase secondSkill;
        //public SkillBase thirdSkill;
        //public SkillBase fourthSkill;

        public List<Button> btns = new List<Button> ();
        public List<Button> changeBtns = new List<Button> ();

        private List<int> randomNum = new List<int>();

        private int selectSkillNum;

        // 플레이어 스킬
        public FireSkill fireSkill;
        public Kunai kunai;
        public MeteorRain meteorRain;
        public LaserFire laserFire;
        public Bomb bomb;
        public BloodSycthe bloodSycthe;

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
            skillBtnsPar = skliiObject.GetChild(0);
            selectBtnPar = selectUI.transform.GetChild(0);

            // 스킬 종류 추가
            skills.Add(fireSkill);
            skills.Add(kunai);
            skills.Add(meteorRain);
            skills.Add(laserFire);
            skills.Add(bomb);
            skills.Add(bloodSycthe);

            for(int i = 0; i < skillBtnsPar.childCount; i++)
            {
                if (skillBtnsPar.GetChild(i).GetChild(0).GetComponent<Button>() == null)
                {
                    continue;
                }
                else
                {
                    btns.Add(skillBtnsPar.GetChild(i).GetChild(0).GetComponent<Button>());
                }
                
            }

            for(int i = 0; i < selectBtnPar.childCount; i++)
            {
                changeBtns.Add(selectBtnPar.GetChild(i).GetChild(0).GetComponent<Button>());
            }

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
                int randomIndex = Random.Range(0, skills.Count);

                if (!randomSkills.Contains(skills[randomIndex]))
                {
                    randomSkills.Add(skills[randomIndex]);
                }
            }

            for (int i = 0; i < randomSkills.Count; i++)
            {
                int index = i; // 로컬 변수로 캡처

                btns[i].onClick.AddListener(() => AssignSkillToKey(index));

                // 랜덤 스킬 이미지 할당
                btns[i].transform.GetChild(0).GetComponent<Image>().sprite = randomSkills[index].skillImage;
                btns[i].transform.GetChild(1).GetComponent<TMP_Text>().text = randomSkills[index].skillName;
                btns[i].transform.GetChild(2).GetComponent<TMP_Text>().text = randomSkills[index].skillDescription;
            }
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
                    UpgradeCheck(skillSlots[i], i);
                    ResetBtnData();
                    return;
                }
            }

            // 빈 슬롯이 있는 경우 스킬 추가
            if (skillSlots.Count < maxSkillSlots)
            {
                skillSlots.Add(randomSkills[skillIndex]);
                SetSkillUI(skillUIList[skillSlots.Count - 1], randomSkills[skillIndex]); // UI 갱신
                skillUIList[skillSlots.Count - 1].transform.GetChild(3).GetComponent<TMP_Text>().text = skillSlots[skillSlots.Count - 1].skillUpgardeCount.ToString();
                ResetBtnData();
                //Debug.Log($"Skill assigned to slot {skillSlots.Count}: {randomSkills[skillIndex].GetType().Name}");
            }
            else
            {
                // 슬롯이 가득 찬 경우, 선택 UI 표시
                selectSkillNum = skillIndex;
                SetSelectUI(randomSkills[skillIndex]);
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

        void UpgradeCheck(SkillBase _skill, int _index)
        {
            _skill.skillUpgardeCount++;

            skillUIList[_index].transform.GetChild(3).GetComponent<TMP_Text>().text = _skill.skillUpgardeCount.ToString();
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
            for(int i = 0; i < btns.Count; i++)
            {
                btns[i].onClick.RemoveAllListeners();
            }
        }

        void RemoveChangeListener()
        {
            for(int i = 0; i < changeBtns.Count; i++)
            {
                changeBtns[i].onClick.RemoveAllListeners();
            }
        }

        public void UIBack()
        {
            Time.timeScale = 1f;
            randomSkills.Clear();
            RemoveListener();
            skliiObject.gameObject.SetActive(false);
        }


        void SetSelectUI(SkillBase _skill)
        {
            selectUI.SetActive(true);

            for (int i = 0; i < changeBtns.Count; i++)
            {
                int index = i;

                SkillBase tmpSkill = skillSlots[index];

                changeBtns[i].transform.GetChild(1).GetComponent<Image>().sprite = skillSlots[index].skillImage;
                changeBtns[i].transform.GetChild(0).GetComponent<TMP_Text>().text = skillSlots[index].skillName;

                changeBtns[i].onClick.AddListener(() => ChangeSkill(ref tmpSkill, index));
            }

            changeNewSkill_Image.sprite = _skill.skillImage;
            changeNewSkill_Text.text = _skill.skillName;
        }

        void ChangeSkill(ref SkillBase skill, int _index)
        {
            skill.damage = skill.upgradeDamage;

            skill = randomSkills[selectSkillNum];

            SetSkillUI(skillUIList[_index], skill);

            skillSlots[_index] = skill;

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


        public IEnumerator SkillCoolTime(SkillBase skill, Image skillUI)
        {
            float countdown = 0f;
            float coolText = skill.cooldown; ;

            if (skill.isSkillOn == false)
            {
                skillUI.transform.GetComponent<Image>().fillAmount = 0f;

                //skillUI.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

                while (countdown < coolText)
                {
                    countdown += Time.deltaTime;

                    skillUI.transform.GetComponent<Image>().fillAmount = countdown / coolText;

                    //skillUI.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = coolText.ToString("0");
                    yield return null;
                }

                skillUI.transform.GetComponent<Image>().fillAmount = 1f;


                //skillUI.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);

                //countdown = 0f;
                //isCooldown = false;
            }
        }

        // Test
        public void ResetSkill()
        {
            skillSlots.Clear();

            foreach (var ui in skillUIList)
            { 
                ui.GetComponent<Image>().sprite = reset;
                ui.transform.GetChild(3).GetComponent<TMP_Text>().text = "-";

            }
        }
    }
}