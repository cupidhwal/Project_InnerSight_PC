using UnityEngine;

namespace JungBin
{

    public class ResurrectionRelic : MonoBehaviour, IRelic
    {
        public string RelicName => "Phoenix Feather";       //유물의 이름
        public string Description => "Allows you to revive once upon death.";       //유물 설명
        private bool isCollected = false; // 중복 수집 방지 플래그

        public void ApplyEffect(Player player)
        {
            player.AddLife(1);  // 플레이어 Life 1 증가
        }

        // 플레이어 감지시 습득
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Player>())
            {
                
                if (isCollected)    // 중복 수집 방지
                    return;

                IRelic relic = gameObject.GetComponent<IRelic>();
                if (relic != null)
                {
                    
                    //플레이어에게 유물 등록
                    GameManager.Instance.RegisterRelic(relic);
                    isCollected = true;

                    Destroy(gameObject);
                }

            }
        }
    }
}