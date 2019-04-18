using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnerComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject BulletPrefab;

    [SerializeField]
    private float shootIntensity;

    [SerializeField]
    private float spawnInterval = 0.5f;

    private bool canSpawn = true;
    private float timeFromLastSpawn = 0f;

    private void Update()
    {
        timeFromLastSpawn += Time.deltaTime;
        if(timeFromLastSpawn >= spawnInterval)
        {
            timeFromLastSpawn = 0f;
            canSpawn = true;
        }
    }

    public void Spawn()
    {
        if (!canSpawn)
            return;
        GameObject bullet = GameObject.Instantiate(BulletPrefab, transform.position, transform.rotation);
        bullet.tag = transform.tag;
        bullet.GetComponent<Rigidbody2D>().AddForce(
            shootIntensity * transform.up, ForceMode2D.Impulse);
        timeFromLastSpawn = 0f;
        canSpawn = false;
    }
}
