using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IEntity
{
    [SerializeField] ColorData data;
    [SerializeField] private ColorInfo info;
    [SerializeField] private LayerMask layer;

    [Header("Enemy Settings")]
    [SerializeField] private float Health;
    [SerializeField] private float DefaultInvisibleCooldown;
    [SerializeField] private float DropChance;

    [Header("State Settings")]
    [SerializeField] private IdleSettings m_idleSettings;
    [SerializeField] private MovementSettings m_movementSettings;
    [SerializeField] private AttackSettings m_attackSettings;
    [SerializeField] private ViewSettings m_viewSettings;

    [Header("Handlers")]
    [SerializeField] private CombatHandler m_combatHandler;
    [SerializeField] private EnemyAlertHandler m_alertHandler;

    [Header("Sensor")]
    [SerializeField] private RangeSensor chaseSensor;
    [SerializeField] private RangeSensor leavingSensor;
    [SerializeField] private RangeSensor recognitionSensor;
    [SerializeField] private RangeSensor attackingSensor;
  
    [NonSerialized] public bool MaxDistanceReached;
    [NonSerialized] public bool wasChasing;
    private Recipe.ColorItems color;
    private SpriteRenderer _sr;
    private float _Health
    {
        get => Health;
        set
        {
            Health = value;
            if (Health <= 0) OnDeath();
        }
    }
    private bool isInvisible => InvisCooldown > Time.time;
    private float InvisCooldown;
    public RangeSensor ChaseSensor => chaseSensor;
    public enum State
    {
        Idle,
        Walk,
        View,
        Attack
    }
    private StateMachine<State> SM = new();
    public StateMachine<State> StateMachine => SM;

    public bool IsAlive => true;
    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        m_alertHandler.enemy = this;
        m_alertHandler.viewSettings = m_viewSettings;

        SM.AddState(new()
        {
            (State.Idle, new EnemyIdle(this, m_idleSettings, leavingSensor)),
            (State.Walk, new EnemyWalk(this, m_movementSettings, chaseSensor)),
            (State.Attack, new EnemyAttack(this, m_attackSettings, attackingSensor, m_combatHandler)),
            (State.View, new EnemyView(this, m_viewSettings, recognitionSensor, m_alertHandler))
        });
        SM.AddTransition(new()
        {
            (State.Walk, new(State.Idle, () => !leavingSensor.IsInRange && wasChasing)),
        });
        SM.AddGlobalTransition(new()
        {

        });
        SM.Initialize(State.Idle);
        chaseSensor.OnEnter += OnChaseRange;
        attackingSensor.OnEnter += OnAttackRange;
        recognitionSensor.OnEnter += OnRecognitionRange;
    }
    private void Update()
    {
        SM.OnLogic();
    }
    public void OnAttackRange() => SM.ChangeState(State.Attack);
    public void OnRecognitionRange() => SM.ChangeState(State.View);
    public void OnChaseRange() => SM.ChangeState(State.Walk);
    public void OnDeath()
    {
        DropManager.instance.DropColor(DropChance, color, transform.position);
        ObjectPoolManager.ReleaseObject(gameObject);
    }
    public void OnReceiveDamage(float value, float InvisDuration = -1, Transform origin = null)
    {
        if (value == 0 || isInvisible) return;
        InvisCooldown = (InvisDuration != -1 ? InvisDuration : DefaultInvisibleCooldown) + Time.time;
        _Health -= value;
    }
    public void SetColor(Recipe.ColorItems color)
    {
        this.color = color;
        //m_combatHandler.ChangeProjectiles(info.GetColor(color).projectiles, layer);
        _sr.color = data.GetColor(color);
    }

    [Serializable]
    public class IdleSettings
    {
        [Range(0,5)]
        public float maxIdleTime;
        public float maxDistanceIdleTime;
        public float playerLeavingIdleTime;
    }

    [Serializable]
    public class MovementSettings
    {
        public float patrolfurthestDistance;
        public float patrolOffset;
        public float patrolSpeed;

        [Header("Ledge Detection")]
        public Transform ledgePosition;
        public float ledgeDistance;
        public LayerMask layerMask;
    }
    [Serializable]
    public class AttackSettings
    {
        public float damage;
        [Range(0,2)]
        public float DMGTimeInterval;
    }
    [Serializable]
    public class ViewSettings
    {
        public float AlertGains;
    }
}
