using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnerComponent : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.GameObject BulletPrefab;

    [SerializeField]
    private float shootIntensity;
    float offset = 2f;

    public void Spawn()
    {
        Spawn(transform.position, transform.rotation);
    }

    public void Spawn(Vector3 position, Quaternion rotation, bool powered = false)
    {
        
        UnityEngine.GameObject bullet = UnityEngine.GameObject.Instantiate(BulletPrefab, 
            position, rotation);
        bullet.tag = transform.tag;
        bullet.GetComponent<BulletBehaviorComponent>().Powered = powered;
        bullet.GetComponent<Rigidbody2D>().AddForce(
            shootIntensity * bullet.transform.up, ForceMode2D.Impulse);
    }

    public void Spawn(Transform startPoint, int n)
    {
        float angularRot = 180f / n;
        Vector3 startSpawnPosition = startPoint.position + startPoint.forward * offset;
        for(float rot = 0; rot < 90f; rot+=angularRot)
        {
            Spawn(startSpawnPosition, Quaternion.Euler(startPoint.forward * rot));
            Spawn(startSpawnPosition, Quaternion.Euler(startPoint.forward * -rot));
        }
        Spawn(startSpawnPosition, startPoint.rotation);
    }
}
