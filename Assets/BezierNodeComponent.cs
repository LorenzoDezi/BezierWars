using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierNodeComponent : MonoBehaviour, Damageable
{
    void Damageable.Damaged()
    {
        ;
    }

    void Damageable.Die()
    {
        Debug.Log("Node removed!");
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
