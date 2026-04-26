using System.Collections;
using UnityEngine;

namespace Project.Player
{
    public enum WeaponType { Click, Hold }

    public class Weapon : MonoBehaviour
    {
        [Header("근접 설정")]
        [SerializeField] private SwordHitbox swordHitbox;
        [SerializeField] private Transform swordObject;  // 칼 오브젝트
        [SerializeField] private float sRate = 0.45f;
        [SerializeField] private int swordDamage = 20;

        [Header("전환형 회전 설정")]
        [SerializeField] private int viewDirections = 8;     // 8방향 전환
        [SerializeField] private float rotateSpeed = 20f; // 높을수록 빠르게 따라옴
        [SerializeField] private float orbitDistance = 0.5f; // 캐릭터와의 간격
        [SerializeField] private SpriteRenderer weaponRenderer; // 정렬 순서 변경용

        [Header("추적 설정")]
        [SerializeField] private Transform playerTransform;

        [Header("칼 휘두르기 설정")]
        [SerializeField] private float swingStartAngle = 45f;   // 휘두르기 시작 각도
        [SerializeField] private float swingEndAngle = -90f;    // 휘두르기 끝 각도
        [SerializeField] private float swingDuration = 0.15f;   // 휘두르는 시간

        [Header("이펙트 설정")]
        [SerializeField] private GameObject slashEffectPrefab; // 검기 프리팹
        [SerializeField] private Transform slashSpawnPoint;    // 검기가 소환될 위치 (칼날 끝부분 추천)

        [Header("원거리 설정")]
        [SerializeField] private float gRate = 0.18f;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform gunMuzzle;
        [SerializeField] private float bulletSpeed = 10f;
        [SerializeField] private int gunDamage = 10;

        [SerializeField] private WeaponType wT;

        private float sTimer;
        private float gTimer;
        private bool isSwinging = false;
        private bool isFacingLeft = false;


        public void WeaponInput(bool sword, bool gun)
        {
            switch (wT)
            {
                case WeaponType.Click:
                    if (sword) TrySwordAttack();
                    break;
                case WeaponType.Hold:
                    if (gun) TryGunAttack();
                    break;
            }
        }

        void Update()
        {
            if (sTimer > 0) sTimer -= Time.deltaTime;
            if (gTimer > 0) gTimer -= Time.deltaTime;

            if (!isSwinging)
                RotateToMouse();
        }
        void RotateToMouse()
        {
            if (playerTransform == null) return;

            transform.position = playerTransform.position;

            Vector3 mousePos = Input.mousePosition;

            Plane plane = new Plane(Vector3.forward, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 mouseWorld = ray.GetPoint(distance);
                mouseWorld.z = 0;

                Vector2 dir = (mouseWorld - transform.position).normalized;
                float realAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                Quaternion targetRot = Quaternion.Euler(0, 0, realAngle);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * rotateSpeed);

                float currentAngle = transform.eulerAngles.z;
                float rad = currentAngle * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * orbitDistance;
                transform.position += offset;

                HandleVisuals(realAngle);
            }
        }

        void HandleVisuals(float angle)
        {
            if (weaponRenderer != null)
            {
                isFacingLeft = Mathf.Abs(angle) > 90f;
                weaponRenderer.flipY = isFacingLeft;
                weaponRenderer.sortingOrder = 10;
            }
        }
        // ── 근접 ──────────────────────────────
        void TrySwordAttack()
        {
            if (sTimer > 0) return;
            if (isSwinging) return;
            sTimer = sRate;

            swordHitbox.SetDamage(swordDamage);
            StartCoroutine(SwingAnim());
        }
        void CreateSlash()
        {
            if (slashEffectPrefab == null) return;

            Quaternion slashRotation = swordObject.rotation;
            GameObject slash = Instantiate(slashEffectPrefab, slashSpawnPoint.position, slashRotation);

            if (slash.TryGetComponent<SlashEffect>(out var effect))
            {
                effect.Init(swordDamage, 0.2f);
            }

            if (transform.lossyScale.x < 0)
            {
                Vector3 s = slash.transform.localScale;
                s.y *= -1;
                slash.transform.localScale = s;
            }
        }
        IEnumerator SwingAnim()
        {
            isSwinging = true;

            float startAngle = isFacingLeft ? -swingStartAngle : swingStartAngle;
            float endAngle = isFacingLeft ? -swingEndAngle : swingEndAngle;

            float prepDuration = 0.05f;
            float timer = 0f;
            float startRot = swordObject.localEulerAngles.z;

            while (timer < prepDuration)
            {
                float angle = Mathf.LerpAngle(startRot, startAngle, timer / prepDuration);
                swordObject.localRotation = Quaternion.Euler(0, 0, angle);
                timer += Time.deltaTime;
                yield return null;
            }

            swordHitbox.EnableHitbox();
            CreateSlash();

            timer = 0f;
            while (timer < swingDuration)
            {
                float t = timer / swingDuration;
                t = t * t;
                float angle = Mathf.LerpAngle(startAngle, endAngle, t);
                swordObject.localRotation = Quaternion.Euler(0, 0, angle);
                timer += Time.deltaTime;
                yield return null;
            }

            swordHitbox.DisableHitbox();
            swordObject.localRotation = Quaternion.Euler(0, 0, 0);

            isSwinging = false;
        }

        // ── 원거리 ─────────────────────────────
        void TryGunAttack()
        {
            if (gTimer > 0) return;
            gTimer = gRate;
            ShootBullet();
        }

        void ShootBullet()
        {
            if (bulletPrefab == null) return;

            //Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Vector3 spawnPos = gunMuzzle != null ? gunMuzzle.position : transform.position;


            Vector2 dir = transform.right;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            Vector3 spawnPos = gunMuzzle != null ? gunMuzzle.position : transform.position;


            GameObject b = Instantiate(bulletPrefab, spawnPos, Quaternion.Euler(0, 0, angle));
            PlayerBullet bullet = b.GetComponent<PlayerBullet>();
            if (bullet != null)
                bullet?.Init(dir,bulletSpeed, gunDamage);
        }
    }
}