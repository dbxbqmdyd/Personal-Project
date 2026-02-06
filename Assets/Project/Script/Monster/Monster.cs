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
        void Start()
        {
            currentState = MonsterState.Idle;
        }



        void Update()
        {
        }
    }
}