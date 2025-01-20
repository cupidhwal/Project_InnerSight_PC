using UnityEngine;

namespace JungBin
{

    public class ResurrectionRelic : MonoBehaviour, IRelic
    {
        public string RelicName => "생명의 고리";       //유물의 이름
         public string Description => "죽음을 맞이하는 순간, 단 한 번 생명력을 되찾아 다시 일어섭니다.";       //유물 설명
        private bool isCollected = false; // 중복 수집 방지 플래그

        public void ApplyEffect(Player player)
        {
            player.AddLife(1);  // 플레이어 Life 1 증가
        }

        public void RemoveEffect(Player player)
        {
            player.RemoveLife(1);
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