using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage;
    private Rigidbody2D rb;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 direction, float bulletSpeed, int bulletDamage)
    {
        damage = bulletDamage;
        rb.AddForce(direction * bulletSpeed, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9) // Monster
        {
            // other.GetComponent<Enemy>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == 7) // Wall
        {
            Destroy(gameObject);
        }
    }
}
