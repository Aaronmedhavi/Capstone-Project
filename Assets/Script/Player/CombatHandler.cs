using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Pool;

public enum ColorItems { red, yellow, green, blue, purple, orange, brown }
//Red = api
//1. fireball kecil
//2. fireball kecil
//3. Ring of fire (Seeking)

//Yellow = petir
//1. 
//2. 
//3. 

//Biru = air
//1. 
//2. 
//3. 

//Hijau = daun akar2
//1. 
//2. 
//3. 

//Ungu = poison
//1. 
//2. 
//3. 

//Orange = fist
//Coklat = Tanah
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

    public void ChangeProjectiles(List<ProjectileCombo> projectiles)
    {
        this.projectiles = projectiles;
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
            var projectile = ObjectPoolManager.GetObject(Projectile.ProjectilePrefab, false, ObjectPoolManager.PooledInfo.GameObject).GetComponent<Projectile>();
            projectile.transform.position = transform.position + transform.right * 1;
            projectile.direction = transform.right;
            projectile.gameObject.SetActive(true);

            //Projectile logicnya blm
        }
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        time = Time.time + ComboInterval;
        isAttacking = null;
    }
}