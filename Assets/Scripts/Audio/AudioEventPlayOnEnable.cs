using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioEventPlayOnEnable : MonoBehaviour
{
    
    public AudioEvent audioEvent;
    
    private AudioSource _audioSource;
    
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    public void OnEnable()
    {
        audioEvent.Play(_audioSource);
    }
}
