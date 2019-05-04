using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class BezierMath
{
    public static int Factorial(int val)
    {
        int fact = 1;
        for (int i = 2; i <= val; i++)
            fact *= i;
        return fact;
    }

    public static Vector3 BernsteinBezier(float t, int k, Vector3[] controlPoints)
    {
        Vector3 result = new Vector3();
        for (int i = 0; i <= k; i++)
        {
            result += 
                controlPoints[i] * BernsteinPolynomial(i, k, t);
        }
        return result;
    }

    public static float BernsteinPolynomial(int i, int k, float t)
    {
        return Mathf.Pow(t, i) * Mathf.Pow(1 - t, k - i)
                * Factorial(k) / (Factorial(k - i) * Factorial(i));
    }

}

