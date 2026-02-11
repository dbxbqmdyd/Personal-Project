using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Project.Room
{

    public class Room : MonoBehaviour
    {
        public Door[] doors;
        public Monster.Monster[] monsters;

        bool isCleared = false;
        bool isActivated = false;

        [SerializeField] private LayerMask playerMask;

        [SerializeField] private Monster.MonsterSpawner spawner;

        private void Start()
        {
            monsters = spawner.SpawnMonsters();
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (IsPlayer(col) && CanActivate())
            {
                EnterRoom();
            }
        }
        private bool IsPlayer(Collider2D col)
        {
            return ((1 << col.gameObject.layer) & playerMask) != 0;
        }

        private bool CanActivate()
        {
            return !isActivated && !isCleared;
        }
        public void EnterRoom()
        {
            isActivated = true;
            LockDoors();
            ActivateMonsters();
        }

        void LockDoors()
        {
            foreach (var door in doors)
                door.LockDoor();
        }

        void ActivateMonsters()
        {
            foreach (var m in monsters)
                m.Activate();
        }

        public void CheckRoomClear()
        {
            foreach (var m in monsters)
            {
                if (!m.IsDead)
                    return;
            }

            ClearRoom();
        }

        void ClearRoom()
        {
            isCleared = true;
            foreach (var door in doors)
                door.UnlockDoor();

            Debug.Log("Room Cleared!" ); // 확인하ㅗ 지우자
        }
    }
}