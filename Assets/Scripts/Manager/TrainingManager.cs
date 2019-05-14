using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    [SerializeField]
    private TrainingVideoComponent videoComp;
    [SerializeField]
    private TrainingTextComponent textComp;

    [Header("Input axis")]
    [SerializeField]
    private string ScrollAxis = "ScrollText";

    void Update()
    {
        if(Input.GetButtonDown(ScrollAxis))
        {
            videoComp.ScrollVideo();
            textComp.ScrollText();
        }
        
    }
}
