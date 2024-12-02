using UnityEngine;
public class DestroyableObject : MonoBehaviour, IEntity
{
    [Header("Object Stats")]
    [SerializeField] private float Health;
    [SerializeField] private float DefaultInvisibleCooldown;
    [SerializeField] private float DropChance;
    public bool IsAlive => false;
    private float _Health
    {
        get => Health;
        set
        {
            Health = value;
            if (Health <= 0) OnDeath();
        }
    }
    private bool isInvisible => InvisCooldown > Time.time;
    private float InvisCooldown;
    public void OnDeath()
    {
        DropManager.instance.DropRandom(DropChance, transform.position);
        ObjectPoolManager.ReleaseObject(gameObject);
    }
    public void OnReceiveDamage(float value, float InvisDuration = -1, Transform origin = null)
    {
        if (value == 0 || isInvisible) return;
        InvisCooldown = (InvisDuration != -1 ? InvisDuration : DefaultInvisibleCooldown) + Time.time;
        _Health -= value;
    }
}
