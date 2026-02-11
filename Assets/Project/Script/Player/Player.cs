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
        [SerializeField] PlayerState currentState;
        [SerializeField] PlayerType pT;
        [SerializeField] private float attackCd = 0.3f;
        [SerializeField] private float attackCdT = 0f;





        private float dashSpeed = 11f;
        private float dashT = 0.25f;

        private bool isDashing;
        private bool isDashCd; // 대시쿨용
        private bool isInvincible; // 무적 상태 여부

        public Weapon weapon;

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
            rb.velocity = input * 5f; // 변수 바꾸기 스피드
        } 
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
        /*
        public void Dash()
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = (mousePos - (Vector2)transform.position).normalized;

            Debug.DrawLine(transform.position, mousePos, Color.blue, 1f);  // 씬에서 보임
            Debug.Log($"대쉬 방향: {dir}");

            StartCoroutine(DashCo(dir));
        }
        */
        /*
        public void Dash()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Mathf.Abs(Camera.main.transform.position.z);  // 이 부분!

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector2 dir = ((Vector2)worldPos - (Vector2)transform.position).normalized;

            Debug.DrawLine(transform.position, worldPos, Color.blue, 1f);
            Debug.Log($"마우스 월드: {worldPos}, 플레이어: {transform.position}, 방향: {dir}");

            StartCoroutine(DashCo(dir));
        }
        IEnumerator DashCo(Vector2 dir)
        {
            isDashing = true;
            float timer = 0f;

            while (timer < dashT)
            {
                rb.velocity = dir * dashSpeed;
                timer += Time.deltaTime;
                yield return null;
            }
            rb.velocity = Vector2.zero;
            isDashing = false;
            currentState = PlayerState.Idle;
        }
        */

        public void DashInput()
        {
            if (!Input.GetMouseButtonDown(1)) return;
            if (isDashing || isDashCd) return;
            if (WASD().sqrMagnitude < 0.01f)
            {
                ChangeState(PlayerState.Idle);
                return;
            }

            ChangeState(PlayerState.Dash);
            StartCoroutine(DashCo(WASD()));
        }
        /*
        public void Dash()
        {
            Vector2 inputDir = WASD();

            if (inputDir.sqrMagnitude < 0.01f)
            {
                ChangeState(PlayerState.Idle);
                return;
            }

            StartCoroutine(DashCo(inputDir));
        }
        */
        IEnumerator DashCo(Vector2 dir)
        {
            isDashing = true;
            isDashCd = true;
            isInvincible = true;

            float timer = 0f;

            while (timer < dashT)
            {
                rb.velocity = dir * dashSpeed;
                timer += Time.deltaTime;
                yield return null;
            }

            rb.velocity = Vector2.zero;
            isDashing = false;
            isInvincible = false;

            ChangeState(PlayerState.Idle);
            yield return new WaitForSeconds(0.15f); // 대시 쿨타임
            isDashCd = false;

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
            if (isInvincible)
            {
                Debug.Log("무적 상태");
            }

            bool isSword = Input.GetMouseButtonDown(0);
            bool isGun = Input.GetMouseButton(0);

            weapon.WeaponInput(isSword, isGun);
            //MouseTracking();
            /*
            if (Input.GetMouseButtonDown(1) && !isDashing && !isDashCd)
            {
                if (WASD().sqrMagnitude > 0f)
                {
                    ChangeState(PlayerState.Dash);
                    Dash();
                }

            }
            */

            DashInput();
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
                case PlayerState.Dash:
                    break;
            }

        }
        private void FixedUpdate() // 리지드보디로 변경 트랜스폼 말고
        {
            if (isDashing) return;

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