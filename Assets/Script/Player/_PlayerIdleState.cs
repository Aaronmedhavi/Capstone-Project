using UnityEngine;

public class _PlayerIdleState : BaseState
{
    Animator animator;
    Rigidbody2D rb;

    public _PlayerIdleState(Animator animator, Rigidbody2D rb)
    {
        this.animator = animator;
        this.rb = rb;
    }

    public override void OnEnter()
    {
        Debug.Log("IDLE");
    }

    public override void OnExit()
    {
    }

    public override void OnLogic()
    {
    }
}
public class _PlayerOnAirState : BaseState
{
    Animator animator;
    Rigidbody2D rb;
    float jumpPower;
    public _PlayerOnAirState(Animator animator, Rigidbody2D rb, float jumpPower)
    {
        this.animator = animator;
        this.rb = rb;
        this.jumpPower = jumpPower;
    }
    public override void OnEnter()
    {
        Debug.Log("AIR");
        animator.SetBool("Jumping", true);
    }
    public override void OnExit()
    {
        animator.speed = 1;
        animator.SetBool("Jumping", false);
    }
    public override void OnLogic()
    {
        float time = Helpers.Map(rb.velocity.y, jumpPower, -jumpPower, 0, 1, true);
        animator.Play("Jump", 0, time);
        animator.speed = 0;
    }
}
public class _PlayerOnHitState : BaseState
{
    Animator animator;
    Rigidbody2D rb;
    GameObject player;
    public _PlayerOnHitState(Animator animator, Rigidbody2D rb, GameObject player)
    {
        this.animator = animator;
        this.rb = rb;
        this.player = player;
    }

    public override void OnEnter()
    {
        Debug.Log("ONHIT");
        animator.SetBool("OnHit", true);
        player.layer = LayerMask.NameToLayer("Default");
    }

    public override void OnExit()
    {
        animator.SetBool("OnHit", true);
        player.layer = LayerMask.NameToLayer("Entity");
    }
    public override void OnLogic()
    {
    }
}