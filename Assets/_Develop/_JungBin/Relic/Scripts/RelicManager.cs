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
        private List<IRelic> relics = new List<IRelic>(); // 🔹 현재 보유 중인 유물 리스트
        private IRelic selectedRelic; // 🔹 마지막으로 결정한 유물 저장
        private IRelic clickRelic; // 🔹 마지막으로 선택한 유물 저장

        [SerializeField] private TextMeshProUGUI relicName; // 🔹 UI에서 유물 이름을 표시하는 텍스트
        [SerializeField] private TextMeshProUGUI relicDescription; // 🔹 UI에서 유물 설명을 표시하는 텍스트
        [SerializeField] private GameObject relicSelectUI; // 🔹 유물 선택 UI 패널
        [SerializeField] private Image applyImage; // 🔹 유물 선택 시 표시되는 아이콘 이미지
        [SerializeField] private Image equippedRelicImage; // 🔹 현재 장착된 유물의 이미지 표시용
        [SerializeField] private GameObject closeButton; // 🔹 유물 UI 닫기 버튼

        public GameObject trinketUIParent; // 🔹 유물 버튼들이 포함된 UI 부모 객체
        private Dictionary<string, GameObject> trinketButtons = new Dictionary<string, GameObject>(); // 🔹 유물 버튼 딕셔너리

        private Sprite sourceImage; // 🔹 선택한 유물의 이미지 저장

        public static RelicManager Instance { get; private set; } // 🔹 싱글톤 인스턴스

        /// <summary>
        /// 싱글톤 패턴 적용: 하나의 인스턴스만 유지되도록 설정
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject); // 중복 방지
        }

        /// <summary>
        /// 게임 시작 시 저장된 유물 데이터를 불러오고, UI를 초기화
        /// </summary>
        private void Start()
        {
            // 🔹 모든 UI 버튼을 기본적으로 비활성화
            foreach (Transform child in trinketUIParent.transform)
            {
                Debug.Log("유물 UI버튼 비활성화");
                child.gameObject.SetActive(false);
            }

            // 🔹 저장된 유물 데이터를 불러와 적용
            foreach (var relicEntry in SaveLoadManager.Instance.relicSaveData.relics)
            {
                IRelic relic = RelicFactory.CreateRelic(relicEntry.relicID);
                Debug.Log($"Relic 데이터 : {relic.RelicID}");
                if (relic != null)
                {
                    Debug.Log("RelicManager 저장된 유물 데이터 적용");
                    relics.Add(relic);
                    RelicEffectManager.RegisterEffect(relic.RelicID, relic.ApplyEffect, relic.RemoveEffect);
                }
            }

            closeButton.SetActive(false);
            LoadRelicUI(); // 🔹 UI 업데이트
        }

        /// <summary>
        /// 새로운 유물을 추가하고 저장하며, 효과를 적용하는 메서드
        /// </summary>
        /// <param name="relic">추가할 유물 객체</param>
        /// <param name="player">플레이어 객체</param>
        public void AddRelic(IRelic relic, Player player)
        {
            relics.Add(relic);
            RelicEffectManager.RegisterEffect(relic.RelicID, relic.ApplyEffect, relic.RemoveEffect);

            // 🔹 중복 저장 방지
            if (!SaveLoadManager.Instance.relicSaveData.relics.Exists(r => r.relicID == relic.RelicID))
            {
                SaveLoadManager.Instance.relicSaveData.relics.Add(new RelicDataEntry(relic.RelicID, relic.RelicName));
                SaveLoadManager.Instance.SaveRelics();
            }

            ActivateTrinketButton(relic.RelicID); // 🔹 UI 버튼 활성화
        }

        /// <summary>
        /// 현재 보유 중인 유물 리스트 반환
        /// </summary>
        /// <returns>보유한 유물 리스트</returns>
        public List<IRelic> GetRelics() => relics;

        /// <summary>
        /// 유물 UI 버튼 클릭 시 실행되는 메서드
        /// </summary>
        /// <param name="name">클릭한 유물의 이름</param>
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

        /// <summary>
        /// 유물의 설명을 UI에 표시하는 메서드
        /// </summary>
        /// 
        /// <param name="name">유물 이름</param>
        /// <returns>찾은 유물 객체</returns>
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
                        closeButton.SetActive(true);

                        clickRelic = relic; // 🔹 선택된 유물 저장
                        return relic;
                    }
                }
            }
            Debug.LogWarning("선택한 유물이 없습니다.");
            return null;
        }

        /// <summary>
        /// 선택된 유물의 효과를 적용하는 버튼 (UI에서 클릭 시 실행)
        /// </summary>
        public void SelectRelicButton()
        {
            ApplyRelicEffect(clickRelic, GameManager.Instance.Player);

            // 🔹 장착한 유물의 이미지를 업데이트
            UpdateEquippedRelicImage(clickRelic);
        }

        /// <summary>
        /// 유물의 효과를 적용하고, 기존 유물 효과를 제거하는 메서드
        /// </summary>
        /// <param name="newRelic">새롭게 장착할 유물</param>
        /// <param name="player">플레이어 객체</param>
        public void ApplyRelicEffect(IRelic newRelic, Player player)
        {
            // 🔹 기존 유물이 있을 경우 효과 제거
            if (selectedRelic != null && RelicEffectManager.HasEffect(selectedRelic.RelicID))
            {
                Debug.Log($"🔹 기존 유물 효과 제거: {selectedRelic.RelicID}");
                RelicEffectManager.RemoveEffect(selectedRelic.RelicID);
            }

            // 🔹 새로운 유물 설정
            selectedRelic = newRelic;

            // 🔹 유물 효과 적용 (효과가 등록된 경우만 실행)
            if (RelicEffectManager.HasEffect(newRelic.RelicID))
            {
                Debug.Log($"✅ 유물 효과 적용: {newRelic.RelicID}");
                RelicEffectManager.ApplyEffect(newRelic.RelicID);
            }
            else
            {
                Debug.LogWarning($"⚠️ [ApplyRelicEffect] {newRelic.RelicID} 효과가 등록되지 않음.");
            }

            closeButton.SetActive (false);
            relicSelectUI.SetActive(false);
        }


        /// <summary>
        /// 현재 장착된 유물의 이미지를 변경하는 메서드
        /// </summary>
        /// <param name="selectedRelic">선택한 유물</param>
        private void UpdateEquippedRelicImage(IRelic selectedRelic)
        {
            if (selectedRelic == null) return;

            // 🔹 선택한 유물의 UI 버튼에서 첫 번째 자식 오브젝트의 Image 컴포넌트를 가져오기
            foreach (Transform child in trinketUIParent.transform)
            {
                if (child.name == selectedRelic.RelicID) // 🔹 선택한 유물과 같은 이름의 버튼 찾기
                {
                    Image relicButtonImage = child.GetChild(0).GetComponent<Image>(); // 첫 번째 자식의 Image 가져오기
                    if (relicButtonImage != null)
                    {
                        // 🔹 인스펙터에서 받아온 `equippedRelicImage`를 선택한 유물 이미지로 변경
                        equippedRelicImage.sprite = relicButtonImage.sprite;
                        Debug.Log($"🔹 장착한 유물 이미지 변경됨: {selectedRelic.RelicName}");
                    }
                    else
                    {
                        Debug.LogWarning("❌ 선택한 유물 버튼에 Image가 없음.");
                    }
                    break; // 🔹 유물 찾았으므로 루프 종료
                }
            }
        }


        /// <summary>
        /// 저장된 유물 데이터를 기반으로 UI 버튼을 활성화하는 메서드
        /// </summary>
        private void LoadRelicUI()
        {
            foreach (var relic in GetRelics())
            {
                foreach (Transform child in trinketUIParent.transform)
                {
                    if (child.name == relic.RelicID) // 🔹 유물의 이름과 UI 오브젝트 이름이 같다면
                    {
                        child.gameObject.SetActive(true); // 🔹 해당 버튼을 활성화
                        break; // 🔹 일치하는 버튼을 찾았으므로 더 이상 반복할 필요 없음
                    }
                }
            }
        }


        /// <summary>
        /// 특정 유물의 UI 버튼을 활성화하는 메서드
        /// </summary>
        /// <param name="relicID">활성화할 유물의 ID</param>
        public void ActivateTrinketButton(string RelicID)
        {
            foreach (Transform child in trinketUIParent.transform)
            {
                if (child.name == RelicID) // 🔹 유물의 이름과 UI 오브젝트 이름이 같다면
                {
                    child.gameObject.SetActive(true); // 🔹 해당 버튼을 활성화
                    break; // 🔹 일치하는 버튼을 찾았으므로 더 이상 반복할 필요 없음
                }
            }

        }

        /// <summary>
        /// 유물 선택 UI를 닫는 메서드
        /// </summary>
        public void CloseRelicUI()
        {
            closeButton.SetActive(false);
            relicSelectUI.SetActive(false);
        }
    }
}
