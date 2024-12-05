using UnityEngine;
using System.Collections.Generic;
public class Bubble : DamagePerParticle
{
    [Header("Bubble Settings")]
    [SerializeField] private float BubbleCount;
    [SerializeField] private float ImmobilizeDuration;

    readonly Dictionary<IEntity, int> entities = new();

    public override void OnEnable()
    {
        base.OnEnable();
        SoundManager.instance.PlaySFX("Blue");
    }
    public override void SetLayer(LayerMask layer, GameObject obj)
    {
        base.SetLayer(layer, obj);
        entities.Clear();
    }
    public override void ProjectileLogic(GameObject other)
    {
        if (other.TryGetComponent(out IEntity entity))
        {
            entity.OnReceiveDamage(Damage, EnemyInvisDuration);
            if (entities.ContainsKey(entity)) entities[entity]++;
            else entities.Add(entity, 1);

            if (entities[entity] == BubbleCount)
            {
                //APPLY IMMOBILIZES
            }
        }
    }
}
