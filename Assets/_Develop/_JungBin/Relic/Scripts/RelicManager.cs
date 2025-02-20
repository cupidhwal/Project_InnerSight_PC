using Noah;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JungBin
{

    public class RelicManager : MonoBehaviour
    {
        private List<IRelic> relics = new List<IRelic>();
        private IRelic selectedRelic; // 마지막으로 선택한 유물 저장

        [SerializeField] private TextMeshProUGUI relicName;      //유물의 이름을 나타내는 텍스트 오브젝트
        [SerializeField] private TextMeshProUGUI relicDescription;      //유물 설명을 해줄 텍스트 오브젝트
        [SerializeField] private GameObject relicSelectUI;
        [SerializeField] private Image applyImage;

        public GameObject trinketUIParent; // 모든 버튼이 포함된 부모 오브젝트
        private Dictionary<string, GameObject> trinketButtons = new Dictionary<string, GameObject>();

        private Sprite sourceImage;

        public static RelicManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // 🔹 `LoadAll()`이 실행된 후 유물 목록을 `RelicManager`에 적용
            foreach (var relicEntry in SaveLoadManager.Instance.relicSaveData.relics)
            {
                IRelic relic = RelicFactory.CreateRelic(relicEntry.relicID); // 🔹 영어 ID를 사용하여 유물 생성
                if (relic != null)
                {
                    relics.Add(relic);
                    relic.ApplyEffect(GameManager.Instance.Player);
                }
            }

            // 🔹 모든 자식 오브젝트를 찾아 Dictionary에 저장
            foreach (Transform child in trinketUIParent.transform)
            {
                trinketButtons[child.name] = child.gameObject;
                child.gameObject.SetActive(false); // 시작 시 모든 버튼 비활성화
            }

            // 🔹 저장된 유물 UI 자동 활성화
            LoadRelicUI();

        }

        // 유물 추가
        public void AddRelic(IRelic relic, Player player)
        {
            relics.Add(relic);
            relic.ApplyEffect(player);
            Debug.Log($"{relic.RelicName} 유물의 효과 : {relic.Description}");

            // 🔹 이미 저장된 유물이 아니라면 추가
            if (!SaveLoadManager.Instance.relicSaveData.relics.Exists(r => r.relicID == relic.RelicID))
            {
                SaveLoadManager.Instance.relicSaveData.relics.Add(new RelicDataEntry(relic.RelicID, relic.RelicName));
            }

            ActivateTrinketButton(relic.RelicID);

            // 🔹 유물 획득 시 즉시 저장하도록 추가
            SaveLoadManager.Instance.SaveRelics();
        }

        // 현재 유물 목록 반환
        public List<IRelic> GetRelics()
        {
            return relics;
        }

        // 현재 적용 중인 유물이 있는지 확인
        public bool HasActiveRelic()
        {
            return selectedRelic != null;
        }

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

        public void SelectRelicButton()
        {
            ApplyRelicEffect(selectedRelic, GameManager.Instance.Player);
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

        // 유물 효과 적용 (새 유물 선택 시 이전 효과 제거)
        public void ApplyRelicEffect(IRelic newRelic, Player player)
        {
            // 이전 유물 효과 제거
            if (selectedRelic != null)
            {
                selectedRelic.RemoveEffect(player);
            }

            // 새 유물 효과 적용
            selectedRelic = newRelic;
            selectedRelic.ApplyEffect(player);
            applyImage.sprite = sourceImage;

            Debug.Log($"{selectedRelic.RelicName} 유물 효과가 적용되었습니다.");

            relicSelectUI.SetActive(false);
        }

        // 🔹 게임 시작 시 저장된 유물 UI 버튼 활성화
        private void LoadRelicUI()
        {
            foreach (var relicEntry in SaveLoadManager.Instance.relicSaveData.relics)
            {
                ActivateTrinketButton(relicEntry.relicID); // 🔹 영어 ID 사용하여 UI 버튼 활성화
            }
        }

        // 🔹 유물 획득 시 해당하는 버튼 활성화
        public void ActivateTrinketButton(string relicID)
        {
            if (trinketButtons.ContainsKey(relicID))
            {
                trinketButtons[relicID].SetActive(true);
                Debug.Log($"{relicID} 버튼이 활성화되었습니다!");
            }
            else
            {
                Debug.LogWarning($"UI 버튼을 찾을 수 없습니다: {relicID}");
            }
        }

        public void CloseRelicUI()
        {
            relicSelectUI.SetActive(false);
        }


    }
}