using UnityEngine;

namespace Project.Player
{
    public class SwordHitbox : MonoBehaviour
    {
        private int damage;
        private bool isActive = false;

        public void SetDamage(int dmg) => damage = dmg;
        public void EnableHitbox() => isActive = true;
        public void DisableHitbox() => isActive = false;

        void OnTriggerEnter2D(Collider2D col)
        {
            if (!isActive) return;

            if (col.TryGetComponent<Boss.Boss>(out var boss))
            {
                boss.TakeDamage(damage);
                Debug.Log($"°Ë È÷Æ®: {col.name}");
            }
        }
    }
}