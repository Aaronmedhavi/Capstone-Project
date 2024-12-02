using System.Collections.Generic;
using UnityEngine;

public class Fist : ProjectileParticle
{
    [SerializeField] private LayerMask groundLayers;
    public override void ParticleLogic(GameObject other) { }
    public override void SetLayer(LayerMask layer, GameObject obj)
    {
        thisObject = obj;
        this.layer = layer | groundLayers;
        var collider = particle.collision;
        collider.collidesWith = ~layer;
        collider.sendCollisionMessages = true;
        var main = particle.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }
    List<ParticleSystem.Particle> b = new();
    private void OnParticleTrigger()
    {
        Debug.Log("HALOOOOOOOOOOOO");
        int count = particle.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, b, out var data);
        for (int i = 0; i < count; i++)
        {
            int collidercount = data.GetColliderCount(i);
            for(int j = 0; j < collidercount; j++)
            {
                var component = data.GetCollider(i, j);
                if(component.gameObject.TryGetComponent(out IEntity entity))
                {
                    Debug.Log("HALO");
                    entity.OnReceiveDamage(Damage,EnemyInvisDuration);
                }
            }
        }
    }
}
