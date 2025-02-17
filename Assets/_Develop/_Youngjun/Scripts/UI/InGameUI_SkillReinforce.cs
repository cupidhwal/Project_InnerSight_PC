using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Noah
{
    public class InGameUI_SkillReinforce : MonoBehaviour
    {
        public Transform skillContentPar;
        public GameObject skillContent;

        public List<Transform> m_SkillList = new List<Transform>();

        InGameUI_Skill inGameUI_Skill;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void OnEnable()
        {
            inGameUI_Skill = InGameUI_Skill.instance;
        }

        public void SetSkill()
        {
            foreach (var skill in inGameUI_Skill.skillSlots)
            {
                GameObject sk_Content = Instantiate(skillContent, skillContentPar.position,
                    skillContentPar.rotation, skillContentPar);

                Transform contentsGroup = sk_Content.transform.GetChild(0);

                contentsGroup.GetChild(0).GetComponent<Image>().sprite = skill.UISprite;
                contentsGroup.GetChild(1).GetComponent<TMP_Text>().text = skill.skillName;
                contentsGroup.GetChild(2).GetComponent<TMP_Text>().text = skill.skillDescription;

                m_SkillList.Add(contentsGroup);    
            }

            ButtonsAddListner(m_SkillList);
        }


        void ButtonsAddListner(List<Transform> _skills)
        {
            for (int i = 0; i < _skills.Count; i++)
            {
                int index = i;

                _skills[i].GetComponent<Button>().onClick.AddListener(() => SkillReinforce(index));
            }

        }

        void RemoveListener()
        {
            for (int i = 0; i < skillContentPar.childCount; i++)
            {
                Destroy(skillContentPar.GetChild(i).gameObject);          
            }

            m_SkillList.Clear();

            gameObject.SetActive(false);
        }

        void SkillReinforce(int _skillIndex)
        {
            Debug.Log($"{inGameUI_Skill.skillSlots[_skillIndex]} 업그레이드");

            inGameUI_Skill.skillSlots[_skillIndex].damage += inGameUI_Skill.skillSlots[_skillIndex].upgradeDamage;
            inGameUI_Skill.UpgradeCheck(inGameUI_Skill.skillSlots[_skillIndex], _skillIndex);

            ResetBtnData();   
        }

        void ResetBtnData()
        {
            Time.timeScale = 1f;

            RemoveListener();
        }
    }
}