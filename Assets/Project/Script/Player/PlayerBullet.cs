using Project.Boss;
using Project.Player;
using UnityEngine;

namespace Project.Player
{
    // ÃÑ¾Ë ÇÁ¸®ÆÕ¿¡ ºÎÂø
    // Rigidbody2D + Collider2D(IsTrigger) ÇÊ¿ä
    public class PlayerBullet : MonoBehaviour
    {
        private Rigidbody2D rb;
        private int damage;
        [SerializeField] private float lifeTime = 3f;

        public void Init(Vector2 dir, float speed, int dmg)
        {
            rb = GetComponent<Rigidbody2D>();
            damage = dmg;
            rb.AddForce(dir.normalized * speed, ForceMode2D.Impulse);
            Destroy(gameObject, lifeTime);
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent<Boss.Boss>(out var boss))
            {
                boss.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
