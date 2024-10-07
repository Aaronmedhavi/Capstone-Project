using UnityEngine;

public class RunState : BaseState
{
    private readonly Animator LowerBody, UpperBody;
    Rigidbody2D rb2d;
    float MaxSpeed;
    public RunState(Animator LowerBody, Animator UpperBody, Rigidbody2D rb2d, float MaxSpeed)
    {
        this.LowerBody = LowerBody;
        this.UpperBody = UpperBody;
        this.rb2d = rb2d;
    }
    public override void OnEnter()
    {
        LowerBody.Play("Run");
        UpperBody.Play("Run");
    }
    public override void OnLogic()
    {
        float speed = Helpers.Map(Mathf.Abs(rb2d.velocity.x), 0, MaxSpeed, 0, 1, true);
        LowerBody.speed = speed;
        UpperBody.speed = speed;
    }
    public override void OnExit()
    {
        LowerBody.speed = 1;
        UpperBody.speed = 1;
    }
}
