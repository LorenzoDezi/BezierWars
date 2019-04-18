using BezierWars.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BezierWars.Bezier
{
    [RequireComponent(typeof(Collider2D))]
    public class DefenseBezierComponent : MonoBehaviour, IDamageable
    {

        private Transform transformToFollow;
        private Vector3 offset;

        private void Start()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void Update()
        {
            if (transformToFollow == null)
                return;
            transform.position = transformToFollow.position + offset;
            Debug.Log(offset);
        }

        public void Damaged()
        {
            //TODO Add particle systems and effects
        }

        public void Die()
        {
            GetComponent<BezierBuilderComponent>().Disabled.Invoke();
            Destroy(gameObject, 0.2f);
        }

        public void SetTargetToFollow(Transform transformToFollow)
        {
            this.transformToFollow = transformToFollow;
            this.offset = transform.position - transformToFollow.position;
        }
    } 
}
