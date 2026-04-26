/*
using System.Collections;
using UnityEngine;

namespace Project.Boss
{
    public class SlimeBoss : Boss
    {
        [Header("슬라임 설정")]
        [SerializeField] private float jumpCooldown = 2f;
        [SerializeField] private float jumpForce = 8f;
        [SerializeField] private GameObject smallSlimePrefab; // 분열용 작은 슬라임
        [SerializeField] private int splitCount = 3;          // 분열 마릿수

        private float jumpTimer;
        private Animator anim;
        private bool isJumping = false;

        protected override void Start()
        {
            base.Start();
            anim = GetComponent<Animator>();
            jumpTimer = jumpCooldown;
        }

        protected override void BattleUpdate()
        {
            if (isJumping) return;

            FacePlayer();
            jumpTimer -= Time.deltaTime;

            if (jumpTimer <= 0f)
            {
                jumpTimer = jumpCooldown;
                StartCoroutine(JumpAttack());
            }
        }

        IEnumerator JumpAttack()
        {
            isJumping = true;
            anim?.SetTrigger("Jump"); // 애니메이션 재생

            // 플레이어 방향으로 점프
            Vector2 dir = (playerTF.position - transform.position).normalized;
            rb.velocity = new Vector2(dir.x * moveSpeed * 2f, jumpForce);

            yield return new WaitForSeconds(0.8f); // 점프 지속 시간
            rb.velocity = Vector2.zero;

            anim?.SetTrigger("Land");
            isJumping = false;
        }

        protected override void OnDeathEffect()
        {
            // 분열: 주변에 smallSlime 소환
            if (smallSlimePrefab != null)
            {
                for (int i = 0; i < splitCount; i++)
                {
                    float angle = i * (360f / splitCount);
                    Vector2 offset = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),
                                                  Mathf.Sin(angle * Mathf.Deg2Rad));
                    Instantiate(smallSlimePrefab, (Vector2)transform.position + offset, Quaternion.identity);
                }
            }
            base.OnDeathEffect();
        }
    }
}
*/
using System.Collections;
using UnityEngine;

namespace Project.Boss
{
    public class SlimeBoss : Boss
    {
        [Header("슬라임 설정")]
        [SerializeField] private float jumpCooldown = 3f; // 쿨타임을 조금 늘려 이동 시간을 확보
        [SerializeField] private float jumpForce = 8f;
        [SerializeField] private GameObject smallSlimePrefab;
        [SerializeField] private int splitCount = 3;

        private float jumpTimer;
        private Animator anim;
        private bool isJumping = false;

        protected override void Start()
        {
            base.Start();
            anim = GetComponent<Animator>();
            jumpTimer = jumpCooldown;
        }

        protected override void BattleUpdate()
        {
            // 점프 중일 때는 이동 로직을 타지 않음
            if (isJumping) return;

            FacePlayer(); // 플레이어 바라보기

            // 1. 플레이어와의 거리 체크 (너무 멀면 다가감)
            float distance = Vector2.Distance(transform.position, playerTF.position);

            if (distance > 1.5f) // 일정 거리 이상일 때만 이동
            {
                // 플레이어 방향으로 천천히 이동
                Vector2 moveDir = (playerTF.position - transform.position).normalized;
                rb.velocity = new Vector2(moveDir.x * moveSpeed, rb.velocity.y);

                //anim?.SetFloat("Speed", rb.velocity.magnitude);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                //anim?.SetFloat("Speed", 0);
            }

            // 2. 점프 타이머 계산
            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0f)
            {
                jumpTimer = jumpCooldown;
                StartCoroutine(JumpAttack());
            }
        }

        IEnumerator JumpAttack()
        {
            isJumping = true;
            rb.velocity = Vector2.zero; // 점프 직전 멈춤

            anim?.SetTrigger("Jump");
            yield return new WaitForSeconds(0.2f); // 점프 전 예비 동작 시간 (애니메이션에 맞춰 조절)

            // 플레이어 방향으로 강하게 도약
            Vector2 dir = (playerTF.position - transform.position).normalized;
            rb.velocity = new Vector2(dir.x * moveSpeed * 3f, jumpForce);

            // 공중에 떠 있는 시간 (중력에 의해 떨어질 때까지 대기)
            yield return new WaitForSeconds(1.0f);

            anim?.SetTrigger("Land");
            rb.velocity = Vector2.zero;

            yield return new WaitForSeconds(0.5f); // 착지 후 경직 시간
            isJumping = false;
        }

        // OnDeathEffect는 동일...
    }
}