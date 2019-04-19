using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

interface IEnemyController
{
    UnityEvent OnDefeat { get; }
    void Move(Vector2 position, float factor);
    void SetTarget(Transform target);
}

