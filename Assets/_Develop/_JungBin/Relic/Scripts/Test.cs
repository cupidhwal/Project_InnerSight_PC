using UnityEngine;

namespace JungBin
{

    public class Test : MonoBehaviour, IRelic
    {
        public string RelicName => "죽음의 고리";       //유물의 이름
        public string RelicID => "deathRing";  // UI 버튼과 매칭될 영어 ID
        public string Description => "죽습니다";       //유물 설명

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
                for(int i = 0; i < RelicManager.Instance.GetRelics().Count; i++)
                {
                    string Temp = RelicManager.Instance.GetRelics()[i].RelicID;
                    if (Temp == RelicID)
                        return;
                }

                IRelic relic = gameObject.GetComponent<IRelic>();
                if (relic != null)
                {

                    //플레이어에게 유물 등록
                    GameManager.Instance.RegisterRelic(relic);

                    Destroy(gameObject);
                }

            }
        }
    }
}