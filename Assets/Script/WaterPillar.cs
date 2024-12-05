using UnityEngine;
using System.Collections;

public class WaterPillar : ProjectileObject
{

    [Header("WaterPillar Settings")]
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private Animator animator;
    [SerializeField] private string AnimationName;
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
        if (triggertime <= Time.time && !triggered)
        {
            triggered = true;
            BoxCollider2D box = col as BoxCollider2D;
            Vector3 position = ray.collider ? ray.collider.transform.position : transform.position + transform.right * Distance / 2;
            RaycastHit2D ground = Physics2D.Raycast(position, Vector3.down, box.size.y, groundLayers);
            if (ground.collider != null)
            {
                transform.position = ground.point;
                animator.Play(AnimationName);
                //particle.Play();

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
            StartCoroutine(WaitExplosion());
        }
    }
    public IEnumerator WaitExplosion()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
        Release();
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