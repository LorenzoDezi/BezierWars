using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BezierWars.Enemy
{
    interface IEnemyController
    {
        void Move(Vector2 position, float factor);
        void SetTarget(Transform target);
    } 
}

