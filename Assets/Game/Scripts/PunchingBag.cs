using UnityEngine;

public class PunchingBag : MonoBehaviour
{
    [SerializeField] private int _attackTempo;

    private int _beatBeforeAttack;

    private bool _isAttacking;
    
    [SerializeField] private Color _baseColor;
    [SerializeField] private Color _preparingAttackColor;

    [SerializeField] private float _pulseSize = 1.5f;
    [SerializeField] private float _returnSpeed = 5f;
    private Vector3 _baseSize;

    private SpriteRenderer _spriteRenderer;
    private Animator m_animator;

    private bool _hitAttack;
    // Start is called before the first frame update
    void Start() {
        _isAttacking = false;
        _beatBeforeAttack = _attackTempo;
        _baseSize = transform.localScale;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
        m_animator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isAttacking)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _baseSize, Time.deltaTime * _returnSpeed);
        }
    }

    public void HandleBeat() {
        if (!_isAttacking)
        {
            if (_beatBeforeAttack == 0)
            {
                Attack();
            }else if (_beatBeforeAttack == 1 )
            {
                PreparingAttack();
                _beatBeforeAttack--;
            }
            else
            {
                _beatBeforeAttack--;
                Pulse();
            }

        }
    }

    private void PreparingAttack() {
        m_animator.enabled = true;
        m_animator.Play("PunchingBagPrepareAttack");
        _isAttacking = false;   
        _spriteRenderer.color = _preparingAttackColor;
        m_animator.SetBool("IsPreparing",true);
        m_animator.SetBool("IsAttacking",false);
    }
    private void Attack() {
        m_animator.SetBool("IsPreparing",false);
        m_animator.SetBool("IsAttacking",true);
        _spriteRenderer.color = _baseColor;
        _isAttacking = true;
    }

    public void AttackFinished() {
        m_animator.SetBool("IsAttacking",false);
        _beatBeforeAttack = _attackTempo;
        m_animator.enabled = false;
        _isAttacking = false;
        _hitAttack = false;
    }

    private void Pulse() {
        transform.localScale = _baseSize * _pulseSize;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.tag.Contains("Player") && _isAttacking && !_hitAttack)
        {
            other.GetComponent<PlayerController>().IsHit();
            _hitAttack = true;
        }
    }
}
