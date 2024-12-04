using System;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public MovementData data { get; set; }
    [SerializeField] private GroundSensor sensor;
    [SerializeField] private Vector3 offset;
    private float ledgeTime, HoldJumpTime, jumpCooldown, dashCooldown, jumpgracePeriod, dashDuration;

    private float gravity, maxFallingSpeed;
    float jumpAttempts;
    bool isFalling => rb.velocity.y < 0;
    private Rigidbody2D rb;
    private Animator animator;
    public bool isJumping;
    public bool isInJumpGracePeriod => jumpgracePeriod > Time.time;
    public bool isDashing { get; private set; }

    Action Logic;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sensor.OnLeavingGround += () =>
        {
            if (!isJumping)
            {
                ledgeTime = Time.time + data.LedgeTime;
            }
        };
        sensor.OnTouchingGround += () =>
        {
            jumpAttempts = data.NumberOfJumps;
        };
        gravity = data.gravity * data.Fall_GravityMultiplier;
        maxFallingSpeed = data.max_Falling_Speed;
    }
    public void Update()
    {
        Logic?.Invoke();
        if (isFalling)
        {
            rb.gravityScale = gravity;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallingSpeed));
        }
        else
        {
            rb.gravityScale = data.gravity;
        }
        if (sensor.IsGrounded && isInJumpGracePeriod)
        {
            Jump_Performed();
        }
    }
    #region Jump
    public void Jump_Canceled()
    {
        if(HoldJumpTime > Time.time) HoldJumpTime = -1;
    }
    public void Jump_Performed()
    {
        jumpgracePeriod = -1;
        bool isGrounded = sensor.IsGrounded;

        if (jumpCooldown > Time.time && jumpAttempts == 0) return;
        if (isGrounded || ledgeTime > Time.time || jumpAttempts > 0)
        {
            isJumping = true;
            HoldJumpTime = Time.time + data.jumpMaxHoldTime;
            ledgeTime = -1;
            jumpAttempts--;

            Logic += Jump_Logic;
            var a = ObjectPoolManager.GetObject(data.jumpParticle.gameObject, true, ObjectPoolManager.PooledInfo.Particle);
            a.transform.position = transform.position + offset;
        }
        else if (!isGrounded)
        {
            jumpgracePeriod = Time.time + data.jump_graceTime;
        }
    }
    public void Jump_Logic()
    {
        if (HoldJumpTime <= Time.time)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
            jumpCooldown = Time.time + data.jumpCooltime;
            Logic -= Jump_Logic;
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, data.jumpPower);
        }
    }
    #endregion
    #region Dash
    public void Dash(Vector2 moveVector)
    {
        Vector2 normalizedDirection;
        float dashCooltime = data.DashCooltime;
        float cd = data.NumberOfDashes * dashCooltime - Mathf.Max(dashCooldown - Time.time, 0);
        int dashAttempts = (int)(cd / dashCooltime);
        Debug.Log(dashAttempts);
        if (dashAttempts != 0 && moveVector.magnitude != 0)
        {
            normalizedDirection = moveVector.normalized;
            rb.velocity = normalizedDirection * data.DashPower;

            dashCooldown += dashCooltime;
            dashDuration = Time.time + data.DashHangTime;
            isDashing = true;
            Logic += DashLogic;
        }
    }
    public void DashLogic()
    {
        if (dashDuration <= Time.time)
        {
            rb.velocity /= data.DashDampeningValue;
            if (rb.velocity.magnitude <= data.DashStopMagnitude)
            {
                Logic -= DashLogic;
                isDashing = false;
            }
        }
    }
    #endregion
    public void Move(float direction)
    {
        float accelX = direction * data.Speed - rb.velocity.x;

        float movement = accelX * data.Acceleration;

        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);

        if (Mathf.Abs(direction) < 0.01) rb.velocity = new Vector2(0, rb.velocity.y);
        else
        {
            if (direction < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
            else transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        animator.SetFloat("Speed", rb.velocity.magnitude / data.Speed);
    }
    #region Others
    public void FastFall_Performed()
    {
        gravity = data.gravity * data.FastFall_GravityMultiplier;
        maxFallingSpeed = data.max_FastFalling_Speed;
    }
    public void FastFall_Canceled()
    {
        gravity = data.gravity * data.Fall_GravityMultiplier;
        maxFallingSpeed = data.max_Falling_Speed;
    }
    #endregion
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + offset, 0.2f);
    }
#endif
}

