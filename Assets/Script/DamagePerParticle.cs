using UnityEngine;

public class DamagePerParticle : ProjectileParticle
{
    public override void ProjectileLogic(GameObject other)
    {
        if(other.TryGetComponent(out IEntity entity))
        {
            entity.OnReceiveDamage(Damage, EnemyInvisDuration);
        }
    }
}
