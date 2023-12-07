using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _moveDuration;
    [SerializeField] private AnimationCurve _moveCurve;
    
    private Animator            m_animator;
    private bool                m_isWallSliding = false;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;
    private float               m_rollDuration = 8.0f / 14.0f;
    private float               m_rollCurrentTime;

    private float _beatTime;
    private float _inputTime;
    private bool _inputDone;
    private bool _beatDone;
    private Action _actionDone;

    private float _direction;
    
    private Vector2 _nextPos;
    private Vector2 _startPos;

    private Vector2 speedVector;

    private float _elapsedTime;

    private Rigidbody2D _rb;
    
    private PlayerInput _input;

    private bool _isMoving = false;

    private bool _getHit;

    private bool _blocking;
    
    private void Awake() {
        _input = new PlayerInput();
    }

    private void OnEnable() {
        _input.Enable();

        _input.Player.Move.performed += MovePerformed;
        _input.Player.Attack.performed += AttackPerformed;
        _input.Player.Block.performed += BlockPerformed;
    }

    private void OnDisable() {
        _input.Disable();
    }

    void Start()
    {
        m_animator = GetComponent<Animator>();

        speedVector = new Vector2(_speed, 0);
        _rb = GetComponent<Rigidbody2D>();
    }
    
    void Update() {
        if (_isMoving)
        {
            _elapsedTime += Time.deltaTime;
            float percentageComplete = _elapsedTime / _moveDuration;
            
            _rb.MovePosition(Vector2.Lerp(_startPos, _nextPos, _moveCurve.Evaluate(percentageComplete)));

            if ((Vector2)transform.position == _nextPos) _isMoving = false;
        }
        
    }
    
    private void BlockPerformed(InputAction.CallbackContext obj) {
        if (!_inputDone && !_beatDone)
        {
            _inputTime = Time.time;
            _inputDone = true;
            _actionDone = global::Action.Block;
            Action();
        }
    }

    private void AttackPerformed(InputAction.CallbackContext obj) {
        if (!_inputDone && !_beatDone)
        {
            _inputTime = Time.time;
            _inputDone = true;
            _actionDone = global::Action.Attack;
            Action();
        }
    }

    private void MovePerformed(InputAction.CallbackContext obj) {
        if (!_inputDone && !_beatDone)
        {
            _inputTime = Time.time;
            _inputDone = true;
            _actionDone = global::Action.Move;
            _direction = obj.ReadValue<float>();
            Action();
        }
    }

    private void Action() {
        if (Mathf.Abs(_beatTime-_inputTime) > 0.5f) return;
        switch (_actionDone)
        {
            case global::Action.Attack:
                Attack();
                break;
            case global::Action.Block:
                Block();
                break;
            case global::Action.Move:
                Move();
                break;
        }


        _beatDone = true;
        _inputDone = false;
    }
    
    public void HandleBeat() {
        if (_getHit && !_blocking)
        {
            TakeDamage();
        }
        
        _beatTime = Time.time;
        _inputDone = false;
        _beatDone = false;
        _getHit = false;
        _blocking = false;
        m_animator.SetInteger("AnimState", 0);
    }

    private void TakeDamage() {
        _getHit = false;
        _nextPos = (Vector2)transform.position - (speedVector * _direction);
        m_animator.SetTrigger("Hurt");
    }

    private void Block() {
        _blocking = true;
        m_animator.SetTrigger("Block");
        m_animator.SetBool("IdleBlock", true);
    }

    private void Attack() {
        m_currentAttack++;

        // Loop back to one after third attack
        if (m_currentAttack > 3)
            m_currentAttack = 1;

        // Reset Attack combo if time since last attack is too large
        if (m_timeSinceAttack > 1.0f)
            m_currentAttack = 1;

        // Call one of three attack animations "Attack1", "Attack2", "Attack3"
        m_animator.SetTrigger("Attack" + m_currentAttack);

        // Reset timer
        m_timeSinceAttack = 0.0f;
    }

    private void Move() {
        _startPos = transform.position;
        _elapsedTime = 0;
        if (_direction>0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            _nextPos = (Vector2)transform.position + speedVector;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
            _nextPos = (Vector2)transform.position - speedVector;
        }
        m_animator.SetInteger("AnimState", 1);
        _isMoving = true;
    }

    public void IsHit() {
        _getHit = true;
    }
}

enum Action
{
    Block,
    Attack,
    Move,
}