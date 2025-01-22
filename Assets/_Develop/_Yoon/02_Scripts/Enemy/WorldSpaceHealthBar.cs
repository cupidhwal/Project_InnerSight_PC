using UnityEngine;
using UnityEngine.UI;
using Seti;
using static Seti.Damagable;

namespace Yoon
{
    public class WorldSpaceHealthBar : MonoBehaviour
    {
        #region Variables
        public Transform healthBarPivot;          // 헬스바가 카메라를 바라보도록 설정
        public Slider EnemyHealthBarSlider;       // 헬스바 슬라이더
        public GameObject damageTextPrefab;       // DamageText 프리팹
        public Transform fightWorldCanvas;        // DamageText 부모 오브젝트

        private Damagable damagable;                // Damagable 참조

        private Player player;

        [SerializeField] private bool hideFullHealthBar = true; // 체력이 가득 찼을 때 숨길지 여부
        #endregion

        private void Start()
        {
            damagable = GetComponent<Damagable>();
            player = FindObjectOfType<Player>();
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
        }
/*
        // DamageText를 표시하는 함수
        public void ShowDamageIndicator()
        {
            //float beforeHealth = CurrentHealth;
            if (damageTextPrefab != null && fightWorldCanvas != null && player != null && damagable != null)
            {

                float realdamage = player.attack - damagable.CurrentHitPoints;
                GameObject damageTextInstance = Instantiate(damageTextPrefab, fightWorldCanvas);
                damageTextInstance.transform.localPosition = new Vector3(0, 0.5f, 0);

                DamageIndicator damageIndicator = damageTextInstance.GetComponent<DamageIndicator>();
                if (damageIndicator != null)
                {
                    damageIndicator.SetDamage(damage);
                }
            }*/
            /*
                        if (damageTextPrefab != null && fightWorldCanvas != null)
                        {
                            // DamageText 프리팹 생성
                            GameObject damageTextInstance = Instantiate(damageTextPrefab, fightWorldCanvas);

                            // DamageText 위치를 적 머리 위로 설정
                            damageTextInstance.transform.localPosition = new Vector3(0, 0.5f, 0);

                            // DamageText에 데미지 값 전달
                            DamageIndicator damageIndicator = damageTextInstance.GetComponent<DamageIndicator>();
                            if (damageIndicator != null)
                            {
                                damageIndicator.SetDamage(damage);
                            }
                        }
            */
        }
    }
