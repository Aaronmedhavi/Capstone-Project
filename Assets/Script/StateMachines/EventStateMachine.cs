using System;
using System.Collections.Generic;
using UnityEngine;

public class EventStateMachine<T, T2> where T : Enum where T2 : Enum
{
    private Dictionary<T, BaseState> States;
    private Dictionary<T2, T> TriggerEvent;
    //private Dictionary<T2, List<EventTransition<T>>> TriggerEvent;
    private Dictionary<T, List<EventTransition<T>>> Transitions;
    private List<EventTransition<T>> GlobalTransitions = new();
    public BaseState CurrentState { get; private set; }
    float interval;
    public T State { get; private set; }
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
            CurrentState.OnEnter();
        }
    }
    public void AddStates(List<(T, BaseState)> states)
    {
        States ??= new();
        foreach (var state in states) States.Add(state.Item1, state.Item2);
    }
    public void AddConditions(List<EventTransition<T>> transitions)
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
    public void AddTriggerEvents(List<(T2 eventState, T transition)> transitions)
    {
        TriggerEvent ??= new();
        foreach(var transition in transitions)
        {
            TriggerEvent.Add(transition.eventState, transition.transition);
        }
        //foreach (var transition in transitions)
        //{
        //    if (TriggerEvent.TryGetValue(transition.Item1, out var list))
        //    {
        //        list.OnAdd(transition.Item2);
        //    }
        //    else
        //    {
        //        TriggerEvent.OnAdd(transition.Item1, new()
        //        {
        //            transition.Item2
        //        });
        //    }
        //}
    }
    public void AddGlobalConditions(List<EventTransition<T>> transitions)
    {
        GlobalTransitions.AddRange(transitions);
    }
    public void Trigger(T2 eventName)
    {
        var state = TriggerEvent[eventName];
        if(state != null)
        {
            MoveState(state);
        }
    }
    public void Check()
    {
        if (Time.time < interval) return;
        if (Transitions.ContainsKey(State))
        {
            List<EventTransition<T>> transitions = Transitions[State];
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
        name = State.ToString()/* + CurrentState.GetRootName()*/;
        return name;
    }
}
public class EventTransition<T> where T : Enum
{
    public T from { get; private set; }
    T to;
    Func<bool> condition;
    bool forceReset;
    public float interval;
    public EventTransition(T from, T to, Func<bool> condition = null, bool forceReset = false, float interval = 0f)
    {
        this.from = from;
        this.to = to;
        this.condition = condition;
        this.forceReset = forceReset;
        this.interval = interval;
    }
    public bool Check(out T toState)
    {
        toState = to;
        return condition?.Invoke() == true;
    }
}