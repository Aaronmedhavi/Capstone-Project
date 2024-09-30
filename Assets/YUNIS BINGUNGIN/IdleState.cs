using System;
using UnityEngine;

public class IdleState : BaseState
{
    Animator animator;
    public IdleState(Animator anim)
    {
        animator = anim;
    }
    public override void OnEnter()
    {
        animator.Play("Idle");
    }
    public override void OnExit()
    {
    }
    public override void OnLogic()
    {

    }
}
