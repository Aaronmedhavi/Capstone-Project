using System.Collections;
using UnityEngine;

public class NewCombatHandler : MonoBehaviour
{
    public enum Mode
    {
        Cycle,
        Focused
    }
    public CombatData data { get; set; }
    float comboTime, projectileCooldown, skillCooldown, attackgracePeriod;
    int attackIndex, projectileIndex;

    public bool isInGracePeriod => attackgracePeriod > Time.time;
    public bool IsInCombo => comboTime > Time.time;
    public bool SkillOnCooldown => skillCooldown > Time.time;
    public bool ProjectileOnCooldown => projectileCooldown > Time.time;
    private Animator animator;
    public Coroutine isAttacking { get; private set; }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Update()
    {
        if(isAttacking == null && isInGracePeriod)
        {
            isAttacking = StartCoroutine(Attacking());
        }
    }
    public void Attack()
    {
        if(isAttacking != null) attackgracePeriod = Time.time + data.attack_graceTime;
        else isAttacking = StartCoroutine(Attacking());
    }
    public void Projectile()
    {
        if (!ProjectileOnCooldown)
        {
            if(data.mode == Mode.Cycle)
            {
                projectileIndex = (projectileIndex + 1)%data.Projectiles.Count;
            }
            SpawnProjectile(data.Projectiles[projectileIndex]);
            projectileCooldown = Time.time + data.Projectile_Cooldown;
        }
    }
    public void Skill()
    {
        if (!SkillOnCooldown)
        {
            SpawnProjectile(data.SkillPrefab);
            projectileCooldown = Time.time + data.Skill_Cooldown;
        }
    }
    public void SpawnProjectile(GameObject _obj)
    {
        var obj = ObjectPoolManager.GetObject(_obj, false, ObjectPoolManager.PooledInfo.GameObject);
        var projectile = obj.GetComponent<Projectile>();
        projectile ??= obj.GetComponentInChildren<Projectile>();
        projectile.SetLayer(data.notInAttackLayer, obj);

        obj.transform.SetPositionAndRotation(transform.position + transform.right * 1, transform.rotation);
        obj.SetActive(true);
    }
    public IEnumerator Attacking()
    {
        attackgracePeriod = -1;
        if (!IsInCombo) attackIndex = 0;
        else attackIndex = (attackIndex + 1)%data.TotalCombos;

        CombatData.BasicAttack atkData = data.damages[attackIndex];
        float normalizedFrame = atkData.Frames != 0 ? atkData.FramePoint / atkData.Frames : 0;

        string animationName = "Attack" + (attackIndex + 1).ToString();

        animator.Play(animationName);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(animationName));
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > normalizedFrame);
        var hit = Physics2D.OverlapBox(transform.position + atkData.Offset, atkData.Size, 0, ~data.notInAttackLayer);
        if (hit && hit.TryGetComponent(out IEntity entity)) entity.OnReceiveDamage(atkData.Damage);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > data.anim_speed);
        comboTime = Time.time + data.ComboInterval;
        isAttacking = null;
    }
#if UNITY_EDITOR
    [SerializeField] CombatData.BasicAttack basicAttackView;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + basicAttackView.Offset, basicAttackView.Size);
    }
#endif
}
