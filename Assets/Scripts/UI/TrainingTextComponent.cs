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
    [SerializeField]
    public float letterPaused = 0.01f;

    [Header("Input axis")]
    [SerializeField]
    private string ScrollTextAxis = "ScrollText";

    void Reset()
    {
        text = GetComponent<Text>();
        currentTextIndex = -1;
        ScrollText();
    }

    private void ScrollText()
    {
        currentTextIndex++;
        text.text = "";
        StartCoroutine("TextAnim");
    }

    IEnumerator TextAnim()
    {
        //Split each char into a char array
        foreach (char letter in textSequence[currentTextIndex].ToCharArray())
        {
            //Add 1 letter each
            text.text += letter;
            yield return 0;
            yield return new WaitForSeconds(letterPaused);
        }
    }

    private void OnEnable()
    {
        Reset();
    }

    private void OnDisable()
    {
        StopCoroutine("TextAnim");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown(ScrollTextAxis) && currentTextIndex < textSequence.Count-1)
        {
            StopCoroutine("TextAnim");
            ScrollText();
        }
    }
}
