using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleComponent : MonoBehaviour
{
    [Header("Destruction parameters")]
    [SerializeField]
    private float deathExplosionIntensity = 30f;
    [SerializeField]
    private GameObject deathExplosionEffect;
    [SerializeField]
    private float timeToDestroyPieces = 2.5f;

    public void StartDestroy()
    {
        for (var i = 0; i < gameObject.transform.childCount; i++)
        {
            var piece = transform.GetChild(i);
            if (!piece.gameObject.activeInHierarchy)
            {
                piece.gameObject.SetActive(true);
                piece.GetComponent<Rigidbody2D>()?.AddForce(
                    Random.insideUnitCircle * deathExplosionIntensity,
                    ForceMode2D.Impulse);
                GameObject.Destroy(piece.gameObject, timeToDestroyPieces);
            }
        }
        SpawnParticle(transform.position);
        transform.DetachChildren();
        GameObject.Destroy(gameObject);
    }

    private void SpawnParticle(Vector3 position)
    {
        //Particle System
        var particle = GameObject.Instantiate(deathExplosionEffect,
            position, Quaternion.identity);
        GameObject.Destroy(particle, 1.5f);
    }
}
