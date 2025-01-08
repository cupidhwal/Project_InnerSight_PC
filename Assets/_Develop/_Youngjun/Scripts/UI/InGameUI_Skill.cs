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
        private List<SkillBase> randomSkills = new List<SkillBase>(); // �������� ���õ� ��ų ���
        private List<Image> skillUIList = new List<Image>(); // UI ���� ����Ʈ
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

        // �÷��̾� ��ų
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

            // UI ������ �ʱ�ȭ (Unity �����Ϳ��� ������ UI Image �迭)
            skillUIList.Add(firstSkillUI);
            skillUIList.Add(secondSkillUI);
            skillUIList.Add(thridSkillUI); // ��ų UI�� �߰��� �ø� �� ����
            skillUIList.Add(fourthSkillUI); // ��ų UI�� �߰��� �ø� �� ����

        }

        // ���� x Ű�� �����Ͱ� ������ c Ű�� ��ų �ֱ�
        // ������ ��ų ���� �� ������ ���� => ���� Ȯ�� ���� UI
        // �̹� xŰ��c�� ��ų�� ���� �� ��ų ���� �� �Ѱ� �ٸ� ��ų ���� �� ��ų ���� => ���� Ȯ�� ���� UI
        // ���� ��ų �����ʹ� ����� ����
        // �����͸� PlayerAttack ��ũ��Ʈ�� ����

        public void GetRandomSkill()
        {
            // ���� ���� index 3���� ����
            // ����Ʈ�� �ִ� �ε��� ��ȣ 3���� �� ��ư���� �ѱ��

            while (randomSkills.Count < 3)
            {
                int randomIndex = UnityEngine.Random.Range(0, skills.Count);

                if (!randomSkills.Contains(skills[randomIndex]))
                {
                    randomSkills.Add(skills[randomIndex]);
                }
            }

            btn1.onClick.AddListener(() => AssignSkillToKey(0)); // ù ��° ���� ��ų�� ��ư1�� �Ҵ�
            btn2.onClick.AddListener(() => AssignSkillToKey(1)); // �� ��° ���� ��ų�� ��ư2�� �Ҵ�
            btn3.onClick.AddListener(() => AssignSkillToKey(2)); // �� ��° ���� ��ų�� ��ư3�� �Ҵ�

            // ���� ��ų �̹��� �Ҵ�
            btn1.transform.GetChild(1).GetComponent<Image>().sprite = randomSkills[0].skillImage;
            btn2.transform.GetChild(1).GetComponent<Image>().sprite = randomSkills[1].skillImage;
            btn3.transform.GetChild(1).GetComponent<Image>().sprite = randomSkills[2].skillImage;
        }

        // ���õ� ��ų�� X�� C Ű�� �Ҵ�
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
        //        Debug.Log($"{firstSkill} + {randomSkills[skillIndex]} ���׷��̵�");
        //        firstSkill.damage += firstSkill.upgradeDamage;
        //        ResetBtnData();
        //    }
        //    else if (firstSkill != null && randomSkills[skillIndex] == secondSkill)
        //    {
        //        Debug.Log($"{secondSkill} + {randomSkills[skillIndex]} ���׷��̵�");
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
            int maxSkillSlots = skillUIList.Count; // UI ���� ������ ���� �ִ� ���� ���� ����

            // �̹� ���Կ� ��ų�� �ִ� ��� ���׷��̵� ó��
            for (int i = 0; i < skillSlots.Count; i++)
            {
                if (skillSlots[i] == randomSkills[skillIndex])
                {
                    Debug.Log($"{skillSlots[i]} + {randomSkills[skillIndex]} ���׷��̵�");
                    skillSlots[i].damage += skillSlots[i].upgradeDamage;
                    ResetBtnData();
                    return;
                }
            }

            // �� ������ �ִ� ��� ��ų �߰�
            if (skillSlots.Count < maxSkillSlots)
            {
                skillSlots.Add(randomSkills[skillIndex]);
                SetSkillUI(skillUIList[skillSlots.Count - 1], randomSkills[skillIndex]); // UI ����
                ResetBtnData();
                //Debug.Log($"Skill assigned to slot {skillSlots.Count}: {randomSkills[skillIndex].GetType().Name}");
            }
            else
            {
                // ������ ���� �� ���, ���� UI ǥ��
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

                //Debug.Log("��ų���");

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