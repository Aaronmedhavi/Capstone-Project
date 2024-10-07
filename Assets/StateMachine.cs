using System;
using System.Collections.Generic;
public abstract class BaseState
{
    protected float startTime;
    public float Time => UnityEngine.Time.time - startTime;
    public virtual void Awake() { }
    public abstract void OnLogic();
    public abstract void OnEnter();
    public abstract void OnExit();
    public virtual string GetRootName() { return null; }
    public void Initialize() => startTime = UnityEngine.Time.time;
}
public class StateMachine<T, T2> where T : Enum where T2 : Enum
{
    private Dictionary<T, BaseState> States;
    private Dictionary<T2, List<Transition<T>>> TriggerEvent;
    private Dictionary<T, List<Transition<T>>> Transitions;
    private List<Transition<T>> GlobalTransitions = new();
    public BaseState CurrentState { get; private set; }
    public T State { get; private set; }
    public void Init()
    {
        foreach (var state in States.Values) state.Awake();
    }
    public void OnLogic()
    {
        Check();
        CurrentState.OnLogic();
    }
    public void OnEnter() => CurrentState.OnEnter();
    public void OnExit() => CurrentState.OnExit();
    public void SetState(T state)
    {
        CurrentState = States[state];
        State = state;
    }
    public void MoveState(T to, bool forceReset = false)
    {
        if (!State.Equals(to) || forceReset)
        {
            CurrentState?.OnExit();
            CurrentState = States[to];
            State = to;
            CurrentState.Initialize();
            CurrentState.OnEnter();
        }
    }
    public void AddStates(List<(T, BaseState)> states)
    {
        States ??= new();
        foreach (var state in states) States.Add(state.Item1, state.Item2);
    }
    public void AddConditions(List<Transition<T>> transitions)
    {
        Transitions ??= new();
        foreach(var transition in transitions)
        {
            if (Transitions.TryGetValue(transition.from, out var list))
            {
                list.Add(transition);
            }
            else
            {
                Transitions.Add(transition.from, new()
                {
                    transition
                });
            }
        }
    }
    public void AddTriggerEvents(List<(T2,Transition<T>)> transitions)
    {
        TriggerEvent ??= new();
        foreach (var transition in transitions)
        {
            if (TriggerEvent.TryGetValue(transition.Item1, out var list))
            {
                list.Add(transition.Item2);
            }
            else
            {
                TriggerEvent.Add(transition.Item1, new()
                {
                    transition.Item2
                });
            }
        }
    }
    public void AddGlobalConditions(List<Transition<T>> transitions)
    {
        GlobalTransitions.AddRange(transitions);
    }
    public void Trigger(T2 eventName)
    {
        var state = TriggerEvent[eventName].Find((x) => x.from.Equals(State));
        if(state != null)
        {
            state.Check(out T to);
            MoveState(to);
        }
    }
    public void Check()
    {
        if (Transitions.ContainsKey(State))
        {
            List<Transition<T>> transitions = Transitions[State];
            foreach (var transition in transitions)
            {
                if (transition.Check(out var toState))
                {
                    MoveState(toState);
                }
            }
        }
        foreach(var transition in GlobalTransitions)
        {
            if(!transition.Check(out var toState))
            {
                MoveState(toState);
            }
        }
    }
    public string GetStateInfo()
    {
        string name;
        name = State.ToString() + CurrentState.GetRootName();
        return name;
    }
}
public class Transition<T> where T : Enum
{
    public T from { get; private set; }
    T to;
    Func<bool> condition;
    bool forceReset;

    public Transition(T from, T to, Func<bool> condition = null, bool forceReset = false)
    {
        this.from = from;
        this.to = to;
        this.condition = condition;
        this.forceReset = forceReset;
    }
    public bool Check(out T toState)
    {
        toState = to;
        return condition?.Invoke() == true;
    }
}