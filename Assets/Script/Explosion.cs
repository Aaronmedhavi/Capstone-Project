using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Explosion : ProjectileObject
{
    [SerializeField] private float distance;
    [SerializeField] private Vector2 ScanSize;
    [SerializeField] private Vector2 size;
    public override void OnEnable()
    {
        var hits = Physics2D.BoxCastAll(transform.position, ScanSize, 0,transform.right, distance, ~notInLayer).ToList();
        Collider2D col = hits.First(x => x.collider.TryGetComponent(out IEntity entity)).collider;
        if (col != null) Explode(col.transform.position);
        else Explode(transform.position + transform.right * 1/2 * distance);
    }
    public void Explode(Vector3 post)
    {
        var colliders = Physics2D.OverlapBoxAll(post, size, 0, ~notInLayer);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out IEntity entity))
            {
                entity.OnReceiveDamage(Damage);
            }
        }
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
