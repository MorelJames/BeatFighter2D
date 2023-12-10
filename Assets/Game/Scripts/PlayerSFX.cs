using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSFX : MonoBehaviour
{
    [Header("Attack SFX")]
    [SerializeField] private AudioClip missHit;
    [SerializeField] private AudioClip goodHit;
    [SerializeField] private AudioClip perfectHit;

    [Header("Block SFX")]
    [SerializeField] private AudioClip block;

    [Header("Get Hit SFX")]
    [SerializeField] private AudioClip getHit;

    [Header("Move SFX")]
    [SerializeField] private AudioClip move;

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

    public void PlayBlockHit()
    {
        _audioSource.PlayOneShot(block);
    }

    public void PlayGetHit()
    {
        _audioSource.PlayOneShot(getHit);
    }

    public void PlayMove()
    {
        _audioSource.PlayOneShot(move);
    }

    public void PlaySFX(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
