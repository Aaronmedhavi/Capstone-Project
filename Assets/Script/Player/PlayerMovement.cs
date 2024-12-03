using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class MovementData
{
    [Header("Movement Settings")]
    public float Speed;
    public float Acceleration;

    [Header("Jump Settings")]
    public float NumberOfJumps = 1;

    public float jump_graceTime;
    public float jumpPower;
    public float jumpCooltime;
    public float jumpMaxHoldTime;
    public ParticleSystem jumpParticle;

    [Header("Dash_Settings")]
    public float NumberOfDashes = 1;

    public float DashPower = 20;
    public float DashCooltime;
    public float DashHangTime = 0.3f;
    public float DashStopMagnitude = 0.5f;
    public float DashDampeningValue = 1.15f;

    [Header("Other Settings")]
    public float LedgeTime;

    public float gravity;
    //public float airTime_Threshold;
    public float Fall_GravityMultiplier;
    public float max_Falling_Speed;

    public float FastFall_GravityMultiplier;
    public float max_FastFalling_Speed;
}

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D Rb => player.rb2d;
    private Animator Animator => player.animator;
    public float speed => Walking_Speed;
    public float jump => jumpPower;

    [Header("Horizontal Movement Settings")]
    [SerializeField] private float Walking_Speed;
    [SerializeField] float Acceleration;

    [Header("Jump Settings")]
    [SerializeField] float jumpPower;
    [Tooltip("it is adviced to have this value over the ledgetime")]
    [SerializeField] float jumpCooltime;
    [SerializeField] float jumpMaxHoldTime;
    [SerializeField] ParticleSystem jumpParticle;

    [Header("Dash_Settings")]
    [SerializeField] private float DashSpeed = 20;
    [SerializeField] private float DashCooldown;
    [SerializeField] private float Dashtime = 0.3f;
    [SerializeField] private float DashStopMagnitude = 0.5f;
    [SerializeField] private float DashDampeningValue = 1.15f;

    [HideInInspector] public Player player;

    Vector2 normalizedDirection;
    float jumpCooldown, HoldJumpDuration, dashCooldown, dashDuration;
    bool isOnCooldown(float time) => time > Time.time;
    public bool isDashing { get; private set; }

    Action Logic;

    public void Update()
    {
        Logic?.Invoke();
    }
    //public void Jump()
    //{
    //    //make sure the condition is still the same every line
    //    bool isGrounded = player.IsGrounded;
    //    if (jumpCooldown > Time.time) return;
    //    if (isGrounded || player.LedgeTime - Time.time >= 0)
    //    {
    //        float jumpPower = this.jumpPower;
    //        Rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    //        jumpCooldown = Time.time + jumpCooltime;
    //        jumpParticle.Play();
    //        player.StateMachine.ChangeState(Player.State.onAir, 0.3f, true);
    //    }
    //}
    public void Jump_Canceled(InputAction.CallbackContext context)
    {
        HoldJumpDuration = -1;
    }
    public void Jump_Performed(InputAction.CallbackContext context)
    {
        bool isGrounded = player.IsGrounded;
        if (isOnCooldown(jumpCooldown)) return;
        if (isGrounded)
        {
            Rb.velocity = new Vector2(Rb.velocity.x, jumpPower);
            //isJumping = true;
            HoldJumpDuration = Time.time + jumpMaxHoldTime;
            Logic += Jump_Logic;
            jumpParticle.Play();
        }
    }
    public void Jump_Logic()
    {
        if (HoldJumpDuration <= Time.time)
        {
            Rb.velocity = new Vector2(Rb.velocity.x, Rb.velocity.y / 2);
            jumpCooldown = Time.time + jumpCooltime;
            Logic -= Jump_Logic;
        }
        else
        {
            Rb.velocity = new Vector2(Rb.velocity.x, jumpPower);
        }
    }
    public void Dash(Vector2 moveVector)
    {
        Debug.Log(moveVector);
        if (!isOnCooldown(dashCooldown) && moveVector.magnitude != 0)
        {
            normalizedDirection = moveVector.normalized;
            Rb.velocity = normalizedDirection * DashSpeed;

            dashCooldown = Time.time + DashCooldown;
            dashDuration = Time.time + Dashtime;
            isDashing = true;
            Logic += DashLogic;
        }
        //Vector2 normalizedDirection = moveVector.normalized;
        //Rb.AddForce(normalizedDirection * DashSpeed, ForceMode2D.Force);
    }
    public void DashLogic()
    {
        if (dashDuration <= Time.time)
        {
            Rb.velocity /= DashDampeningValue;
            if(Rb.velocity.magnitude <= DashStopMagnitude)
            {
                Logic -= DashLogic;
                isDashing = false;
            }
        }
    }
    public void Move(float direction)
    {
        float accelX = direction * Walking_Speed - Rb.velocity.x;

        float movement = accelX * Acceleration;

        Rb.AddForce(movement * Vector2.right, ForceMode2D.Force);

        if (Mathf.Abs(direction) < 0.01) Rb.velocity = new Vector2(0, Rb.velocity.y);
        else
        {
            if (direction < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
            else transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        //transform.localScale = new Vector3(Mathf.Sign(direction), 1, 1);
        Animator.SetFloat("Speed", Rb.velocity.magnitude / Walking_Speed);
    }
}

