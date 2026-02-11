using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Project.Room
{
    public class Door : MonoBehaviour
    {
        private Collider2D doorCol;
        private SpriteRenderer spriteRenderer;

        [SerializeField] private bool isLock = false;

        
        public void UnlockDoor()
        {
            isLock = false;
            doorCol.enabled = false; // 열린 문
            spriteRenderer.color = Color.green;
        }
        public void LockDoor()
        {
            isLock = true;
            doorCol.enabled = true; // 닫힌 문
            spriteRenderer.color = Color.red;
        }
        void Start()
        {
            doorCol = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            doorCol.enabled = false;
        }

        void Update()
        {
            /*
            if (isLock)
            {
                LockDoor();
                // 문 잠김 상태 처리
            }
            else {                 UnlockDoor();
                // 문 열림 상태 처리
            }
            */
        }
    }
}