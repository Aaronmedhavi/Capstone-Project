using UnityEngine;

public class AirState : BaseState
{
    Rigidbody2D rb2d; Animator lowerBody, upperBody;
    float jumpPower;
    public AirState(Animator lowerBody, Animator upperBody, Rigidbody2D rb2d, float jumpPower)
    {
        this.rb2d = rb2d;
        this.lowerBody = lowerBody;
        this.upperBody = upperBody;
        this.jumpPower = jumpPower;
    }
    public override void OnEnter()
    {

    }
    public override void OnLogic()
    {
        //map the animator speed (KONSEP : kita convert nilai dia diclamp aj harusnya)
        //Map -> minta value, ubah jadi (0,1) dlm bentuk animasi
        float time = Helpers.Map(rb2d.velocity.y, jumpPower, -jumpPower, 0, 1, true);
        lowerBody.Play("Jump", 0, time);
        upperBody.Play("Jump", 0, time);
        lowerBody.speed = 0;
        upperBody.speed = 0;
    }
    public override void OnExit()
    {
        lowerBody.speed = 1;
        upperBody.speed = 1;
    }
}
