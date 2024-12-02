using System.Collections.Generic;
using UnityEngine;
public class Lightning : ProjectileParticle
{
    [Header("Lightning Settings")]
    [SerializeField] private float chain;
    [SerializeField] private float distance;
    [SerializeField] private GameObject effectObject;

    public List<GameObject> hits = new(); 
    public override void ParticleLogic(GameObject other)
    {
        Debug.Log("a");
        if(other.TryGetComponent<IEntity>(out var hit))
        {
            hit.OnReceiveDamage(Damage, EnemyInvisDuration);
            var obj = ObjectPoolManager.GetObject(effectObject, false, ObjectPoolManager.PooledInfo.Particle);
            obj.transform.position = other.transform.position;
            obj.gameObject.SetActive(true);
        }
        Stop();
    }
}
