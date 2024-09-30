using UnityEngine;

public class AirState : BaseState
{
    Rigidbody2D rb; Animator animator;
    float jumpPower;
    public AirState(Rigidbody2D rb, Animator animator, float jumpPower)
    {
        this.rb = rb;
        this.animator = animator;
        this.jumpPower = jumpPower;
    }
    public override void OnEnter()
    {

    }
    public override void OnLogic()
    {
        //map the animator speed (KONSEP : kita convert nilai dia diclamp aj harusnya)
        //Map -> minta value, ubah jadi (0,1) dlm bentuk animasi
        float time = Helpers.Map(rb.velocity.y, jumpPower, -jumpPower, 0, 1, true);
        animator.Play("Jump", 0, time);
        animator.speed = 0;
    }
    public override void OnExit()
    {

    }
}
