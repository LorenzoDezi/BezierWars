using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermiteNodeComponent : BezierNodeComponent
{
    private bool movEnabled;
    [SerializeField]
    private float rotSpeed;
    [SerializeField]
    private SpriteRenderer radarRenderer;
    public bool IsHermitePlacing { get; private set; } = true;

    private void OnMouseDown()
    {
        if (IsHermitePlacing)
            movEnabled = true;
    }

    private void OnMouseUp()
    {
        movEnabled = false;
    }

    /// <summary>
    /// This method must be called when the hermite curve is built.
    /// </summary>
    public void Consume()
    {
        IsHermitePlacing = false;
        radarRenderer.enabled = false;
    }

    private void OnMouseDrag()
    {
        if (!movEnabled) return;
        Vector3 mousePosition = new Vector3(
            Input.mousePosition.x, Input.mousePosition.y, transform.position.z
            );
        Vector3 direction = Camera.main.ScreenToWorldPoint(mousePosition) - transform.position;
        direction.Normalize();
        direction.z = 0;
        float angle = Vector3.SignedAngle(transform.up, direction, transform.forward);
        angle = angle > 180 ? -angle : angle;
        transform.Rotate(0, 0, angle * rotSpeed * Time.deltaTime);
    }
}
