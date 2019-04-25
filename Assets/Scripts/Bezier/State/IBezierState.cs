using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IBezierState
{
    /// <summary>
    /// Manages the input for the BezierSpawner.
    /// </summary>
    /// <returns> The new state if the input has changed the state </returns>
    IBezierState HandleInput();
    /// <summary>
    /// It must be called before entering the state.
    /// </summary>
    /// <param name="spawner">The BezierSpawner for initialization</param>
    void Enter(BezierSpawner spawner);
    /// <summary>
    /// It must be called before leaving the state.
    /// </summary>
    void Exit();
}

