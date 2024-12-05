using UnityEngine;

public class PoisonSpray : ProjectileObject
{
    [Header("Fist Settings (No lifeTime, follows particle Lifetime)")]
    [SerializeField] private float colActivateTime;
    [SerializeField] private float HitsOverTime;

    float colSpawnTime, dot_starttime;
    int dot_indicator;
    public override void OnEnable()
    {
        dot_indicator = 1;
        dot_starttime = Time.time;
        colSpawnTime = Time.time + colActivateTime;
        SoundManager.instance.PlaySFX("PurpleSkill");
    }
    public override void Update()
    {
        if (colSpawnTime <= Time.time && (Time.time - dot_starttime) * HitsOverTime > dot_indicator / HitsOverTime) 
        {
            dot_indicator++;
            BoxCollider2D box = col as BoxCollider2D;
            var collisions = Physics2D.OverlapBoxAll((Vector2)transform.position + box.offset, box.size, 0, ~notInLayer);
            foreach (var collision in collisions)
            {
                if (collision.TryGetComponent(out IEntity entity))
                {
                    entity.OnReceiveDamage(Damage, EnemyInvisDuration);
                }
            }
        }
    }
    private void OnParticleSystemStopped()
    {
        Release();
    }
    public override void ProjectileLogic(GameObject other) { }
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
