using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Monster
{
    public class MonsterSpawner : MonoBehaviour
    {

        [SerializeField] private Monster[] monsterPrefabs;
        [SerializeField] private int monsterCount = 5; // 마릿수
        [SerializeField] private Vector2 spawnArea = new Vector2(15f, 5f); // 스폰 영역 크기
        [SerializeField] private float minSpawnDis = 2f; // 최소 스폰 거리
        
        public Monster[] SpawnMonsters()
        {
            Monster[] monsters = new Monster[monsterCount];

            for(int i =0; i<monsterCount; i++)
            {
                Vector2 spawnPos = GetValidSpawnPos(monsters, i);
                Monster prefab = GetRandomMonsterPrefab();

                Monster monster = Instantiate(prefab, spawnPos, Quaternion.identity, transform);
                monster.gameObject.SetActive(false);
                monsters[i] = monster;
            }
            return monsters;
        }



        Vector2 GetValidSpawnPos(Monster[] monsters, int currentIndex)
        {
            int maxAttempt = 40;

            for (int attempt = 0; attempt < maxAttempt; attempt++)
            {
                Vector2 randomPos = GetRandomPos();

                bool isValid = true;
                for (int i =0; i < currentIndex; i++)
                {
                    if (monsters[i] != null)
                    {
                        float dis = Vector2.Distance(randomPos, monsters[i].transform.position);
                        if (dis < minSpawnDis)
                        {
                            isValid = false;
                            break;
                        }
                    }
                }
                if (isValid) return randomPos;
            }
            return GetRandomPos(); // 유효한 위치를 찾지 못하면 그냥 랜덤 위치 반환
        }
        Vector2 GetRandomPos()
        {
            float rX = Random.Range(-spawnArea.x / 2, spawnArea.x / 2);
            float rY = Random.Range(-spawnArea.y / 2, spawnArea.y / 2);
            return (Vector2)transform.position + new Vector2(rX, rY);
        }
        Monster GetRandomMonsterPrefab()
        {
            return monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
        }
        private void OnDrawGizmosSelected() //확인용 스폰영역
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, spawnArea);
        }
        void Start()
        {

        }

        void Update()
        {

        }
    }
}