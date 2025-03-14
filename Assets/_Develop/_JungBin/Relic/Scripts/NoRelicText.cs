using UnityEngine;

public class NoRelicText : MonoBehaviour
{
    [SerializeField] private GameObject content;

    void Update()
    {
        // 🔥 content가 null이 아니고, 자식 오브젝트가 존재할 경우 체크
        if (content == null) return;

        foreach (Transform child in content.transform)
        {
            if (child.gameObject.activeSelf) // 하나라도 활성화된 오브젝트가 있다면
            {
                this.gameObject.SetActive(false); // 🔥 NoRelicText 비활성화
                return; // 불필요한 반복 방지
            }
        }
    }
}
