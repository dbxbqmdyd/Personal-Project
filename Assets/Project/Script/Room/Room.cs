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

        public void EnterRoom()
        {
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
        }
    }
}