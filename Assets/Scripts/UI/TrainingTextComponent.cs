using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TrainingTextComponent : MonoBehaviour
{
    private Text text;
    [Header("Text")]
    [SerializeField]
    List<string> textSequence;
    int currentTextIndex;


    
    void ResetText()
    {
        text = GetComponent<Text>();
        currentTextIndex = -1;
        ScrollText();
    }

    public void ScrollText()
    {
        if (textSequence.Count < 1) return;
        if (currentTextIndex >= textSequence.Count - 1)
            currentTextIndex = 0 ;
        currentTextIndex++;
        text.text = textSequence[currentTextIndex];
    }

    private void OnEnable()
    {
        ResetText();
    }

}
