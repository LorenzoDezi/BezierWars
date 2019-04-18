using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace BezierWars.Extensions
{
    public static class MyUnityExtensions
    {
        public static bool IsObjectVisible(this UnityEngine.Camera @this, Renderer renderer)
        {
            return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(@this), renderer.bounds);
        }
    }

}
