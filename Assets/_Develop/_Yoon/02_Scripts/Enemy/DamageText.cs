using UnityEngine;
using Seti;
using static Seti.Damagable;
using Noah;

namespace Yoon
{

    public class DamageText : MonoBehaviour
    {

        public GameObject damageTextPrefab;     // DamageText 프리팹 연결
        public Transform fightWorldCanvas;      // DamageText가 생성될 부모 FightWorldCanvas

        public Player player;
        private Damagable damagable;

        private void Start()
        {
            damagable = GetComponent<Damagable>();
            //damagable.OnReceiveDamage += OnTakeDamage;
        }


        // 데미지를 받은 경우 호출되는 메서드
        public void OnTakeDamage(DamageMessage data)
        {
            //DamageAmount(data);    // 데미지 계산 및 출력

            ShowDamageIndicator(data);

            // player.Attack 값을 확인
            //Debug.Log("Player's Attack: " + player.Attack);
        }

        public void DamageAmount(DamageMessage data)
        {
            // Debug.Log로 데미지 양 출력
            Debug.Log($"Enemy received {data.amount} damage.");

            // Damageable의 OnReceiveDamage 이벤트 호출
            damagable.OnReceiveDamage?.Invoke();
        }

        // 데미지 텍스트 표시 메서드
        private void ShowDamageIndicator(DamageMessage data)
        {
            if (damageTextPrefab != null && fightWorldCanvas != null && player != null)
            {
                // DamageText 프리팹 생성
                GameObject damageTextInstance = Instantiate(damageTextPrefab, fightWorldCanvas);

                // DamageText 위치 설정 (적 머리 위)
                damageTextInstance.transform.position = transform.position + new Vector3(0, 2f, 0);

                // 회전 고정 (항상 45도 유지)
                damageTextInstance.transform.rotation = Quaternion.Euler(0f, 45f, 0f);

                // DamageIndicator 스크립트에 데미지 값 전달
                DamageIndicator damageIndicator = damageTextInstance.GetComponent<DamageIndicator>();
                if (damageIndicator != null)
                {
                    damageIndicator.SetDamage(data.amount);
                }

            }

        }
    }
    
}
