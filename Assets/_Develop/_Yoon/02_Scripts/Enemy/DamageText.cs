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
            damagable.OnReceiveDamage += OnTakeDamage;
        }


        // 데미지를 받은 경우 호출되는 메서드
        public void OnTakeDamage()
        {



            //DamageAmount(data);    // 데미지 계산 및 출력
            
            
            ShowDamageIndicator();


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
        private void ShowDamageIndicator()
        {
            if (damageTextPrefab != null && fightWorldCanvas != null && player != null)
            {
                // DamageText 프리팹 생성
                GameObject damageTextInstance = Instantiate(damageTextPrefab, fightWorldCanvas);

                // DamageText 위치 설정 (적 머리 위)
                damageTextInstance.transform.position = transform.position + new Vector3(0, 1f, 0);

                // DamageIndicator 스크립트에 데미지 값 전달
                DamageIndicator damageIndicator = damageTextInstance.GetComponent<DamageIndicator>();
                if (damageIndicator != null)
                {
                    damageIndicator.SetDamage(player.Attack); // 또는 player.GetAttack() 사용
                }
                /*else
                {
                    Debug.LogError("DamageIndicator script is missing on the prefab!");
                }*/
            }
            /*else
            {
                Debug.LogError("DamageTextPrefab, FightWorldCanvas, or Player is not assigned!");
            }*/
        }
    }
}