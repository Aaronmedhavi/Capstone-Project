using System;
using System.Collections.Generic;
using UnityEngine;
public abstract class BaseState
{
    //protected float startTime;
    //public float Time => UnityEngine.Time.time - startTime;
    public abstract void OnLogic();
    public abstract void OnEnter();
    public abstract void OnExit();
    //public virtual string GetRootName() { return null; }
    //public void Initialize() => startTime = UnityEngine.Time.time;
}
public class Transition<T> where T : Enum
{
    public T toState;
    public Func<bool> condition;

    public Transition(T to, Func<bool> condition)
    {
        toState = to;
        this.condition = condition;
    }
    public bool Check() => condition?.Invoke() == true;
}
public class StateMachine<T> where T : Enum
{
    private Dictionary<T, BaseState> _states;
    private Dictionary<T, List<Transition<T>>> StateTransition;
    private List<Transition<T>> GlobalTransition;
    private T _currentState;
    public BaseState ThisState { get; private set; }

    float Interval;
    public T CurrentState
    {
        get
        {
            return _currentState;
        }
        set
        {
            ThisState = _states[value];
            _currentState = value;
        }
    }

    public void Initialize(T starting_State)
    {
        CurrentState = starting_State;
        ThisState.OnEnter();
    }
    public void AddState(List<(T state, BaseState baseState)> states)
    {
        _states ??= new();
        foreach (var state in states)
        {
            _states.Add(state.state, state.baseState);
        }
    }
    public void AddGlobalTransition(List<Transition<T>> transitions)
    {
        GlobalTransition ??= new();
        foreach (var transition in transitions)
        {
            GlobalTransition.Add(transition);
        }
    }
    public void AddTransition(List<(T from, Transition<T> transition)> transitions)
    {
        StateTransition ??= new();
        foreach (var t in transitions)
        {
            if (!StateTransition.ContainsKey(t.from)) StateTransition.Add(t.from, new());
            StateTransition[t.from].Add(t.transition);
        }
    }
    public void OnLogic()
    {
        Check();
        ThisState.OnLogic();
    }
    public void ChangeState(T new_State, float interval = 0f, bool forceChange = false)
    {
        if (!CurrentState.Equals(new_State) || forceChange)
        {
            ThisState.OnExit();
            CurrentState = new_State;
            ThisState.OnEnter();
            Interval = interval + Time.time;
        }
    }
    public void Check()
    {
        if (Interval >= Time.time) return;
        GlobalTransition ??= new();
        StateTransition ??= new();
        foreach (var transition in GlobalTransition)
        {
            if (transition.Check())
            {
                ChangeState(transition.toState);
                return;
            }
        }
        foreach (var State in StateTransition)
        {
            foreach (var transition in State.Value)
            {
                if (transition.Check())
                {
                    ChangeState(transition.toState);
                    return;
                }
            }
        }
    }

}
