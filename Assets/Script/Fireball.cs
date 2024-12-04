using System.Linq;
using UnityEngine;

public class Explosion : ProjectileObject
{
    [SerializeField] private float distance;
    [SerializeField] private Vector2 size;
    public override void OnEnable()
    {
        var hits = Physics2D.RaycastAll(transform.position, transform.right, distance, ~notInLayer).ToList();
        IEntity entity = null;
        Collider2D col = null;
        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out entity))
            {
                col = hit.collider;
                break;
            }
        }
        if (entity != null)
        {
            Explode(col.transform.position);
        }
        else
        {
            Explode(transform.position + transform.right * distance);
        }
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
}
public class Fireball : ProjectileObject
{
    [Header("Fireball Settings")]
    [SerializeField] private float radius;
    public override void ProjectileLogic(GameObject other)
    {
        var colliders = Physics2D.OverlapCircleAll(other.transform.position, radius);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out IEntity entity))
            {
                entity.OnReceiveDamage(Damage);
            }
        }
        Release();
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}
