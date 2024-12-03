using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GroundSensor : MonoBehaviour
{
    [SerializeField] float jumptimeOffset;
    public bool IsGrounded { get; private set; }
    public Action OnLeavingGround, OnTouchingGround;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTouchingGround?.Invoke();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        IsGrounded = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        IsGrounded = false;
        OnLeavingGround?.Invoke();
    }
    
}

