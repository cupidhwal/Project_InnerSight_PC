using JungBin;
using Noah;
using Seti;
using UnityEngine;

public class HealObject : MonoBehaviour
{
    private string actionUI_Text = "";
    private bool isContact = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isContact)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                ActionUIManager.Instance.DisableActionUI();

                isContact = false;

                HealPlayer();

                // 🔹 유물 습득 후 오브젝트 삭제
                Destroy(gameObject);
            }
        }
    }

    private void HealPlayer()
    {
        if (GameManager.Instance.Player == null)
        {
            Debug.Log("GameManager.Instance.Player == null");
            return;
        }

        Damagable damagable = GameManager.Instance.Player.GetComponent<Damagable>();
        if (damagable == null)
        {
            Debug.Log("damagable == null");
            return;
        }

        float currentHp = damagable.CurrentHitPoints;
        float maxHp = damagable.MaxHitPoint;
        float healAmount = 30;  // 내림
        if (maxHp == currentHp)
        {
            Debug.Log("생명력 회복 없음");
            return;
        }
        else if (maxHp - currentHp < healAmount)
        {
            damagable.HealReviveHitPoint(maxHp - currentHp);
            Debug.Log("남은 체력 다 회복");
        }
        else if (maxHp - damagable.CurrentHitPoints >= healAmount)
        {
            damagable.HealReviveHitPoint(healAmount); // ✅ 기존 체력 회복 함수 호출
            Debug.Log($"{healAmount} 회복");
        }

        //damagable.HealCurrentHitPoint(healAmount); // ✅ 기존 체력 회복 함수 호출

        Debug.Log($"🔹 체력 강화 효과 적용됨! +{healAmount} HP 증가");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isContact = true;
            actionUI_Text = "회복 하기";
            ActionUIManager.Instance.EnableActionUI(actionUI_Text);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isContact = false;

            ActionUIManager.Instance.DisableActionUI();
        }
    }
}

