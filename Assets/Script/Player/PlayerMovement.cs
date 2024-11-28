using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
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
    [SerializeField] private float dspeed = 20;
    [SerializeField] private float stamina_consumed = 5;
    [SerializeField] private float dash_time;

    [HideInInspector] public Player player;

    float jumpCooldown, HoldJumpCooldown;
    bool isJumping;

    public void Update()
    {
        if (isJumping)
        {
            if (HoldJumpCooldown - Time.time <= 0)
            {
                Rb.velocity = new Vector2(Rb.velocity.x, Rb.velocity.y / 2);
                isJumping = false;
                jumpCooldown = Time.time + jumpCooltime;
            }
            else
            {
                Rb.velocity = new Vector2(Rb.velocity.x, jumpPower);
            }
        }
    }
    public void Jump()
    {
        //make sure the condition is still the same every line
        bool isGrounded = player.IsGrounded;
        if (jumpCooldown > Time.time) return;
        if (isGrounded || player.LedgeTime - Time.time >= 0)
        {
            float jumpPower = this.jumpPower;
            Rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jumpCooldown = Time.time + jumpCooltime;
            jumpParticle.Play();
            player.StateMachine.ChangeState(Player.State.onAir, 0.3f, true);
        }
    }
    public void Jump_Canceled(InputAction.CallbackContext context)
    {
        HoldJumpCooldown = -1;
    }
    public void Jump_Performed(InputAction.CallbackContext context)
    {
        bool isGrounded = player.IsGrounded;
        if (jumpCooldown > Time.time) return;
        if (isGrounded || player.LedgeTime - Time.time >= 0)
        {
            Rb.velocity = new Vector2(Rb.velocity.x, jumpPower);
            isJumping = true;
            HoldJumpCooldown = Time.time + jumpMaxHoldTime;
            jumpParticle.Play();
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
        if (direction > 0.01)
        {
            Animator.SetFloat("Direction", direction);
        }
        Animator.SetFloat("Speed", Rb.velocity.magnitude / Walking_Speed);
    }
}

