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

        private float detectRange = 10f;  // 플레이어 감지 범위
        private float attackRange = 5f;   // 공격 범위
        private float moveSpeed = 3f;
        MonsterState currentState;


        private bool isDead = false;
        public bool IsDead => isDead; //람다
        void Start()
        {
            currentState = MonsterState.Idle;
        }

        public void Activate()
        {
            gameObject.SetActive(true); 
            currentState = MonsterState.Move; // 상태 변경
            Debug.Log($"{gameObject.name} 활성화!");

        }

        void Update()
        {
        }
    }
}