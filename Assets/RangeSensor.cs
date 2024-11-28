using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class RangeSensor : MonoBehaviour
{
    [SerializeField] float RecognitionRadius;
    [SerializeField] LayerMask layerMask;

    public bool IsInRange { get; private set; }
    public Transform Target { get; private set; }
    public Action OnEnter, OnExit;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        IsInRange = true;
        Target = collision.transform;
        OnEnter?.Invoke();
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        IsInRange = false;
        Target = null;
        OnExit?.Invoke();
    }
#if UNITY_EDITOR
    [Header("Editor Only")]
    [SerializeField] UnityEngine.Color sensorcolor;
    private void OnValidate()
    {
        var col = GetComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.includeLayers = layerMask;
        col.excludeLayers = ~layerMask;
        col.contactCaptureLayers = layerMask;
        col.callbackLayers = layerMask;
        col.radius = RecognitionRadius;
    }
    private void OnDrawGizmos()
    {
        var position = transform.position;
        Gizmos.color = sensorcolor;
        Gizmos.DrawWireSphere(position, RecognitionRadius);
    }
    #endif
} 
