using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Monster
{
    enum MonsterState
    {
        Idle,
        Move,
        Attack,
    }
    public class MonsterLogic
    {

    }
    public class Monster : MonoBehaviour
    {
        [SerializeField] Transform playerTF;
        [SerializeField] private LayerMask playerMask;
        private Rigidbody2D rb;

        [SerializeField] private float detectRange = 10f;  // 플레이어 감지 범위
        [SerializeField] private float attackRange = 5f;   // 공격 범위
        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private int maxHp = 100; //임시
        private int currentHp; 

        [SerializeField] private LayerMask wallMask;
        [SerializeField] private float wallCheckDiis = 1f;
        [SerializeField] private float changeDir = 2f;


        MonsterState currentState;


        private bool isDead = false;
        public bool IsDead => isDead; //람다

        public event Action<Monster> OnDead;
        void Start()
        {
            currentHp = maxHp; 

            rb = GetComponent<Rigidbody2D>();
            currentState = MonsterState.Idle;

            if(playerTF == null) // 레이어?
            {

            }
        }

        public void Activate()
        {
            gameObject.SetActive(true); 
            currentState = MonsterState.Move; // 상태 변경
            Debug.Log($"{gameObject.name} 활성화!"); // 확인하고 지우기

        }

        void Update()
        {
        }
    }
}