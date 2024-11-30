using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEntity
{
    [Header("Enemy Settings")]


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
    private void Awake()
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
    }
    public void OnReceiveDamage(float value, Transform origin = null)
    {
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
