using UnityEngine;
public class Lightning : ProjectileParticle
{
    public override void ParticleLogic(GameObject other)
    {
        Debug.Log("a");
        if(other.TryGetComponent<IEntity>(out var hit))
        {
            hit.OnReceiveDamage(Damage, EnemyInvisDuration);
        }
        Stop();
    }
}
