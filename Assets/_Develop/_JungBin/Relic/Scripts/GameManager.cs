using System.Collections.Generic;
using UnityEngine;

namespace JungBin
{

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; } // �̱��� �ν��Ͻ�

        [SerializeField] private RelicManager relicManager;
        [SerializeField] private Player player;
        [SerializeField] private Transform player_Transform;
        [SerializeField] private CapsuleCollider player_HitCapsuleCollider;

        public Player Player => player; // �ܺο��� ���� ������ ������Ƽ
        public Transform PlayerTransform => player_Transform;
        public CapsuleCollider Player_HitCapsuleCollider => player_HitCapsuleCollider;

        private void Awake()
        {
            // �̱��� �ν��Ͻ� ����
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        // ������ ����ϰ� ȿ���� ����
        public void RegisterRelic(IRelic relic)
        {
            if (relic != null)
            {
                relicManager.AddRelic(relic, player);
                
                Debug.Log($"���� ��� ������ �̸� : {relic.RelicName}");
            }
        }
    }
}