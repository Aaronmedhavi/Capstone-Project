//using Unity.VisualScripting;
//using UnityEditor.TerrainTools;
//using UnityEngine;

//public class Navigate : State
//{
//    public Vector2 destination;
//    public float speed = 1;
//    public float threshold = 0.1f;
//    public State RunAnimation;
//    public override void OnEnter()
//    {
//        //running animation state
//        Set(RunAnimation,true);
//    }
//    public override void OnExit()
//    {
//        if(Vector2.Distance(core.transform.position, destination) < threshold)
//        {
//            isComplete = true;
//        }
//        core.transform.localScale = new Vector3(Mathf.Sign(rb.velocity.x), 1, 1);
//    }
//    public override void OnLogic()
//    {
//    }
//}
//public class Patrol : State
//{
//    [Header("State Settings")]
//    public Navigate navigate;
//    public IdleState idle;
//    [Space(25)]
//    public Transform anchor1;
//    public Transform anchor2;

//    public float waitTime;
//    public override void OnEnter()
//    {
//        SetDestination();
//    }

//    public override void OnExit()
//    {

//    }

//    public override void OnLogic()
//    {
//        if (SM.state == navigate && navigate.isComplete)
//        {
//            Set(idle, true);
//            rb.velocity = new Vector2(0, rb.velocity.y);
//        }
//        else
//        {
//            if(SM.state.time > waitTime)
//            {
//                SetDestination();
//            }
//        }
//    }

//    void SetDestination()
//    {
//        navigate.destination = navigate.destination == (Vector2)anchor2.position ? anchor1.position : anchor2.position;
//        Set(navigate, true);
//    }
//}
//public class DistanceDetector : MonoBehaviour
//{
//    public float Radius;
//    public LayerMask layer;
//    public Vector3 targetPosition;
//    //public bool isClose(Vector2 target) => Mathf.Abs(transform.position.x - target.x) <= Radius;
//    public bool isClose()
//    {
//        var coll = Physics2D.OverlapArea(transform.position - Vector3.left * Radius, transform.position + Vector3.left * Radius, layer);
//        targetPosition = coll != null ? coll.transform.position : Vector3.zero;
//        return coll != null;
//    }
//    private void OnDrawGizmos()
//    {
//        Gizmos.DrawLine(transform.position - Vector3.left * Radius, transform.position + Vector3.left * Radius);
//    }
//}
//public class Chase : State
//{
//    public DistanceDetector detector;
//    public IdleState idle;
//    public Navigate navigate;
//    public float waitTime;
//    public override void OnEnter()
//    {
//    }

//    public override void OnExit()
//    {
//    }

//    public override void OnLogic()
//    {
//        if (state == navigate)
//        {
//            if (detector.isClose())
//            {
//                Debug.Log("Found");
//                Set(idle, true);
//                navigate.destination = detector.targetPosition;
//            }
//            //else
//            //{
//            //    navigate.destination = 
//            //    Set(navigate, true);
//            //}
//        }
//        else
//        {
//            if (state.time > waitTime)
//            {
//                isComplete = true;
//            }
//        }
//    }
//}
//public class NPC : Core
//{
//    public Patrol patrol;
//    private void Start()
//    {
//        SetupInstances();
//        Set(patrol);
//    }
//    private void Update()
//    {
//        if (state.isComplete)
//        {

//        }
//        state.LogicBranch();
//    }
//}
