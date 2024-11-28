using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditorInternal;
using UnityEngine.Timeline;

public interface IEntity
{
    public void OnDeath();
    public void OnReceiveDamage(float value, Transform origin = null);
}
public class Player : MonoBehaviour, IEntity
{
    public enum State
    {
        idle,
        walk, 
        onAir,
        onHit
    }

    [Header("Player Stats")]
    [SerializeField] private float Health;
    [SerializeField] private float KnockbackMagnitude;

    [Header("Player Settings")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlatformHandler platformHandler;

    [Header("Sensor Settings")]
    [SerializeField] private GroundSensor groundSensor;
    public Rigidbody2D rb2d { get; private set; }
    public Animator animator { get; private set; }

    private PlayerControl input;
    readonly StateMachine<State> SM = new();
    public StateMachine<State> StateMachine => SM;
    private void OnEnable()
    {
        input.Player.Enable();
    }
    private void OnDisable()
    {
        input.Player.Disable();
    }
    private void Awake()
    {
        input = new();
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //GameManager.input = input;
        playerMovement.player = this;
        playerCombat.player = this;
        SM.AddState(new()
        {
            (State.idle, new _PlayerIdleState(animator, rb2d)),
            (State.walk, new _PlayerWalkState(animator, rb2d, playerMovement.speed)),
            (State.onAir, new _PlayerOnAirState(animator, rb2d, playerMovement.jump)),
            (State.onHit, new _PlayerOnHitState(animator, rb2d, gameObject)),
        });
        SM.AddGlobalTransition(new List<Transition<State>>()
        {
            new(State.onAir, () => !IsGrounded),
            new(State.idle, () => IsGrounded && rb2d.velocity.magnitude <= 0.2),
            new(State.walk, () => IsGrounded && Mathf.Abs(rb2d.velocity.x) > 0.2)
        });
        SM.Initialize(State.idle);

        input.Player.Jump.performed += playerMovement.Jump_Performed;
        input.Player.Jump.canceled += playerMovement.Jump_Canceled;
        input.Player.Attack.performed += (x) => playerCombat.Attack();
    }
    private void Update()
    {
        playerMovement.Move(InputValue.x * (playerCombat.isAttacking == null ? 1 : 0));
        if (InputValue.y < 0) platformHandler.GoDown();
        SM.OnLogic();
    }
    public void OnDeath()
    {
        //animator.SetBool("Dead", true);
        //GameManager.instance.GameOver();
    }
    public void OnReceiveDamage(float value, Transform origin = null)
    {
        if (value == 0) return;
        Health -= value;
        if (Health <= 0) OnDeath();
        else
        {
            rb2d.AddForce(origin.right * KnockbackMagnitude, ForceMode2D.Impulse);
            SM.ChangeState(State.onHit, 1);
        }
    }
    public Vector2 InputValue => input.Player.Movement.ReadValue<Vector2>();
    public bool IsGrounded => groundSensor.IsGrounded;
    public float LedgeTime => groundSensor.LedgeTime;
}
