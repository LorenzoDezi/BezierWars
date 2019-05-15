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
        currentTextIndex = 0;
        ScrollText();
    }

    public void ScrollText()
    {
        if (textSequence.Count < 1) return;
        if (currentTextIndex >= textSequence.Count)
            currentTextIndex = 0 ;
        text.text = textSequence[currentTextIndex];
        currentTextIndex++;
    }

    private void OnEnable()
    {
        ResetText();
    }

}
