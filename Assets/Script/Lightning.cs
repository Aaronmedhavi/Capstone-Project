using System.Collections.Generic;
using UnityEngine;
public class ThunderStrike : ProjectileParticle {
    public override void ParticleLogic(GameObject other) { }

    private void OnParticleTrigger()
    {
        
    }
}

public class Lightning : ProjectileParticle
{
    [Header("Lightning Settings")]
    [SerializeField] private float chain;
    [SerializeField] private float distance;
    [SerializeField] private float dmgMultiplierEachChain;
    [SerializeField] private GameObject chainProjectile;

    [Header("Visuals")]
    [SerializeField] private GameObject effectObject;

    public List<Transform> hits = new(); 
    public override void ParticleLogic(GameObject other)
    {
        if (other.TryGetComponent<IEntity>(out var hit) && !hits.Contains(other.transform))
        {
            hit.OnReceiveDamage(Damage, EnemyInvisDuration);
            var effect = ObjectPoolManager.GetObject(effectObject, false, ObjectPoolManager.PooledInfo.Particle);
            effect.transform.position = other.transform.position;
            effect.SetActive(true);
            if (chain > 0 && hit.isAlive)
            {
                var ray = Physics2D.Raycast(other.transform.position + transform.right * 2, transform.right, distance, ~layer);
                if (ray && ray.collider.TryGetComponent<IEntity>(out var enty) && enty.isAlive)
                {
                    Debug.Log("chain");
                    hits.Add(other.transform);
                    var chainEffect = ObjectPoolManager.GetObject(chainProjectile, false, ObjectPoolManager.PooledInfo.GameObject);
                    chainEffect.transform.SetPositionAndRotation(other.transform.position + transform.right * 2,transform.rotation);
                    var lightning = chainEffect.GetComponent<Lightning>();
                    lightning.SetValue(chain - 1, Damage * dmgMultiplierEachChain, hits);
                    lightning.SetLayer(layer, thisObject);

                    chainEffect.SetActive(true);
                }
            }
            hits.Clear();
            Stop();
        }
        else if (hit == null) Stop();
    }
    public void SetValue(float chain, float damage, List<Transform> hit)
    {
        this.chain = chain;
        Damage = damage;
        hits = new(hit);
    }
}

