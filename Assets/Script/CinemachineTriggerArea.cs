using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CinemachineTriggerArea : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;
    private Vector3 originalOffset;

    [Header("Offset Settings")]
    public Vector3 direction;
    public float magnitude;
    public float smoothTransitionTime = 1.0f;

    private Coroutine changeOffsetCoroutine;
    private Coroutine resetOffsetCoroutine;

    void Start()
    { 
        var framingTransposer = vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (framingTransposer != null)
        {
            Debug.Log("ada framing transposernya... harusnya");
            originalOffset = framingTransposer.m_TrackedObjectOffset;
            Debug.Log(originalOffset.x);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (changeOffsetCoroutine != null)
            {
                StopCoroutine(changeOffsetCoroutine);
            }
            changeOffsetCoroutine = StartCoroutine(ChangeOffset());
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (resetOffsetCoroutine != null)
            {
                StopCoroutine(resetOffsetCoroutine);
            }
            resetOffsetCoroutine = StartCoroutine(ResetOffset());
        }
    }
    
    private IEnumerator ChangeOffset()
    {
        var framingTransposer = vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (framingTransposer != null)
        {
            Vector3 targetOffset = originalOffset + direction.normalized * magnitude;
            Vector3 startOffset = framingTransposer.m_TrackedObjectOffset;
            float elapsedTime = 0; while (elapsedTime < smoothTransitionTime)
            {
                framingTransposer.m_TrackedObjectOffset = Vector3.Lerp(startOffset, targetOffset, elapsedTime / smoothTransitionTime);
                elapsedTime += Time.deltaTime; yield return null;
            }
            framingTransposer.m_TrackedObjectOffset = targetOffset;
        }
    }
    
    private IEnumerator ResetOffset()
    {
        var framingTransposer = vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (framingTransposer != null) 
       {
            Vector3 currentOffset = framingTransposer.m_TrackedObjectOffset;float elapsedTime = 0;
            while (elapsedTime < smoothTransitionTime)
            {
                framingTransposer.m_TrackedObjectOffset = Vector3.Lerp(currentOffset, originalOffset, elapsedTime / smoothTransitionTime);
                elapsedTime += Time.deltaTime; yield return null;
            }
            framingTransposer.m_TrackedObjectOffset = originalOffset;
        }
    }
}