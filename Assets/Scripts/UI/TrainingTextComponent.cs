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

    public void ScrollText()
    {
        if (currentTextIndex >= textSequence.Count - 1) return;
        StopCoroutine("TextAnim");
        currentTextIndex++;
        text.text = "";
        StartCoroutine("TextAnim");
    }

    IEnumerator TextAnim()
    {
        char[] charArray = textSequence[currentTextIndex].ToCharArray();
        for(int i = 0; i < charArray.Length; i++)
        {
            var letter = charArray[i];
            //Find bold text to be rendered in one frame
            if (letter == '<')
            {
                //<b> is three characters long
                string boldTextToAdd = "" + letter + charArray[i+1] + charArray[i+2];
                int j = i + 3;
                while(letter != '>')
                {
                    letter = charArray[j++];
                    boldTextToAdd += letter;
                }
                text.text += boldTextToAdd;
                i = j-1;
            } else
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

    void Update()
    {
        if(Input.GetButtonDown(ScrollTextAxis))
            ScrollText();
    }
}
