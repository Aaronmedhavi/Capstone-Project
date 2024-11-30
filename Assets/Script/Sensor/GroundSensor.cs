using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GroundSensor : MonoBehaviour
{
    [SerializeField] float jumptimeOffset;
    public bool IsGrounded { get; private set; }
    public float LedgeTime { get; private set; }
    private void OnTriggerStay2D(Collider2D collision)
    {
        IsGrounded = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        IsGrounded = false;
        LedgeTime = Time.time + jumptimeOffset;
    }
}

