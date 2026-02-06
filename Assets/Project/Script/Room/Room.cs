using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Project.Room
{

    public class Room : MonoBehaviour
    {
        [SerializeField] private Door[] doors;
        [SerializeField] private bool isActiveRoom = false;
        [SerializeField] private bool isClearRoom = false;
        [SerializeField] private LayerMask playerMask;


        private bool LayerCheck(Collider2D col)
        {
            return ((1 << col.gameObject.layer) & playerMask) != 0;
        }
        private bool ActiveCheck()
        {
            return !isActiveRoom && !isClearRoom;
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            if(LayerCheck(col) && ActiveCheck())
            {

            }
        }




        void Start()
        {

        }

        void Update()
        {

        }
    }
}