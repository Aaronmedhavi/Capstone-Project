using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ColorItems { red, yellow, green, blue, purple, orange, brown }
public enum Attack_Type { melee, projectile }
[Serializable]
public struct ProjectileCombo
{
    public int Combo_number;
    public GameObject ProjectilePrefab;

    [Header("Frame Settings")]
    public int FramesInAnimation;
    public int ProjectileSpawnFrame;
}
public class CombatHandler : MonoBehaviour 
{
    [SerializeField] private List<ProjectileCombo> projectiles = new();
    [SerializeField] private float TotalCombos = 3;
    [SerializeField] private float ComboInterval;
    [SerializeField] private Animator animator;
    public Coroutine isAttacking { get; private set; }
    LayerMask layer = default;

    public void ChangeProjectiles(List<ProjectileCombo> projectiles, LayerMask layer_type = default)
    {
        this.projectiles = projectiles;
        layer = layer_type;
    }
    float time = 0;
    int index = 0;
    public void Attack() => isAttacking ??= StartCoroutine(Attacking());
    public IEnumerator Attacking()
    {
        if (Time.time >= time || index + 1 >= TotalCombos) index = 0;
        else index++;

        string animationName = "Attack" + (index + 1).ToString();
        ProjectileCombo Projectile = projectiles.Find(x => x.Combo_number == index + 1);
        animator.Play(animationName);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(animationName));
        if (Projectile.Combo_number != 0) //struct gabisa kosong
        {
            float time = Projectile.ProjectileSpawnFrame / Projectile.FramesInAnimation;
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > time);
            var obj = ObjectPoolManager.GetObject(Projectile.ProjectilePrefab, false, ObjectPoolManager.PooledInfo.GameObject);
            var projectile = obj.GetComponent<Projectile>();
            projectile ??= obj.GetComponentInChildren<Projectile>();
            projectile.SetLayer(layer, obj);

            obj.transform.SetPositionAndRotation(transform.position + transform.right * 1, transform.rotation);
            obj.SetActive(true);
        }
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
        time = Time.time + ComboInterval;
        isAttacking = null;
    }
}