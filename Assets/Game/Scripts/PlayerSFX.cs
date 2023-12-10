using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSFX : MonoBehaviour
{
    [SerializeField] private AudioClip missHit;
    [SerializeField] private AudioClip goodHit;
    [SerializeField] private AudioClip perfectHit;

    [Header("References")]
    [SerializeField] private AudioSource _audioSource;

    private void Awake()
    {
        if(_audioSource == null)
            _audioSource = GetComponent<AudioSource>();
    }

    public void PlayMissHit()
    {
        _audioSource.PlayOneShot(missHit);
    }

    public void PlayGoodHit()
    {
        _audioSource.PlayOneShot(goodHit);
    }

    public void PlayPerfectHit()
    {
        _audioSource.PlayOneShot(perfectHit);
    }

    public void PlaySFX(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
