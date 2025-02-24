using Noah;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JungBin
{
    /// <summary>
    /// 유물 관리 클래스: 유물의 생성, 적용, UI 연동 등의 기능 수행
    /// </summary>
    public class RelicManager : MonoBehaviour
    {
        private List<IRelic> relics = new List<IRelic>(); // 현재 보유 중인 유물 리스트
        private IRelic selectedRelic; // 마지막으로 선택한 유물 저장

        [SerializeField] private TextMeshProUGUI relicName;
        [SerializeField] private TextMeshProUGUI relicDescription;
        [SerializeField] private GameObject relicSelectUI;
        [SerializeField] private Image applyImage;

        public GameObject trinketUIParent;
        private Dictionary<string, GameObject> trinketButtons = new Dictionary<string, GameObject>();

        private Sprite sourceImage;

        public static RelicManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            foreach (var relicEntry in SaveLoadManager.Instance.relicSaveData.relics)
            {
                IRelic relic = RelicFactory.CreateRelic(relicEntry.relicID);
                if (relic != null)
                {
                    relics.Add(relic);
                    RelicEffectManager.RegisterEffect(relic.RelicID, relic.ApplyEffect, relic.RemoveEffect);
                    RelicEffectManager.ApplyEffect(relic.RelicID);
                }
            }

            LoadRelicUI();
        }

        public void AddRelic(IRelic relic, Player player)
        {
            relics.Add(relic);
            RelicEffectManager.RegisterEffect(relic.RelicID, relic.ApplyEffect, relic.RemoveEffect);
            RelicEffectManager.ApplyEffect(relic.RelicID);

            SaveLoadManager.Instance.relicSaveData.relics.Add(new RelicDataEntry(relic.RelicID, relic.RelicName));
            SaveLoadManager.Instance.SaveRelics();

            ActivateTrinketButton(relic.RelicID);
        }

        public List<IRelic> GetRelics() => relics;

        public void ClickRelicButton(string name)
        {
            GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
            if (clickedObject != null)
            {
                Image image = clickedObject.transform.GetChild(0).GetComponent<Image>();
                if (image != null)
                {
                    sourceImage = image.sprite;
                }
            }
            ShowRelicDescription(name);
        }
        public IRelic ShowRelicDescription(string name)
        {
            Debug.Log("클릭");
            if (relicDescription != null)
            {
                foreach (var relic in GetRelics())
                {
                    if (name == relic.RelicName)
                    {
                        relicDescription.gameObject.SetActive(true);
                        relicName.text = relic.RelicName;
                        relicDescription.text = relic.Description;

                        selectedRelic = relic; // 선택된 유물 저장
                        return relic;
                    }
                }
            }
            Debug.LogWarning("선택한 유물이 없습니다.");
            return null;
        }

        public void SelectRelicButton()
        {
            ApplyRelicEffect(selectedRelic, GameManager.Instance.Player);
        }

        public void ApplyRelicEffect(IRelic newRelic, Player player)
        {
            if (selectedRelic != null)
                RelicEffectManager.RemoveEffect(selectedRelic.RelicID);

            selectedRelic = newRelic;
            RelicEffectManager.ApplyEffect(newRelic.RelicID);
            applyImage.sprite = sourceImage;
            relicSelectUI.SetActive(false);
        }

        private void LoadRelicUI()
        {
            foreach (var relicEntry in SaveLoadManager.Instance.relicSaveData.relics)
            {
                ActivateTrinketButton(relicEntry.relicID);
            }
        }

        public void ActivateTrinketButton(string relicID)
        {
            if (trinketButtons.ContainsKey(relicID))
                trinketButtons[relicID].SetActive(true);
        }

        public void CloseRelicUI()
        {
            relicSelectUI.SetActive(false);
        }
    }
}
