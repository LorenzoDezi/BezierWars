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

    // Update is called once per frame
    void UpdateText(int value)
    {
        GetComponent<Text>().text = value.ToString();
    }
}
