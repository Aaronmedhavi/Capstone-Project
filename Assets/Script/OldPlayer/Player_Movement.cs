using UnityEngine;

public enum playerState
{
    idle, run, attack, air
}
public class GroundedState : BaseState
{
    public override void OnEnter()
    {
    }
    public override void OnExit()
    {
    }
    public override void OnLogic()
    {
    }
}
public class AiredState : BaseState
{
    public override void OnEnter()
    {
    }
    public override void OnExit()
    {
    }
    public override void OnLogic()
    {
    }
}
public enum playereventState { grounded, onAir}
public class Player_Movement : MonoBehaviour
{
    [Header("Player Movement Settings")]
    [SerializeField] float Walking_Speed;
    [SerializeField] float Acceleration;
    [SerializeField] float JumpPower;
    [SerializeField] float flying_Acceleration;
    
    public float jumpPower { get { return JumpPower;  } }
    public float speed { get { return Walking_Speed; } }
    Animator upperBody;
    Animator lowerBody;

    GroundSensor groundSensor;
    Rigidbody2D rb2d;
    public void Set(Animator upperBody, Animator lowerBody, GroundSensor groundSensor, Rigidbody2D rb2d)
    {
        this.upperBody = upperBody;
        this.lowerBody = lowerBody;
        this.groundSensor = groundSensor;
        this.rb2d = rb2d;
    }
    public void Move(float magnitude, bool isGrounded)
    {
        float accel = isGrounded ? Acceleration : flying_Acceleration;
        //get the value of force / velocity difference
        float MoveMagnitude = magnitude * Walking_Speed - rb2d.velocity.x;

        float movement = MoveMagnitude * accel;

        rb2d.AddForce(movement * Vector2.right, ForceMode2D.Force);

        if (Mathf.Abs(magnitude) < 0.01) rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        if (Mathf.Abs(magnitude) > 0.01)
        {
            //transform.localScale = new Vector3(Mathf.Sign(magnitude), 1, 1);
            if (magnitude < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
            else transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    public void Jump(bool condition)
    {
        if (isGrounded && condition)
        {
            rb2d.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }
    public void Dash(bool condition)
    {

    }
    bool isGrounded => groundSensor.IsGrounded;
}