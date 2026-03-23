using System.Collections;
using UnityEngine;

namespace Project.Boss
{
    public enum HybridPhase { Ranged, Melee }

    public class HybridBoss : Boss
    {
        [Header("하이브리드 설정")]
        [SerializeField] private float meleeRange = 2f;
        [SerializeField] private float meleeCooldown = 1f;
        [SerializeField] private int meleeDamage = 30;

        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float bulletSpeed = 5f;
        [SerializeField] private float rangedCooldown = 1.5f;
        [SerializeField] private int rangedBulletCount = 5;

        [SerializeField] private float phaseSwitchInterval = 5f; // 몇 초마다 페이즈 전환

        private float attackTimer;
        private float phaseTimer;
        private HybridPhase currentPhase = HybridPhase.Ranged;

        protected override void Start()
        {
            base.Start();
            phaseTimer = phaseSwitchInterval;
        }

        protected override void BattleUpdate()
        {
            FacePlayer();

            // 페이즈 전환 타이머
            phaseTimer -= Time.deltaTime;
            if (phaseTimer <= 0f)
            {
                phaseTimer = phaseSwitchInterval;
                SwitchPhase();
            }

            attackTimer -= Time.deltaTime;
            float dist = Vector2.Distance(transform.position, playerTF.position);

            if (currentPhase == HybridPhase.Ranged)
                HandleRanged(dist);
            else
                HandleMelee(dist);
        }

        void SwitchPhase()
        {
            currentPhase = currentPhase == HybridPhase.Ranged
                ? HybridPhase.Melee
                : HybridPhase.Ranged;
            attackTimer = 0f;
            Debug.Log($"페이즈 전환: {currentPhase}");
        }

        // 원거리 페이즈: 적정 거리 유지 + 부채꼴 탄막
        void HandleRanged(float dist)
        {
            float keepDist = 6f;
            Vector2 dir = (playerTF.position - transform.position).normalized;

            if (dist < keepDist)
                rb.velocity = -dir * moveSpeed; // 뒤로 빠지기
            else if (dist > keepDist + 1f)
                rb.velocity = dir * moveSpeed;
            else
                rb.velocity = Vector2.zero;

            if (attackTimer <= 0f)
            {
                attackTimer = rangedCooldown;
                ShootSpread();
            }
        }

        // 근거리 페이즈: 돌진 후 근접 공격
        void HandleMelee(float dist)
        {
            Vector2 dir = (playerTF.position - transform.position).normalized;
            rb.velocity = dir * (moveSpeed * 1.5f); // 돌진

            if (dist <= meleeRange && attackTimer <= 0f)
            {
                attackTimer = meleeCooldown;
                rb.velocity = Vector2.zero;
                // 플레이어에게 데미지 (플레이어 구현 후 연결)
                // playerTF.GetComponent<PlayerHealth>()?.TakeDamage(meleeDamage);
                Debug.Log($"근접 공격! 데미지: {meleeDamage}");
            }
        }

        void ShootSpread()
        {
            if (bulletPrefab == null) return;
            float baseAngle = Vector2.SignedAngle(Vector2.right,
                (playerTF.position - transform.position).normalized);
            float spreadAngle = 30f;
            float step = rangedBulletCount > 1 ? spreadAngle * 2 / (rangedBulletCount - 1) : 0f;

            for (int i = 0; i < rangedBulletCount; i++)
            {
                float angle = baseAngle - spreadAngle + step * i;
                Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),
                                           Mathf.Sin(angle * Mathf.Deg2Rad));
                GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                b.GetComponent<Rigidbody2D>()?.AddForce(dir * bulletSpeed, ForceMode2D.Impulse);
            }
        }
    }
}