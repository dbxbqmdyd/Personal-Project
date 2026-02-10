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
            doorCol.enabled = false; // ¿­¸° ¹®
            spriteRenderer.color = Color.green;
        }
        public void LockDoor()
        {
            isLock = true;
            doorCol.enabled = true; // ´ÝÈù ¹®
            spriteRenderer.color = Color.red;
        }
        void Start()
        {
            doorCol = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {

        }
    }
}