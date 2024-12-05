using System.Collections.Generic;
using UnityEngine;

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
    public override void OnEnable()
    {
        base.OnEnable();
        SoundManager.instance.PlaySFX("Yellow");
    }
    public override void ProjectileLogic(GameObject other)
    {
        if (other.TryGetComponent<IEntity>(out var hit) && !hits.Contains(other.transform))
        {
            hit.OnReceiveDamage(Damage, EnemyInvisDuration);
            var effect = ObjectPoolManager.GetObject(effectObject, false, ObjectPoolManager.PooledInfo.Particle);
            effect.transform.position = other.transform.position;
            effect.SetActive(true);
            if (chain > 0 && hit.IsAlive)
            {
                var ray = Physics2D.Raycast(other.transform.position + transform.right * 1, transform.right, distance, ~notInLayer);
                if (ray && ray.collider.TryGetComponent<IEntity>(out var enty) && enty.IsAlive)
                {
                    Debug.Log("chain");
                    hits.Add(other.transform);
                    var chainEffect = ObjectPoolManager.GetObject(chainProjectile, false, ObjectPoolManager.PooledInfo.GameObject);
                    chainEffect.transform.SetPositionAndRotation(other.transform.position + transform.right * 2,transform.rotation);
                    var lightning = chainEffect.GetComponent<Lightning>();
                    lightning.SetValue(chain - 1, Damage * dmgMultiplierEachChain, hits);
                    lightning.SetLayer(notInLayer, thisObject);

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

