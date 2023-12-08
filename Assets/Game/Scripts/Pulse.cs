using UnityEngine;

public class Pulse : MonoBehaviour
{
    [SerializeField] private float _pulseSize = 1.5f;
    [SerializeField] private float _returnSpeed = 5f;
    private Vector3 _baseSize;

    private void Start() {
        _baseSize = transform.localScale;
    }

    private void Update() {
        transform.localScale = Vector3.Lerp(transform.localScale, _baseSize, Time.deltaTime * _returnSpeed);
    }

    public void Puulse() {
        transform.localScale = _baseSize * _pulseSize;
    }
}
