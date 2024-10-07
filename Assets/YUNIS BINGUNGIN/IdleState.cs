using System;
using UnityEngine;

public class IdleState : BaseState
{
    private readonly Animator LowerBody, UpperBody;
    public IdleState(Animator LowerBody, Animator UpperBody)
    {
        this.LowerBody = LowerBody;
        this.UpperBody = UpperBody;
    }
    public override void OnEnter()
    {
        LowerBody.Play("Idle");
        UpperBody.Play("Idle");
    }
    public override void OnExit()
    {
    }
    public override void OnLogic()
    {

    }
}
