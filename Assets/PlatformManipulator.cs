using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManipulator : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] LayerMask platformLayer;
    [SerializeField] Transform platformDetector;
    [SerializeField] Vector2 DetectorSize;

    Coroutine LGTURUN;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            var platform = Physics2D.OverlapBox(platformDetector.position, DetectorSize, 0, platformLayer);
            if (platform && platform.TryGetComponent<PlatformEffector2D>(out var eff))
            {
                LGTURUN ??= StartCoroutine(startPlatform(eff));
            }
        }
    }
    IEnumerator startPlatform(PlatformEffector2D eff)
    {
        eff.rotationalOffset = (eff.rotationalOffset + 180) % 360;
        yield return new WaitForSeconds(0.5f);
        eff.rotationalOffset = (eff.rotationalOffset + 180) % 360;
        LGTURUN = null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(platformDetector.position, DetectorSize);
    }
}
