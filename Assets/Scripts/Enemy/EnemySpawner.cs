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

    public float SpawnInterval {
        get => spawnInterval; set => spawnInterval = value;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Valuta un numero fisso di nemici per tempo
        if(lastTimeSpawned + spawnInterval < Time.time)
        {
            GameObject.Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
            lastTimeSpawned = Time.time;
        }
    }
}
