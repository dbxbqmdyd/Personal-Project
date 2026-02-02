using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Project.Script.Player
{
    public enum PlayerState
    {
        Idle,
        Move,
        Attack,
        Dash,
        Hit

    }
    public class Player : MonoBehaviour
    {
        Rigidbody2D rb;
        PlayerState currentState;
        Vector2 WASD()
        {
            float h = 0f;
            float v = 0f;

            if (Input.GetKey(KeyCode.A)) h -= 1f;
            if (Input.GetKey(KeyCode.D)) h += 1f;
            if (Input.GetKey(KeyCode.W)) v += 1f;
            if (Input.GetKey(KeyCode.S)) v -= 1f;

            return new Vector2(h, v).normalized;
        }
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            currentState = PlayerState.Idle;
        }
        public void ChangeState(PlayerState state)
        {
            if (currentState == state) return;
            currentState = state;
        }
        public void Idle()
        {
            Vector2 input = WASD();
            if (input.magnitude > 0.1f)
            {
                ChangeState(PlayerState.Move);
            }

        }
        public void Move()
        {
            Vector2 input = WASD();
            if (input.magnitude > 0.1f)
            {
                transform.Translate(input * 5f * Time.deltaTime);
            }
            else
            {
                ChangeState(PlayerState.Idle);
            }
        }
        void Update() // 입력만 여기로
        {
            switch (currentState)
            {
                case PlayerState.Idle:
                    Idle();
                    break;
                case PlayerState.Move:
                    Move();
                    break;
            }

        }
        private void FixedUpdate() // 리지드보디로 변경 트랜스폼 말고
        {
            switch (currentState)
            {
                case PlayerState.Idle:
                    //Idle();
                    break;
                case PlayerState.Move:
                    //Move();
                    break;
            }
        }
    }
}