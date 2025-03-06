using UnityEngine;
using Seti;
using static Seti.Damagable;
using System;
//using Noah;

namespace Yoon
{

    public class DamageText : MonoBehaviour
    {

        public GameObject damageTextPrefab;     // DamageText 프리팹 연결
        public Transform fightWorldCanvas;      // DamageText가 생성될 부모 FightWorldCanvas

        private Player player;
        private Damagable damagable;

        private void Start()
        {
            damagable = GetComponent<Damagable>();
            player = InitializeManager.Instance.Player;
            //damagable.OnReceiveDamage += OnTakeDamage;
        }


        // 데미지를 받은 경우 호출되는 메서드
        public  void OnTakeDamage(DamageMessage data)
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
            if (damageTextPrefab == null)
            {
                Debug.LogError("🚨 damageTextPrefab이 연결되지 않았습니다!");
                return;
            }

            if (fightWorldCanvas == null)
            {
                Debug.LogError("🚨 fightWorldCanvas가 설정되지 않았습니다!");
                return;
            }

            // 플레이어가 존재하는지 확인
            if (player == null)
            {
                Debug.LogError("🚨 플레이어 객체가 설정되지 않았습니다!");
                return;
            }




        }
    }
    
}
