using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GroundSensor : MonoBehaviour
{
    [SerializeField] Vector2 GroundCheckSize;
    [SerializeField] LayerMask layerMask;

    public Action OnEnter;
    public Action OnExit;
    void OnTriggerEnter2D(Collider2D collision)
    {
        OnEnter?.Invoke();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        OnExit?.Invoke();
    }
    public bool isGrounded => Physics2D.OverlapBox(transform.position, GroundCheckSize, 0, layerMask) != null;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, GroundCheckSize);
    }
}
