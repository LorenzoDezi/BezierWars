using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this);
    }
    
    public static void PlaySound(AudioClip clip, float volumeScale = 1f)
    {
        instance.GetComponent<AudioSource>().PlayOneShot(clip, volumeScale);
    }
}
