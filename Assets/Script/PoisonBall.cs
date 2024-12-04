using UnityEngine;

public class PoisonBall : ProjectileParticle
{
    [Header("Poison Settings")]
    [SerializeField] private float radius;
    public override void ProjectileLogic(GameObject other)
    {
        var colliders = Physics2D.OverlapCircleAll(other.transform.position, radius);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out IEntity entity))
            {
                entity.OnReceiveDamage(Damage);
                //give status effect
            }
        }
        Release();
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
