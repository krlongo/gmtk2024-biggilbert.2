using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource source; // insturment
    public AudioClip musicClip; // song

    void Start()
    {
        source.clip = musicClip;
        source.Play(); // play music on start
    }

    void Update()
    {
        if(!source.isPlaying)
        {
            source.Play();
        }
    }
}
