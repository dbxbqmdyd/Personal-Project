using System;
using UnityEngine;

namespace Project.Boss
{
    public enum BossState { Idle, Battle, Dead }

    public abstract class Boss : MonoBehaviour
    {
        [Header("기본 스탯")]
        [SerializeField] protected int maxHp = 500;
        [SerializeField] protected float moveSpeed = 3f;
        [SerializeField] protected float detectRange = 15f;

        protected int currentHp;
        protected Transform playerTF;
        protected Rigidbody2D rb;
        protected BossState currentState;
        protected bool isDead = false;

        [SerializeField] protected LayerMask playerMask;

        public event Action<Boss> OnBossDead;
        public event Action<int, int> OnHpChanged; // 현재HP, 최대HP (UI용)

        protected virtual void Start()
        {
            currentHp = maxHp;
            rb = GetComponent<Rigidbody2D>();
            currentState = BossState.Idle;
        }

        protected virtual void Update()
        {
            if (isDead) return;

            if (currentState == BossState.Idle)
                DetectPlayer();
            else
                BattleUpdate();
        }

        void DetectPlayer()
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, detectRange, playerMask);
            if (hit != null)
            {
                playerTF = hit.transform;
                currentState = BossState.Battle;
                OnBattleStart();
            }
        }

        protected virtual void OnBattleStart() { } // 자식에서 오버라이드

        protected abstract void BattleUpdate(); // 자식에서 반드시 구현

        public void TakeDamage(int damage)
        {
            if (isDead) return;
            currentHp -= damage;
            currentHp = Mathf.Max(currentHp, 0);
            OnHpChanged?.Invoke(currentHp, maxHp);

            if (currentHp <= 0) Die();
        }

        protected virtual void Die()
        {

            isDead = true;
            rb.velocity = Vector2.zero;
            OnBossDead?.Invoke(this);
            OnDeathEffect();
        }

        protected virtual void OnDeathEffect()
        {
            gameObject.SetActive(false); // 자식에서 오버라이드해서 분열 등 처리
        }

        protected void FacePlayer()
        {
            if (playerTF == null) return;
            Vector3 scale = transform.localScale;
            scale.x = playerTF.position.x < transform.position.x ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }
}