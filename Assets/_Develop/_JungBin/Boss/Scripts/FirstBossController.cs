using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace JungBin
{
    /// <summary>
    /// ������ �ִϸ��̼��� ������ ��� ����
    /// </summary>
    public class FirstBossController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Transform player_Transform;    //�÷��̾��� ��ġ
        [SerializeField] private GameObject attackBox;      //������ ���� �ڽ�
        private Animator m_Animator;


        [SerializeField] private float turnSpeed = 1200;    //������ ȸ�� �ӵ�

        public static bool isAttack { get; set; } = false; // ���������� ����
        public static bool isDashing { get; set; } = false; // ���������� ����
        private int lastAttack = -1; //���� ���� ����
        #endregion

        private void Start()
        {
            m_Animator = this.GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            //�÷��̾���� �Ÿ�
            Vector3 direction = player_Transform.position - transform.position;
            direction.y = 0;
            float distance = direction.magnitude;

            if (!isAttack)
            {
                TurnBossToPlayer(direction);
            }

            CloseDistanceToPlayer(distance);

            if (m_Animator.GetBool("Idle") == true && m_Animator.GetBool("IsFar") == false)
            {
                NextAttack();
            }
            AttackBoxActive();

        }

        //������ ȸ�� ���� �ڵ�
        public void TurnBossToPlayer(Vector3 direction)
        {
            // �÷��̾��� ���� ���
            Vector3 dir = direction.normalized;

            // ���� ����� ��ǥ ������ ȸ�� ������ ��ȯ
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // ���� ȸ���� ��ǥ ȸ������ ����
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }

        //���� �Ÿ� �̻� �־������� ������ ������ ������ �ڵ�
        private void CloseDistanceToPlayer(float distance)
        {
            if (distance > 15f)
            {
                m_Animator.SetBool("IsFar", true);
            }
            else if (distance < 5f)
            {
                m_Animator.SetBool("IsFar", false);
            }
        }

        //���� ���� ���ϴ� �ڵ�
        public void NextAttack()
        {
            if (m_Animator == null)
                return;

            int attackIndex;

            //ù��° �������� Ȯ��
            if (lastAttack == -1)
            {
                //ù �����ϰ�� 3���� ������ �ϳ� ����
                attackIndex = Random.Range(1, 4);
            }
            else
            {
                do
                {
                    attackIndex = Random.Range(1, 4);
                } while (lastAttack == attackIndex);
            }

            //���õ� ���� �ִϸ��̼� ����
            TriggerAttackAnimation(attackIndex);

            //�ߺ� ���� ����
            lastAttack = attackIndex;

            m_Animator.SetBool("Idle", false);
        }

        // ���� �ִϸ��̼� ȣ��
        private void TriggerAttackAnimation(int attackIndex)
        {
            string triggerName = $"Attack0{attackIndex}";
            m_Animator.SetTrigger(triggerName);
        }

        private void AttackBoxActive()
        {
            if (m_Animator.GetBool("IsAttack02") == true)
            {
                attackBox.SetActive(true);
            }
            else
            {
                attackBox.SetActive(false);
            }
        }

    }
}