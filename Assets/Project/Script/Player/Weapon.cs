using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace Project.Player
{
    public enum WeaponType
    {
        Click,   // 검
        Hold     // 총
    }
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private float SRate = 0.45f; // 공격 간격
        [SerializeField] private float STimer = 0f; // 공격 타이머  나중에 SO처리할 생각도 해두기
        [SerializeField] private float GRate = 0.18f;
        [SerializeField] private float GTimer = 0f;
        float timer;

        [SerializeField] private WeaponType wT;

        public void WeaponInput(bool sword, bool gun)
        {
            switch (wT)
            {
                case WeaponType.Click:
                    if (sword)
                        Attack(); // 임시
                    break;
                case WeaponType.Hold:
                    if (gun)
                        Attack2(); // 임시
                    break;
            }
        }
        private void Attack()
        {
            Debug.Log($"클릭공격");
        }
        private void Attack2()
        {
            Debug.Log($"홀드공격");
        }
        void Start()
        {

        }

        void Update()
        {

        }
    }
}