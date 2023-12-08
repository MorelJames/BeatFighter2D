using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BeatManager : MonoBehaviour
{
    [SerializeField] private float _bpm;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Intervals[] _intervals;

    private void Update() {
        foreach (Intervals interval in _intervals)
        {
            float sampledTime = (_audioSource.timeSamples / (_audioSource.clip.frequency * interval.GetBeatLength(_bpm)));
            interval.CheckForNewInterval(sampledTime);
        }
    }
}

[System.Serializable]
internal class Intervals
{
    [SerializeField] private float _steps;
    [SerializeField] private UnityEvent _trigger;
    private int _lastInterval;

    public float GetBeatLength(float bpm) {
        return 60f / (bpm * _steps);
    }

    public void CheckForNewInterval(float interval) {
        if (Mathf.FloorToInt(interval) != _lastInterval)
        {
            _lastInterval = Mathf.FloorToInt(interval);
            _trigger.Invoke();
        }
    }
}
