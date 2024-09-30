using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public static class Helpers
{
    public static float Map(float value, float originalMin, float originalMax, float newMin, float newMax, bool useClamp)
    {
        float newValue = (value - originalMin) / (originalMax - originalMin) * (newMax - newMin) + newMin;
        if (useClamp) newValue = Mathf.Clamp(newValue, newMin, newMax);
        return newValue;
    }
}
public abstract class State : MonoBehaviour
{
    public bool isComplete { get; protected set; }
    protected float startTime;
    public float time => Time.time - startTime;

    protected Core core;
    protected Rigidbody2D rb => core.rb;
    protected Animator animator => core.animator;

    public StateMachineYUNIS SM;
    public StateMachineYUNIS parent;
    public State state => SM.state;
    public void SetCore(Core core)
    {
        SM = new();
        this.core = core;
    }
    public void Set(State newState, bool forceReset = false)
    {
        SM.Set(newState, forceReset);
    }
    public abstract void OnLogic();
    public abstract void OnEnter();
    public abstract void OnExit();

    public void LogicBranch()
    {
        OnLogic();
        state?.LogicBranch();
    }
    public void Initialize(StateMachineYUNIS _parent)
    {
        SM = _parent;
        isComplete = false;
        startTime = Time.time;
    }
}
public class StateMachineYUNIS
{
    public State state;
    public void Set(State newState, bool forceReset = false)
    {
        if(state != newState || forceReset)
        {
            state?.OnExit();
            state = newState;
            state.Initialize(this);
            state.OnEnter();
        }
    }
}

public abstract class Core : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

    public StateMachineYUNIS SM;

    public State state => SM.state;
    [Header("Scanner")]
    [SerializeField] Vector2 offset;
    [SerializeField] Vector2 GroundCheckSize;
    [Space(15)]
    [SerializeField] LayerMask layerMask;

    public void SetupInstances()
    {
        SM = new();
        List<State> States = GetComponentsInChildren<State>().ToList();
        foreach(var state in States) state.SetCore(this);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(offset.x, offset.y), GroundCheckSize);
    }
    public bool isGrounded => Physics2D.OverlapBox(transform.position + new Vector3(offset.x, offset.y), GroundCheckSize, 0, layerMask) != null;
}
