using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundColorComponent : MonoBehaviour
{
    [SerializeField]
    private float colorShiftSpeed = -0.0005f;

    private void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 1f);
    }

    void Update()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        var color = spriteRenderer.color;
        color.b = Mathf.Clamp01(color.b + colorShiftSpeed);
        color.r = Mathf.Clamp01(color.r - colorShiftSpeed);
        if (color.b <= 0 || color.b >= 1) colorShiftSpeed = -colorShiftSpeed;
        spriteRenderer.color = color;
    }
}
