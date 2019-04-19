using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CharacterSoundComponent : MonoBehaviour
{
    [SerializeField]
    private AudioClip attackSound;
    [SerializeField]
    private AudioClip deathSound;

    private void Start()
    {
        GetComponent<AudioSource>().loop = false;
    }
    //TODO: Refactor with only one method
    public void PlayAttackSound()
    {
        if (attackSound == null) return;
        var audiosrc = GetComponent<AudioSource>();
        audiosrc.clip = attackSound;
        audiosrc.Play();
    }

    public void PlayDeathSound()
    {
        if (deathSound == null) return;
        var audiosrc = GetComponent<AudioSource>();
        audiosrc.clip = deathSound;
        audiosrc.Play();
    }
}
