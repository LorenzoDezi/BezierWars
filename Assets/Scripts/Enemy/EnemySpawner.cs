using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.GameObject enemyToSpawn;
    [SerializeField]
    private float spawnInterval = 20f;
    private float currentSpawnInterval;
    private float lastTimeSpawned = float.NegativeInfinity;
    [SerializeField]
    private int maxEnemiesCanSpawn = 5;
    private int currentMaxEnemiesCanSpawn;
    private int currentEnemiesSpawned;
    private Transform target;

    public float SpawnInterval {
        get => currentSpawnInterval; set => currentSpawnInterval = value;
    }

    public int MaxEnemiesCanSpawn
    {
        get => currentMaxEnemiesCanSpawn; set => currentMaxEnemiesCanSpawn = value;
    }

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        currentMaxEnemiesCanSpawn = maxEnemiesCanSpawn;
        currentSpawnInterval = spawnInterval;
        lastTimeSpawned = float.NegativeInfinity;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    // Update is called once per frame
    void Update()
    {
        if(lastTimeSpawned + currentSpawnInterval < Time.time && currentEnemiesSpawned <= currentMaxEnemiesCanSpawn)
        {
            var enemy = UnityEngine.GameObject.Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
            enemy.GetComponent<EnemyAI>().SetTarget(target);
            enemy.GetComponent<IEnemyController>().OnDefeat.AddListener(() => currentEnemiesSpawned--);
            lastTimeSpawned = Time.time;
            currentEnemiesSpawned++;
        }
    }
}
