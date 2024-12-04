﻿using UnityEngine;
using System.Collections.Generic;
public class WallSlam : ProjectileObject
{
    [Header("Lightning Settings (No lifeTime, follows particle Lifetime)")]
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private float Distance;
    [SerializeField] private float timeOverDistance;
    [SerializeField] private float MaxHits;

    RaycastHit2D ray;
    float hit, triggertime;
    bool triggered;

    public override void OnEnable()
    {
        var box = col as BoxCollider2D;
        ray = Physics2D.BoxCast((Vector2)transform.position + box.offset, box.size, 0, transform.right, Distance, ~notInLayer);
        triggered = false;

        triggertime = Time.time + ray.distance * timeOverDistance;
        hit = 0;
    }

    public override void Update()
    {
        if (triggertime <= Time.time && !triggered && ray.collider)
        {
            triggered = true;
            BoxCollider2D box = col as BoxCollider2D;
            Vector3 position = ray.collider.transform.position;
            RaycastHit2D ground = Physics2D.Raycast(position, Vector3.down, box.size.y, groundLayers);
            if (ground.collider != null)
            {
                transform.position = ground.point;
                particle.Play();

                var collisions = Physics2D.OverlapBoxAll(ground.point + box.offset, box.size, 0, ~notInLayer);
                foreach (var collision in collisions)
                {
                    if (collision.TryGetComponent(out IEntity entity) && hit < MaxHits)
                    {
                        entity.OnReceiveDamage(Damage, EnemyInvisDuration);
                        hit++;
                    }
                }
            }
        }
    }
    private void OnParticleSystemStopped()
    {
        Release();
    }
    public override void ProjectileLogic(GameObject other) { }

    public override void SetLayer(LayerMask layer, GameObject obj)
    {
        layer = layer | groundLayers;
        base.SetLayer(layer, obj);
    }
}
public class Bubble : DamagePerParticle
{
    [Header("Bubble Settings")]
    [SerializeField] private float BubbleCount;
    [SerializeField] private float ImmobilizeDuration;

    readonly Dictionary<IEntity, int> entities = new();

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
