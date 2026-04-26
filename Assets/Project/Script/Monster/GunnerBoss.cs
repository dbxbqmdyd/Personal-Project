
/*
using System.Collections;
using UnityEngine;

namespace Project.Boss
{
    public class GunnerBoss : Boss
    {
        [Header("거너 설정")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float bulletSpeed = 6f;
        [SerializeField] private float shootCooldown = 2.5f;
        [SerializeField] private int bulletCount = 8;     // 한 번에 발사할 탄 수
        [SerializeField] private bool isSpiral = false;   // true면 나선형 탄막

        private float shootTimer;
        private float spiralAngle = 0f;

        protected override void Start()
        {
            base.Start();
            shootTimer = shootCooldown;
        }

        protected override void OnBattleStart()
        {
            Debug.Log("거너 보스 전투 시작!");
        }

        protected override void BattleUpdate()
        {
            FacePlayer();
            shootTimer -= Time.deltaTime;

            if (shootTimer <= 0f)
            {
                shootTimer = shootCooldown;
                StartCoroutine(isSpiral ? ShootSpiral() : ShootAllDirection());
            }
        }

        // 총탄 킹 스타일 - 사방으로 균등하게 발사
        IEnumerator ShootAllDirection()
        {
            float angleStep = 360f / bulletCount;
            // 플레이어 방향으로 오프셋을 줘서 조준감 추가
            float baseAngle = Vector2.SignedAngle(Vector2.right,
                (playerTF.position - transform.position).normalized);

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = baseAngle + (angleStep * i);
                SpawnBullet(angle);
            }
            yield return null;
        }

        // 나선형 탄막 (페이즈2 등 응용 가능)
        IEnumerator ShootSpiral()
        {
            int bursts = 12;
            for (int i = 0; i < bursts; i++)
            {
                SpawnBullet(spiralAngle);
                SpawnBullet(spiralAngle + 180f);
                spiralAngle += 30f;
                yield return new WaitForSeconds(0.1f);
            }
        }

        void SpawnBullet(float angle)
        {
            if (bulletPrefab == null) return;
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),
                                       Mathf.Sin(angle * Mathf.Deg2Rad));
            GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            b.GetComponent<Rigidbody2D>()?.AddForce(dir * bulletSpeed, ForceMode2D.Impulse);
        }
    }
}
*/
using System.Collections;
using UnityEngine;

namespace Project.Boss
{
    public class GunnerBoss : Boss
    {
        [Header("건너 설정")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float bulletSpeed = 6f;
        [SerializeField] private float shootCooldown = 2.5f;
        [SerializeField] private int bulletCount = 8;

        [Header("움직임 설정")]
        [SerializeField] private float orbitRadius = 6f;    // 플레이어 주변 선회 반경
        [SerializeField] private float orbitSpeed = 0.4f;   // 선회 속도 (느릴수록 총탄킹 느낌)
        [SerializeField] private float approachSpeed = 2f;  // 거리 조정 속도

        private float shootTimer;

        protected override void Start()
        {
            base.Start();
            shootTimer = shootCooldown;

            // 시작 각도는 현재 위치 기준으로 설정
            Vector2 offset = (Vector2)transform.position - (Vector2)playerTF.position;
        }

        protected override void BattleUpdate()
        {
            FacePlayer();
            OrbitMove();

            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                shootTimer = shootCooldown;
                StartCoroutine(ShootAllDirection());
            }
        }

        void OrbitMove()
        {
            float dist = Vector2.Distance(transform.position, playerTF.position);

            // 일정 거리보다 멀어지면 플레이어쪽으로 다가감
            if (dist > orbitRadius)
            {
                Vector2 dir = (playerTF.position - transform.position).normalized;
                rb.velocity = dir * approachSpeed;
            }
            else
            {
                // 사거리 안에 있으면 그냥 멈춤
                rb.velocity = Vector2.zero;
            }
        }

        IEnumerator ShootAllDirection()
        {
            // 탄막 쏘는 동안 잠깐 멈추는 느낌 (총탄킹 느낌)
            float prevSpeed = approachSpeed;
            approachSpeed = 0f;

            float angleStep = 360f / bulletCount;
            float baseAngle = Vector2.SignedAngle(Vector2.right,
                (playerTF.position - transform.position).normalized);

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = baseAngle + (angleStep * i);
                SpawnBullet(angle);
            }

            yield return new WaitForSeconds(0.3f); // 발사 후 짧은 경직
            approachSpeed = prevSpeed;
        }

        void SpawnBullet(float angle)
        {
            if (bulletPrefab == null) return;
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),
                                       Mathf.Sin(angle * Mathf.Deg2Rad));
            GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            b.GetComponent<Rigidbody2D>()?.AddForce(dir * bulletSpeed, ForceMode2D.Impulse);
        }
    }
}