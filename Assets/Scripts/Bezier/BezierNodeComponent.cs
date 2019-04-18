using BezierWars.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BezierWars.Bezier
{
    public class BezierNodeComponent : MonoBehaviour, IDamageable
    {
        private UnityEvent died;

        public UnityEvent Died => died;

        private void Awake()
        {
            died = new UnityEvent();
        }

        void IDamageable.Damaged()
        {
            //TODO: Particle System
        }

        void IDamageable.Die()
        {
            //TODO: Particle System
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
}
