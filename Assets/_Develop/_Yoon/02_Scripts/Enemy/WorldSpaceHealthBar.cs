using UnityEngine;
using UnityEngine.UI;
using Seti;
using static Seti.Damagable;

namespace Yoon
{
    public class WorldSpaceHealthBar : MonoBehaviour
    {
        #region Variables
        private Damagable damagable;                // Damagable 참조          
        public Player player;                       // Player 객체 참조

        public Transform healthBarPivot;          // 헬스바가 카메라를 바라보도록 설정
        public Slider EnemyHealthBarSlider;       // 헬스바 슬라이더
        public GameObject damageTextPrefab;       // DamageText 프리팹
        public Transform fightWorldCanvas;        // DamageText 부모 오브젝트

        [SerializeField]
        private bool hideFullHealthBar = true; // 체력이 가득 찼을 때 숨길지 여부
        #endregion

        private void Start()
        {
            damagable = GetComponent<Damagable>();
            player = player.GetComponent<Player>();
        }

        private void Update()
        {
            // 헬스바가 카메라를 바라보도록 설정
            if (healthBarPivot != null)
            {
                healthBarPivot.LookAt(Camera.main.transform.position);
            }

            // 체력이 가득 찼을 때 헬스바 숨김
            if (hideFullHealthBar && damagable != null)
            {
                EnemyHealthBarSlider.gameObject.SetActive(damagable.CurrentHitPoints < damagable.MaxHitPoint);
            }

            // 체력바 업데이트
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            if (damagable != null && EnemyHealthBarSlider != null)
            {
                EnemyHealthBarSlider.value = damagable.CurrentHitPoints / damagable.MaxHitPoint;
            }
        }

        // DamageText를 표시하는 함수
        public void ShowDamageIndicator()
        {
            if (player != null && damagable != null)
            {
                // Player의 Attack 값을 참조
                float damage = player.Attack;

                // DamageMessage 객체를 생성 (생성자 없이, 속성 설정 방식으로 변경)
                DamageMessage damageMessage = new DamageMessage();

                // damage 값을 설정
                //damageMessage.damage = damage;  // damage 프로퍼티가 있다면 이렇게 설정

                // damageSource 값을 설정
                damageMessage.damageSource = player.transform.position;  // damageSource는 player의 위치로 설정

                // 적에게 데미지를 가함
                damagable.TakeDamage(damageMessage);

                // DamageText 표시
                if (damageTextPrefab != null && fightWorldCanvas != null)
                {
                    // DamageText 프리팹 생성
                    GameObject damageTextInstance = Instantiate(damageTextPrefab, fightWorldCanvas);

                    // DamageText 위치를 적 머리 위로 설정
                    damageTextInstance.transform.position = healthBarPivot.position + new Vector3(0, 1, 0); // 적 머리 위에 배치

                    // DamageText에 데미지 값을 표시
                    DamageIndicator damageIndicator = damageTextInstance.GetComponent<DamageIndicator>();
                    if (damageIndicator != null)
                    {
                        damageIndicator.SetDamage(damage); // DamageText에 데미지 표시
                    }
                }

                // 디버깅 로그 출력
                Debug.Log($"Player dealt {damage} damage to Enemy. Remaining Health: {damagable.CurrentHitPoints}");
            }
        }
    }
}