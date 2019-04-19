using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyToSpawn;
    [SerializeField]
    private float spawnInterval = 20f;
    private float lastTimeSpawned = float.NegativeInfinity;
    [SerializeField]
    private int maxEnemiesCanSpawn = 5;
    private int currentEnemiesSpawned;

    public float SpawnInterval {
        get => spawnInterval; set => spawnInterval = value;
    }

    // Update is called once per frame
    void Update()
    {
        if(lastTimeSpawned + spawnInterval < Time.time && currentEnemiesSpawned <= maxEnemiesCanSpawn)
        {
            var enemy = GameObject.Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
            enemy.GetComponent<IEnemyController>().OnDefeat.AddListener(() => currentEnemiesSpawned--);
            lastTimeSpawned = Time.time;
            currentEnemiesSpawned++;
        }
    }
}
