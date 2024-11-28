using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum Color { red, yellow, green, blue, purple, orange, brown }
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
public class PlayerCombat : MonoBehaviour 
{
    [SerializeField] private List<ProjectileCombo> projectiles = new();
    [SerializeField] private float TotalCombos = 3;
    [SerializeField] private float ComboInterval;

    private Rigidbody2D rb2d;
    public Coroutine isAttacking { get; private set; }

    [NonSerialized] public Player player;

    private Animator animator => player.animator;
    //ObjectPool<Projectile> ProjectilePool;
    float time = 0;
    int index = 0;

    private void Awake()
    {
        //ProjectilePool = new ObjectPool<Projectile>(createProjectile, getProjectile, returnProjectile, destroyProjectile, true, 150, 10_000);
    }
    public void Attack() => isAttacking ??= StartCoroutine(Attacking());
    public IEnumerator Attacking()
    {
        if (player.IsGrounded)
        {
            if (Time.time >= time || index + 1 >= TotalCombos) index = 0;
            else index++;

            string animationName = "Attack" + (index + 1).ToString();
            var Projectile = projectiles.Find(x => x.Combo_number == index + 1);
            animator.Play(animationName);
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(animationName));
            if (Projectile.Combo_number != 0) //struct gabisa kosong
            {
                float time = Projectile.ProjectileSpawnFrame / Projectile.FramesInAnimation;
                yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > time);
                //ProjectilePool.Get();
            }
            time = Time.time + ComboInterval;
        }
        isAttacking = null;
    }
    //private Projectile createProjectile()
    //{
    //    GameObject instance = Instantiate(ProjectilePrefab, Vector3.zero, Quaternion.identity);
    //    Projectile projectile = instance.GetComponent<Projectile>();
    //    projectile.direction = player.transform.right;
    //    projectile.onRelease = ProjectilePool.Release;
    //    return projectile;
    //}
    //private void getProjectile(Projectile instance)
    //{
    //    instance.gameObject.SetActive(true);
    //}
    //public void returnProjectile(Projectile instance)
    //{
    //    instance.gameObject.SetActive(false);
    //}
    //private void destroyProjectile(Projectile instance)
    //{
    //    Destroy(instance);
    //}
}