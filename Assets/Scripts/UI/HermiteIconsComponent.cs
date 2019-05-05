using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HermiteIconsComponent : MonoBehaviour
{
    [SerializeField]
    private List<Image> hermiteIcons;
    private BezierSpawner bezierSpawner;
    private int currentRemovalIndex;

    private void ResetUI()
    {
        hermiteIcons.ForEach((obj) => obj.enabled = true);
        currentRemovalIndex = hermiteIcons.Count - 1;
        bezierSpawner = GameManager.GetCurrentBezierSpawner()?.GetComponent<BezierSpawner>();
        if (bezierSpawner != null)
            bezierSpawner.HermiteConsumed.AddListener(RemoveIcon);
    }

    private void OnEnable()
    {
        ResetUI();
    }

    public void RemoveIcon()
    {
        if (currentRemovalIndex < 0) return;
        hermiteIcons[currentRemovalIndex].enabled = false;
        currentRemovalIndex--;
    }
}
