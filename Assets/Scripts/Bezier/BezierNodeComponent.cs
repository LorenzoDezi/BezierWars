using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BezierNodeComponent : MonoBehaviour
{
    private UnityEvent died;

    public UnityEvent Died => died;
    [SerializeField]
    private GameObject deathParticleSystem;

    private void Awake()
    {
        died = new UnityEvent();
    }

    public void DestroyNode()
    {
        var particleSystem = GameObject.Instantiate(deathParticleSystem, gameObject.transform);
        Destroy(gameObject, 0.5f);
    }
}
