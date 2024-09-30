using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Player Movement Settings")]
    [SerializeField] private float Walking_Speed;
    [SerializeField] float Acceleration;
    [SerializeField] float jumpPower;

    [Header("Dash_Settings")]
    [SerializeField] private float dspeed = 20;
    [SerializeField] private float stamina_consumed = 5;
    [SerializeField] private float dash_time;

    [Header("Scanner")]
    [SerializeField] Transform GroundCheck;
    [SerializeField] Vector2 GroundCheckSize;

    [Space(30)] 
    [SerializeField] LayerMask layerMask;

    bool isFacingRight;
    float input;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }   
    void Update()
    {
        input = Input.GetAxis("Horizontal");
        if(Input.GetKeyDown(KeyCode.Space))Jump();
        Move(input);
    }
    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }
    public void Move(float direction)
    {
        //get the value of force / velocity difference
        float accelX = direction * Walking_Speed - rb.velocity.x;

        float movement = accelX * Acceleration;

        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);

        //AddFriction();
        if (Mathf.Abs(direction) < 0.01) rb.velocity = new Vector2(0, rb.velocity.y);
        if (direction > 0.01)
        {
            animator.SetFloat("Direction", direction);
        }
        //kalo lebih dari 1 lari.
        animator.SetFloat("Speed", rb.velocity.magnitude);
    }
    private void Turn()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        isFacingRight = !isFacingRight;
    }
    private void OnGUI()
    {
        GUILayout.TextField($"Velocity : {rb.velocity.x}\nInput : {input}", GUILayout.MinHeight(1), GUILayout.MaxHeight(100), GUILayout.MinWidth(1), GUILayout.MaxWidth(100));
    }
    //public void dash(Vector2 direction)
    //{
    //    direction = direction.normalized;
    //    rb.AddForce(direction.x * dspeed * Vector2.right, ForceMode.Impulse);
    //    rb.AddForce(direction.y * dspeed * Vector2.up, ForceMode.Impulse);
    //}
    bool isGrounded => Physics2D.OverlapBox(GroundCheck.position, GroundCheckSize, 0, layerMask) != null;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(GroundCheck.position, GroundCheckSize);
    }
}

