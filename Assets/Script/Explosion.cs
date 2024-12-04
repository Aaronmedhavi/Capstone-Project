using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Explosion : ProjectileObject
{
    [SerializeField] private float distance;
    [SerializeField] private Vector2 ScanSize;
    [SerializeField] private Animator animator;
    [SerializeField] private string AnimationName;

    public override void OnEnable()
    {
        base.OnEnable();
        var hits = Physics2D.BoxCastAll(transform.position, ScanSize, 0, transform.right, distance, ~notInLayer).ToList();
        Collider2D col = null;

        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.TryGetComponent(out IEntity entity))
            {
                col = hit.collider;
                break;
            }
        }
        if (col) { 
            Explode(col.transform.position);
        }
        else Explode(transform.position + transform.right * 1 / 2 * distance);
    }
    public void Explode(Vector3 post)
    {
        var boxcollider = col as BoxCollider2D;
        transform.position = post;
        animator.Play(AnimationName);
        var colliders = Physics2D.OverlapBoxAll(post, boxcollider.size, 0, ~notInLayer);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out IEntity entity))
            {
                entity.OnReceiveDamage(Damage);
            }
        }
        StartCoroutine(WaitExplosion());
    }
    public IEnumerator WaitExplosion()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
        Release();
    }
    public override void ProjectileLogic(GameObject other){}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var box = col as BoxCollider2D;
        Gizmos.DrawWireCube(transform.position, box.size);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, ScanSize);
    }
}
