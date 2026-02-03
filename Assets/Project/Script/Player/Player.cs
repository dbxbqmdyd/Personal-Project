using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Project.Player
{
    public enum PlayerType
    {
        Samurai,
        Gunner
    }
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
        [Serialize] PlayerState currentState;
        [Serialize] PlayerType pT;
        [Serialize] private float attackCd = 0.3f;
        [Serialize] private float attackCdT = 0f;

        Weapon weapon;

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
            rb.velocity = input * 5f;
        } // 변수 바꾸기 스피드
        public void Stop()
        {
            rb.velocity = Vector2.zero;
        }
        public void Attack()
        {
            if (attackCdT > 0)
            {
                attackCdT -= Time.deltaTime;
            }
            //Move();
            Vector2 input = WASD();
            rb.velocity = input * 2f;

            ChangeState(PlayerState.Idle);
        }
        public void TestTK()// 조준선 느낌으로 사용이 가능할듯
        {
            Vector3 mousePos = Input.mousePosition; // 마우스 위치를 가져옴
            mousePos.z = 10f; // 카메라 거리 조절용

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos); // 화면좌표를 월드좌표로 변환
            transform.position = worldPos; //오브젝트위치를 마우스위치로
        }
        void Update() // 입력만 여기로
        {
            /*
            bool isSword = Input.GetMouseButtonDown(0);
            bool isGun = Input.GetMouseButton(0);

            weapon.WeaponInput(isSword, isGun);
            //MouseTracking();

            */




            if (Input.GetMouseButtonDown(1)) ChangeState(PlayerState.Dash);
            switch (currentState)
            {
                case PlayerState.Idle:
                    if (WASD().sqrMagnitude > 0f)
                        ChangeState(PlayerState.Move);
                    break;
                case PlayerState.Move:
                    if (WASD().sqrMagnitude == 0f)
                        ChangeState(PlayerState.Idle);
                    break;
                case PlayerState.Attack:
                    Attack();
                    break;
            }

        }
        private void FixedUpdate() // 리지드보디로 변경 트랜스폼 말고
        {
            switch (currentState)
            {
                case PlayerState.Move:
                    Move();
                    break;
                default:
                    Stop();
                    break;


            }
        }
    }
}