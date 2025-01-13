using UnityEngine;
using System.Collections;

namespace JungBin
{

    public class JumpAttackController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Transform raycastOffsetObj;
        [SerializeField] private float raycastOffset = 0.15f;
        [SerializeField] private float jumpSpeed = 10f;
        [SerializeField] private float maxHeight = 10f;
        public LayerMask groundLayer; // 땅 레이어
        [SerializeField] private GameObject jumpAttackBox;
        private Animator animator;
        private bool isGrounded;
        private bool isMax;
        private Player player;


        #endregion

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            animator = this.GetComponent<Animator>();
            isMax = false;
            player = GameManager.Instance.Player;
            jumpAttackBox.SetActive(false);
        }

        private void Update()
        {
            

            if(animator.GetBool("IsJump") == true)
            {
                if (transform.position.y < maxHeight && !isMax)
                {
                    transform.position += Vector3.up * jumpSpeed * Time.deltaTime;
                    if(transform.position.y >= maxHeight)
                    {
                        isMax = true;
                    }
                }
                else if (isMax)
                {
                    if (animator.GetBool("IsAttack01") == true)
                    {
                        JumpToPlayer();
                    }
                    else if (animator.GetBool("IsAttack01") == false)
                    {
                        transform.position += Vector3.down * jumpSpeed * 3f * Time.deltaTime;
                        jumpAttackBox.SetActive (true);
                        if (IsGrounded() == true)
                        {
                            // 착지 애니메이션 재생
                            animator.SetBool("IsJump", false);
                            isMax = false;
                        }
                    }
                }
            }
        }

        public void JumpToPlayer()  //공중에서 플레이어한테 이동
        {
            Vector3 direction = new Vector3(player.transform.position.x - transform.position.x, 0f, player.transform.position.z - transform.position.z);

            Vector3 dir = direction.normalized;
            float player_Distance = direction.magnitude;

            transform.position += dir * jumpSpeed * Time.deltaTime;

            if (player_Distance < 0.2f)
            {
                animator.SetBool("IsAttack01", false);
            }
        }

        public void JumpAttackBoxOff()
        {
            jumpAttackBox.SetActive(false);
        }

        public void StartJumpAttack()
        {
            // 점프 시작 애니메이션 재생
            animator.SetBool("IsJump", true);   
        }

        private bool IsGrounded()
        {
            // 바닥 감지 (Raycast로 땅과 충돌 여부 확인)
            return Physics.Raycast(raycastOffsetObj.position, Vector3.down, raycastOffset, groundLayer);   
        }
    }
}