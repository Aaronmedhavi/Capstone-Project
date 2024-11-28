using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlatformHandler : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] LayerMask platformLayer;
    [SerializeField] Vector2 DetectorSize;

    Coroutine LGTURUN;
    GameObject player;
    private void Awake()
    {
        player = transform.parent.gameObject;
    }
    public void GoDown() => player.layer = LayerMask.NameToLayer("Falling");
    public void OnTriggerEnter2D(Collider2D collision)
    {
        player.layer = LayerMask.NameToLayer("Player");
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        GoDown();
    }
    //var platform = Physics2D.OverlapBox(transform.position, DetectorSize, 0, platformLayer);
    //if (platform && platform.TryGetComponent<PlatformEffector2D>(out var eff))
    //{
    //    LGTURUN ??= StartCoroutine(startPlatform(eff));
    //}
    //public void TurnPlatform()
    //{
    //    var platform = Physics2D.OverlapBox(transform.position + Vector3.down * DetectorSize.y / 2, DetectorSize, 0, platformLayer);
    //    if (platform && platform.TryGetComponent<PlatformEffector2D>(out var eff))
    //    {
    //        LGTURUN ??= StartCoroutine(startPlatform(eff));
    //    }
    //}
    //IEnumerator startPlatform(PlatformEffector2D eff)
    //{
    //    eff.rotationalOffset = (eff.rotationalOffset + 180) % 360;
    //    yield return new WaitForSeconds(0.5f);
    //    eff.rotationalOffset = (eff.rotationalOffset + 180) % 360;
    //    LGTURUN = null;
    //}
#if UNITY_EDITOR
    [Header("Collider Settings")]
    [SerializeField] Vector2 offset;
    [SerializeField] Vector2 size;

    private void OnValidate()
    {
        var box = GetComponent<BoxCollider2D>();
        box.offset = offset;
        box.size = size;
        box.includeLayers = platformLayer;
        box.excludeLayers = ~platformLayer;
        box.contactCaptureLayers = platformLayer;
        box.callbackLayers = platformLayer;
        box.forceSendLayers = platformLayer;
        box.forceReceiveLayers = platformLayer;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.green;
        Gizmos.DrawWireCube(transform.position + Vector3.down * DetectorSize.y/2, DetectorSize);
    }
    #endif
}
