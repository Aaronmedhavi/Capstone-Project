using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Combo
{
    public GameObject Projectile;
    public Attack_Type type;
    public float FrameInWhichProjectileSpawn;
    public float AnimationFrames;
}
public class Player_Attack : MonoBehaviour
{
    public List<Combo> combos = new();

    public float MassIncreaseOnAttack;
    public float ComboInterval;

    private Animator LowerBody, UpperBody;
    private Rigidbody2D rb2d;
    private Coroutine coroutine;
    float time = 0;
    int index = 0;
    public void Set(Animator upperBody, Animator lowerBody, Rigidbody2D rb2d)
    {
        UpperBody = upperBody;
        LowerBody = lowerBody;
        this.rb2d = rb2d;
    }
    //Cara ganti attack patternya gimana yah
    public void Attack(bool AttackPRESSED)
    {
        if (AttackPRESSED && coroutine == null) coroutine = StartCoroutine(Attacking());
    }
    public IEnumerator Attacking()
    {
        rb2d.mass += MassIncreaseOnAttack;
        if (Time.time >= time || index + 1 >= combos.Count) index = 0;
        else index++;

        float ProjectileSpawnTime = combos[index].type == Attack_Type.projectile ? combos[index].FrameInWhichProjectileSpawn / combos[index].AnimationFrames : 10;
        string animationName = "Attack" + (index+1).ToString();
        bool hasSpawned = false;

        LowerBody.Play(animationName);
        UpperBody.Play(animationName);
        yield return new WaitUntil(() => LowerBody.GetCurrentAnimatorStateInfo(0).IsName(animationName));
        while (LowerBody.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f)
        {
            if(LowerBody.GetCurrentAnimatorStateInfo(0).normalizedTime >= ProjectileSpawnTime && !hasSpawned)
            {
                var obj = Instantiate(combos[index].Projectile,transform.position,transform.rotation);
                hasSpawned = true;
            }
            yield return null;
        }
        rb2d.mass -= MassIncreaseOnAttack;
        coroutine = null;
        time = Time.time + ComboInterval;
    }
}
