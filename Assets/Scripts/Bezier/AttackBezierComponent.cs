using BezierWars.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BezierWars.Bezier
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(LineRenderer))]
    public class AttackBezierComponent : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            var damager = collider.GetComponent<IDamager>();
            if (collider.CompareTag("Player") && damager != null)
                damager.Damage = damager.Damage * 2f;
        }
    } 
}
