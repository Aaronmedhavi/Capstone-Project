using UnityEngine;
public class Fireball : ProjectileParticle
{
    [Header("Fireball Settings")]
    [SerializeField] private float radius;

    public override void OnEnable()
    {
        base.OnEnable();
        SoundManager.instance.PlaySFX("Red");
    }
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
