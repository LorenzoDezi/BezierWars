using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HermiteCurveComponent : AttackBezierCurveComponent
{

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(this.tag)) return;
        var hComp = collider.GetComponent<HealthComponent>();
        if (hComp != null)
            hComp.ForceDeath();
    }

    public override void Die()
    {
        foreach (BezierNodeComponent node in GetComponentsInChildren<BezierNodeComponent>())
        {
            node.transform.parent = null;
            node.DestroyNode();
        }
        Destroy(gameObject);
    }
}
