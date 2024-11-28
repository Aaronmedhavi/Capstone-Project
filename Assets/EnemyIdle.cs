using System.Collections.Generic;
using UnityEngine;

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
        if (!leavingSensor.IsInRange) idleTime = Time.time + settings.playerLeavingIdleTime;
        else if(enemy.MaxDistanceReached) idleTime = Time.time + settings.maxDistanceIdleTime;
        else idleTime = Time.time + Random.Range(0, settings.maxIdleTime);
    }
    public override void OnExit()
    {

    }
    public override void OnLogic()
    {
    }
}
public class EnemyWalk : BaseState
{
    RangeSensor ChaseSensor;
    Transform target_location, EnemyTransform;
    Vector3 Starting_Position;

    Enemy enemy;

    Enemy.MovementSettings settings;
    float pos;
    bool isChasing;
    public EnemyWalk(Enemy enemy, Enemy.MovementSettings movementSettings, RangeSensor chaseSensor)
    {
        ChaseSensor = chaseSensor;
        this.enemy = enemy;
        EnemyTransform = enemy.transform;
        Starting_Position = EnemyTransform.position;
        settings = movementSettings;
    }
    public override void OnEnter()
    {
        target_location = ChaseSensor.Target;
        isChasing = ChaseSensor.IsInRange && Mathf.Abs(target_location.position.x - Starting_Position.x) > settings.patrolfurthestDistance;
        pos = Random.Range(0,1) * 2 + 1;
    }
    public override void OnExit()
    {

    }
    public override void OnLogic()
    {
        GoToDestination();
    }
    public void GoToDestination()
    {
        Vector3 destination = isChasing ? target_location.position : 
            Starting_Position + settings.patrolOffset * (pos - 2) * Vector3.right;

        if (Vector2.Distance(EnemyTransform.position, destination) < 0.3f)
        {
            pos = (pos + 2) % 3;
            ChangePos();
        }
        else
        {
            EnemyTransform.position = Vector2.MoveTowards(EnemyTransform.position, destination, settings.patrolSpeed * Time.deltaTime);
        }
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
    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }

    public override void OnLogic()
    {
    }
}
public class EnemyView : BaseState
{
    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }

    public override void OnLogic()
    {
    }
}

