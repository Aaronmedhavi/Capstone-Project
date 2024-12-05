using System.Collections.Generic;
using UnityEngine;

public class Fist : ProjectileObject
{
    [Header("Fist Settings (No lifeTime, follows particle Lifetime)")]
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private float colActivateTime;
    [SerializeField] private float MaxHits;
    [SerializeField] private float TotalHitsPerEnemy = 1;

    float colSpawnTime, hit;
    public override void OnEnable()
    {
        particle.Play();
        colSpawnTime = Time.time + colActivateTime;
        hit = 0;
        SoundManager.instance.PlaySFX("Orange");
    }
    public override void Update()
    {
        if (colSpawnTime <= Time.time)
        {
            BoxCollider2D box = col as BoxCollider2D;
            var collisions = Physics2D.OverlapBoxAll((Vector2)transform.position + box.offset, box.size, 0, ~notInLayer);
            foreach (var collision in collisions)
            {
                if (collision.TryGetComponent(out IEntity entity) && hit < MaxHits)
                {
                    for (int i = 0; i < TotalHitsPerEnemy; i++)
                    {
                        entity.OnReceiveDamage(Damage, EnemyInvisDuration);
                    }
                    hit++;
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

//public class Fist : ProjectileParticle
//{
//    [SerializeField] private LayerMask groundLayers;
//    public override void ProjectileLogic(GameObject other) { }
//    public override void SetLayer(LayerMask notInLayer, GameObject obj)
//    {
//        thisObject = obj;
//        this.notInLayer = notInLayer | groundLayers;
//        var collider = particle.collision;
//        collider.collidesWith = ~notInLayer;
//        collider.sendCollisionMessages = true;
//        var main = particle.main;
//        main.stopAction = ParticleSystemStopAction.Callback;
//    }
//    List<ParticleSystem.Particle> b = new();
//    private void OnParticleTrigger()
//    {
//        Debug.Log("HALOOOOOOOOOOOO");
//        int count = particle.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, b, out var data);
//        for (int i = 0; i < count; i++)
//        {
//            int collidercount = data.GetColliderCount(i);
//            for(int j = 0; j < collidercount; j++)
//            {
//                var component = data.GetCollider(i, j);
//                if(component.gameObject.TryGetComponent(out IEntity entity))
//                {
//                    Debug.Log("HALO");
//                    entity.OnReceiveDamage(Damage,EnemyInvisDuration);
//                }
//            }
//        }
//    }
//}
