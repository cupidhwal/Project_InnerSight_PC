using UnityEngine;

namespace JungBin
{

    public interface IRelic
    {
        string RelicName { get; }    // ���� �̸�
        string Description { get; } // ���� ����

        void ApplyEffect(Player player); // ���� ȿ�� ����
    }
}