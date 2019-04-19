using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnerComponent : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.GameObject BulletPrefab;

    [SerializeField]
    private float shootIntensity;

    public void Spawn()
    {
        UnityEngine.GameObject bullet = UnityEngine.GameObject.Instantiate(BulletPrefab, transform.position, transform.rotation);
        bullet.tag = transform.tag;
        bullet.GetComponent<Rigidbody2D>().AddForce(
            shootIntensity * transform.up, ForceMode2D.Impulse);
    }
}
