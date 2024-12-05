using UnityEngine;
using System.Collections.Generic;

public interface IEntity
{
    public bool IsAlive { get; }
    public void OnDeath();
    public void OnReceiveDamage(float value, float InvisDuration = -1, Transform origin = null);
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

    [Header("Datas")]
    [SerializeField] PlayerData playerData;
    [SerializeField] ColorData colorData;
    [SerializeField] private ColorInfo info;

    [Header("Player Settings")]
    [SerializeField] private Player_Movement playerMovement;
    [SerializeField] private NewCombatHandler playerCombat;
    [SerializeField] private PlatformHandler platformHandler;

    [Header("Sensor Settings")]
    [SerializeField] private GroundSensor groundSensor;

    public float CurrentHealth => _Health;
    public float MaxHealth => maxHealth;

    private float maxHealth;
    private float health;
    public Rigidbody2D rb2d { get; private set; }
    public Animator animator { get; private set; }
    private SpriteRenderer _sr;

    private float _Health
    {
        get => health; 
        set
        {
            health = value;
            if (health <= 0) OnDeath();
        }
    }
    private bool isInvisible => InvisCooldown > Time.time;
    public Vector2 InputValue => input.Player.Movement.ReadValue<Vector2>();
    public bool IsGrounded { get => groundSensor.IsGrounded; }
    public bool IsAlive => true;
    public void SetColor(Recipe.ColorItems color)
    {
        Color_ = color;
        playerData = info.GetInfo(color).data;
        playerCombat.data = playerData.CombatData;
        playerMovement.data = playerData.MovementData;
        _sr.color = colorData.GetColor(color);
    }
    public Recipe.ColorItems Color_;
    readonly StateMachine<State> SM = new();
    public StateMachine<State> StateMachine => SM;

    private float InvisCooldown;
    private PlayerControl input;
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
        _sr = GetComponent<SpriteRenderer>();
        SetColor(Color_);

        maxHealth = playerData.playerStats.MaxHealth;
        _Health = maxHealth;

        SM.AddState(new()
        {
            (State.idle, new _PlayerIdleState(animator, rb2d)),
            (State.walk, new _PlayerWalkState(animator, rb2d, playerData.MovementData)),
            (State.onAir, new _PlayerOnAirState(animator, rb2d, playerData.MovementData)),
            (State.onHit, new _PlayerOnHitState(animator, rb2d, gameObject)),
        });
        SM.AddGlobalTransition(new List<Transition<State>>()
        {
            new(State.onAir, () => !IsGrounded),
            new(State.idle, () => IsGrounded && rb2d.velocity.magnitude <= 0.2),
            new(State.walk, () => IsGrounded && Mathf.Abs(rb2d.velocity.x) > 0.2)
        });
        SM.Initialize(State.idle);

        input.Player.Jump.performed += (x) => playerMovement.Jump_Performed();
        input.Player.Jump.canceled += (x) => playerMovement.Jump_Canceled();
        input.Player.Attack.performed += (x) =>
        {
            if (IsGrounded && !playerMovement.isDashing) playerCombat.Attack();
        };
        input.Player.Dash.performed += (x) => playerMovement.Dash(InputValue);
        input.Player.Projectile.performed += (x) => playerCombat.Projectile();
        input.Player.Skill.performed += (x) => playerCombat.Skill();
    }
    private void Update()
    {
        if(!playerMovement.isDashing) playerMovement.Move(InputValue.x * (playerCombat.isAttacking == null ? 1 : 0));
        if (InputValue.y < 0) platformHandler.GoDown();
        SM.OnLogic();
    }
    public void OnDeath()
    {
        ObjectPoolManager.ReleaseObject(gameObject);
    }
    public void OnReceiveDamage(float value, float InvisDuration = -1, Transform origin = null)
    {
        if (value == 0 || isInvisible) return;
        InvisCooldown = (InvisDuration != -1 ? InvisDuration : playerData.playerStats.DefaultInvisibleCooldown) + Time.time;
        _Health -= value;
        if (health > 0 && origin != null)
        {
            rb2d.AddForce(origin.right * playerData.playerStats.KnockbackMagnitude, ForceMode2D.Impulse);
            SM.ChangeState(State.onHit, 1);
        }
    }
}