using UnityEngine;

namespace JungBin
{

    public class ResurrectionRelic : MonoBehaviour, IRelic
    {
        public string RelicName => "Phoenix Feather";       //������ �̸�
        public string Description => "Allows you to revive once upon death.";       //���� ����
        private bool isCollected = false; // �ߺ� ���� ���� �÷���

        public void ApplyEffect(Player player)
        {
            player.AddLife(1);  // �÷��̾� Life 1 ����
        }

        // �÷��̾� ������ ����
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Player>())
            {
                
                if (isCollected)    // �ߺ� ���� ����
                    return;

                IRelic relic = gameObject.GetComponent<IRelic>();
                if (relic != null)
                {
                    
                    //�÷��̾�� ���� ���
                    GameManager.Instance.RegisterRelic(relic);
                    isCollected = true;

                    Destroy(gameObject);
                }

            }
        }
    }
}