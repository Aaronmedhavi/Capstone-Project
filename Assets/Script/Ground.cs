using System.Collections;
using UnityEngine;

public class Ground : ProjectileObject
{
    [Header("Ground Settings (No lifeTime, follows particle Lifetime)")]
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private ParticleSystem FX;
    [SerializeField] private float Distance;
    [SerializeField] private float timeOverDistance;
    [SerializeField] private float speed;
    [SerializeField] private float MaxHits;
    [SerializeField] private Animator animator;
    [SerializeField] private string AnimationName;

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
        Vector3 location = ray.collider ? ray.collider.transform.position : transform.position + Distance / 2 * transform.right;
        transform.position = Vector2.MoveTowards(transform.position, location, speed * Time.deltaTime);
        if (triggertime <= Time.time && !triggered)
        {
            triggered = true;
            Debug.Log("triggered");
            BoxCollider2D box = col as BoxCollider2D;
            Vector3 position = ray.collider ? ray.collider.transform.position : transform.position + Distance / 2 * transform.right;
            RaycastHit2D ground = Physics2D.Raycast(position, Vector3.down, box.size.y, groundLayers);
            if (ground.collider != null)
            {
                Debug.Log("grass");
                transform.position = ground.point;
                animator.Play(AnimationName);
                if(FX) FX.Play();

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
