using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Enemy : MonoBehaviour, IEntity
{
    [SerializeField] private IdleSettings m_IdleSettings;
    [SerializeField] private MovementSettings m_movementSettings;


    [Header("Sensor")]
    [SerializeField] private RangeSensor chaseSensor;
    [SerializeField] private RangeSensor leavingSensor;
    [SerializeField] private RangeSensor recognitionSensor;
    [SerializeField] private RangeSensor attackingSensor;

    [NonSerialized] public bool MaxDistanceReached;
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
        SM.AddState(new()
        {
            (State.Idle, new EnemyIdle(this, m_IdleSettings, leavingSensor)),
            (State.Walk, new EnemyWalk(this, m_movementSettings, chaseSensor)),
            (State.Attack, new EnemyAttack()),
            (State.View, new EnemyView())
        });
        SM.AddTransition(new()
        {
            (State.Walk, new(State.Idle, () => !leavingSensor.IsInRange)),
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
}
