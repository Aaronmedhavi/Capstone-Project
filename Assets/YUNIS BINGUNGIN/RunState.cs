using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RunState : BaseState
{
    Player_Movement movement;
    Rigidbody2D rb;
    Animator animator;
    float speed, accel;
    public RunState(Player_Movement movement, float speed, float accel, Rigidbody2D rb, Animator animator)
    {
        this.movement = movement;
        this.rb = rb;
        this.animator = animator;
        this.speed = speed;
        this.accel = accel;
    }
    public override void OnEnter()
    {
    }
    public override void OnLogic()
    {
        float direction = movement.xInput;
        //get the value of force / velocity difference
        float accelX = direction * speed - rb.velocity.x;
        float movements = accelX * accel;

        rb.AddForce(movements * Vector2.right, ForceMode2D.Force);

        rb.velocity = new Vector2(0, rb.velocity.y);
        animator.SetFloat("Direction", direction);
    }
    public override void OnExit()
    {

    }
}
