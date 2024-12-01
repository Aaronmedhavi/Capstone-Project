using UnityEngine;
public class Lightning : ProjectileParticle
{
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("a");
        other.GetComponent<IEntity>().OnReceiveDamage(Damage,EnemyInvisDuration);
        particle.Stop();
    }
}
