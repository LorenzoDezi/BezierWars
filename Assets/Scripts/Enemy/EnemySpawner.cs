using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.GameObject enemyToSpawn;
    [SerializeField]
    private float spawnInterval = 20f;
    private float currentSpawnInterval;
    private float lastTimeSpawned;
    [SerializeField]
    private int maxEnemiesCanSpawn = 5;
    private int currentMaxEnemiesCanSpawn;
    private int currentEnemiesSpawned;
    public UnityEvent Disabled { get; private set; }

    private void Start()
    {
        currentMaxEnemiesCanSpawn = maxEnemiesCanSpawn;
        currentSpawnInterval = spawnInterval;
        lastTimeSpawned = float.NegativeInfinity;
        currentEnemiesSpawned = 0;
        Disabled = new UnityEvent();
    }

    public float SpawnInterval {
        get => currentSpawnInterval; set => currentSpawnInterval = value;
    }

    public int MaxEnemiesCanSpawn
    {
        get => currentMaxEnemiesCanSpawn; set => currentMaxEnemiesCanSpawn = value;
    }

    void OnEnable()
    {
        Start();
    }

    void OnDisable()
    {
        Disabled.Invoke();
    }
    // Update is called once per frame
    void Update()
    {
        if(lastTimeSpawned + currentSpawnInterval < Time.time && currentEnemiesSpawned <= currentMaxEnemiesCanSpawn)
        {
            var enemy = UnityEngine.GameObject.Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
            //Add listener on bezSpawner disable (handling game state change)
            Disabled.AddListener(() => GameObject.Destroy(enemy));
            enemy.GetComponent<IEnemyController>().OnDefeat.AddListener(() => currentEnemiesSpawned--);
            lastTimeSpawned = Time.time;
            currentEnemiesSpawned++;
        }
    }
}
