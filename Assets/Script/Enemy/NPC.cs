using UnityEngine;

public enum EnemyBehavior
{
    Chase, Walk, Idle, Patrol, Attack
}
public enum EnemyEventState
{
    OnRecognized, InRange
}
//public class NPCIdle : BaseState
//{
    
//}
public class NPC : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] float ChasingRadius;
    [SerializeField] float ChasingDistance;
    [SerializeField] float RecognitionRadius;
    [SerializeField] float AttackingRadius;

    //Sensor
    EventStateMachine<EnemyBehavior, EnemyEventState> SM = new();
    private void Awake()
    {
        SM.AddStates(new()
        {
            //(EnemyBehavior.Idle, new IdleState(null, null)),
            //(EnemyBehavior.Chase, new RunState(null, null, null, 0)),
            //(EnemyBehavior.Attack, new IdleState(null, null)),
            //(EnemyBehavior.Walk, new IdleState(null, null)),
            //(EnemyBehavior.Patrol, new IdleState(null, null)),
        });
        SM.AddTriggerEvents(new()
        {
            (EnemyEventState.InRange, EnemyBehavior.Attack),
            (EnemyEventState.OnRecognized, EnemyBehavior.Chase)
        });
    }

    private void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        Vector3 position = transform.position;
        //Draw Chasing Area
        Gizmos.color = UnityEngine.Color.cyan;
        Gizmos.DrawWireSphere(position, ChasingRadius);

        //Draw Recognition Area
        Gizmos.color = UnityEngine.Color.yellow; 
        Gizmos.DrawWireSphere(position, RecognitionRadius);

        //Draw Chasing range
        Gizmos.color = UnityEngine.Color.green;
        Gizmos.DrawLine(position - Vector3.right * ChasingDistance, position + Vector3.right * ChasingDistance);

        //Draw Attacking Area
        Gizmos.color= UnityEngine.Color.red;
        Gizmos.DrawWireSphere(position, AttackingRadius);
        #endif
    }
}
