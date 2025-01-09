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
        [SerializeField] private GameObject rushAttackBox;      //������ ���� �ڽ�
        [SerializeField] private BoxCollider throwAttackBox;      //������ ���� �ڽ�
        private Animator m_Animator;


        [SerializeField] private float turnSpeed = 1200;    //������ ȸ�� �ӵ�

        public static bool isAttack { get; set; } = false; // ���������� ����
        public static bool isDashing { get; set; } = false; // ���������� ����
        private int lastAttack = -1; //���� ���� ����
        private float jumpMoveSpeed = 10f;
        #endregion

        private void Start()
        {
            m_Animator = this.GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            //�÷��̾���� �Ÿ�
            Vector3 direction = GameManager.Instance.PlayerTransform.position - transform.position;
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

            Vector3 dir = PlayerPosition();

            if(m_Animator.GetBool("IsAttack01"))
            {
                JumpToPlayer(dir);

            }

            AttackBoxActive();

        }

        //������ ȸ�� ���� �ڵ�
        public void TurnBossToPlayer(Vector3 direction)
        {
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);

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
            else if (distance < 3f)
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

        public Vector3 PlayerPosition()
        {
            Vector3 player_Position = GameManager.Instance.PlayerTransform.position - transform.position;

            return player_Position;
            
        }

        public void JumpToPlayer(Vector3 distance)
        {
            Vector3 dir = distance.normalized;
            float player_Distance = distance.magnitude;

            transform.position += dir * jumpMoveSpeed * Time.deltaTime;

            if (player_Distance < 1f)
            {
                m_Animator.SetBool("IsAttack01", false);
            }
        }

        private void AttackBoxActive()
        {
            if (m_Animator.GetBool("IsAttack02") == true)
            {
                rushAttackBox.SetActive(true);
            }
            else
            {
                rushAttackBox.SetActive(false);
            }

            if (m_Animator.GetBool("IsAttack01") == true)
            {
                throwAttackBox.enabled = true;
            }
            else
            {
                throwAttackBox.enabled = false;
            }
        }

    }
}