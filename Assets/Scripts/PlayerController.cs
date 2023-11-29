using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_rollForce = 6.0f;
    [SerializeField] bool       m_noBlood = false;
    [SerializeField] GameObject m_slideDust;

    [SerializeField] private PlayerInput _input;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private bool                m_isWallSliding = false;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;
    private float               m_rollDuration = 8.0f / 14.0f;
    private float               m_rollCurrentTime;
    // Start is called before the first frame update

    private float _beatTime;
    private float _inputTime;
    private bool _inputDone;
    private bool _beatDone;
    private Action _actionDone;

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
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    private void BlockPerformed(InputAction.CallbackContext obj) {
        Debug.Log("performed");
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
        
    }

    private void Action() {
        if (Mathf.Abs(_beatTime-_inputTime) > 0.4f) return;
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
        _beatTime = Time.time;
        _inputDone = false;
        _beatDone = false;
    }

    private void Block() {
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
        
    }
}

enum Action
{
    Block,
    Attack,
    Move,
}