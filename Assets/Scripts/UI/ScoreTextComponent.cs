using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTextComponent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnScoreChanged().AddListener(UpdateText);
    }

    void UpdateText(int value)
    {
        GetComponent<Text>().text = value.ToString();
    }
}
