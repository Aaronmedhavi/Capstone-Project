using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyIdle : BaseState
{
    Enemy.IdleSettings settings;
    RangeSensor leavingSensor;
    Enemy enemy;

    float idleTime;
    public EnemyIdle(Enemy enemy, Enemy.IdleSettings settings, RangeSensor leavingSensor)
    {
        this.enemy = enemy;
        this.settings = settings;
        this.leavingSensor = leavingSensor;
    }
    public override void OnEnter()
    {
        if (!leavingSensor.IsInRange)
        {
            idleTime = Time.time + settings.playerLeavingIdleTime;
            enemy.wasChasing = false;
        }
        else if (enemy.MaxDistanceReached) idleTime = Time.time + settings.maxDistanceIdleTime;
        else idleTime = Time.time + Random.Range(0, settings.maxIdleTime);
        Debug.Log("Idling");
    }
    public override void OnExit()
    {

    }
    public override void OnLogic()
    {
        if(idleTime <= Time.time)
        {
            enemy.StateMachine.ChangeState(Enemy.State.Walk);
        }
    }
}
public class EnemyWalk : BaseState
{
    RangeSensor ChaseSensor;
    Transform EnemyTransform;
    Vector3 Starting_Position;

    Enemy enemy;

    Enemy.MovementSettings settings;
    float pos;
    public bool isChasing;
    public Transform target_location;
    public EnemyWalk(Enemy enemy, Enemy.MovementSettings movementSettings, RangeSensor chaseSensor)
    {
        ChaseSensor = chaseSensor;
        this.enemy = enemy;
        EnemyTransform = enemy.transform;
        Starting_Position = EnemyTransform.position;
        settings = movementSettings;
        pos = Random.Range(0, 1) * 2 + 1;
    }
    public override void OnEnter()
    {
        if (ChaseSensor.IsInRange)
        {
            target_location = ChaseSensor.Target;
            isChasing = enemy.wasChasing = !enemy.MaxDistanceReached;
            if (enemy.MaxDistanceReached && Mathf.Abs(target_location.position.x - Starting_Position.x) <= settings.patrolfurthestDistance)
            {
                isChasing = true;
                enemy.wasChasing = true;
            }
        }
        if (isChasing) Debug.Log("Chasing");
        else Debug.Log("Patroling");
    }
    public override void OnExit()
    {
        isChasing = false;
    }
    public override void OnLogic()
    {
        GoToDestination();
    }   
    public void GoToDestination()
    {
        Vector3 destination = isChasing ? target_location.position : 
            Starting_Position + settings.patrolOffset * (pos - 2) * Vector3.right;
        if(Mathf.Abs(EnemyTransform.position.x - destination.x) < 0.3f)
        {
            pos = (pos + 2) % 4;
            ChangePos();
        }
        else EnemyTransform.position = new Vector2(Vector2.MoveTowards(EnemyTransform.position, destination, settings.patrolSpeed * Time.deltaTime).x, EnemyTransform.position.y);
        if (Mathf.Abs(EnemyTransform.position.x - Starting_Position.x) > settings.patrolfurthestDistance)
        {
            enemy.MaxDistanceReached = true;
            ChangePos();
        }
    }
    public void ChangePos()
    {
        enemy.StateMachine.ChangeState(Enemy.State.Idle, 0.3f);
    }
}
public class EnemyAttack : BaseState
{
    Enemy enemy;
    Enemy.AttackSettings attackSettings;
    RangeSensor AttackSensor;
    CombatHandler combatHandler;
    Coroutine isAttacking;
    float time;
    public EnemyAttack(Enemy enemy, Enemy.AttackSettings attackSettings, RangeSensor AttackSensor, CombatHandler combat)
    {
        this.enemy = enemy;
        this.attackSettings = attackSettings;
        this.AttackSensor = AttackSensor;
        combatHandler = combat;
        isAttacking = combat.isAttacking;
    }
    public override void OnEnter()
    {
        Debug.Log("Attacking");
    }

    public override void OnExit()
    {
    }

    public override void OnLogic()
    {
        if (AttackSensor.IsInRange)
        {
            if (isAttacking == null && time - Time.time <= 0)
            {
                time = Time.time + Random.Range(0,attackSettings.DMGTimeInterval);
                combatHandler.Attack();
            }
        }
        else
        {
            enemy.StateMachine.ChangeState(Enemy.State.Walk);
        }
    }
}
public class EnemyView : BaseState
{
    Enemy enemy;
    Enemy.ViewSettings viewSettings;
    RangeSensor viewSensor;
    EnemyAlertHandler alertHandler;
    public EnemyView(Enemy enemy, Enemy.ViewSettings viewSettings, RangeSensor viewSensor, EnemyAlertHandler alerter)
    {
        this.enemy = enemy;
        this.viewSettings = viewSettings;
        this.viewSensor = viewSensor;
        alertHandler = alerter;
    }
    public override void OnEnter()
    {
        Debug.Log("Viewing");
        alertHandler.Target = viewSensor.Target;
        alertHandler.isAlert = true;
    }

    public override void OnExit()
    {
        alertHandler.isAlert = false;
    }

    public override void OnLogic()
    {
        if (!viewSensor.IsInRange) enemy.StateMachine.ChangeState(Enemy.State.Idle);
    }
}

