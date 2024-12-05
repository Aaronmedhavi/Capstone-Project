using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlatformHandler : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] LayerMask platformLayer;
    [SerializeField] string LayerName;

    Coroutine LGTURUN;
    GameObject player;
    Transform last_parent;
    private void Awake()
    {
        player = transform.parent.gameObject;
    }
    public void GoDown() => player.layer = LayerMask.NameToLayer("Falling");
    public void OnTriggerEnter2D(Collider2D collision)
    {
        player.layer = LayerMask.NameToLayer(LayerName);
        last_parent = player.transform.parent;
        player.transform.SetParent(collision.transform, true);
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.activeInHierarchy)
        {
            // GoDown();
            player.transform.SetParent(last_parent, true);
        }
    }
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
    #endif
}
