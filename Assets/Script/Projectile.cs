using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float Damage;
    [SerializeField] protected float EnemyInvisDuration = -1; //ini sengaja biar kalo mau ada yang bredet, ini bredet
    protected IEntity.type type;
    public virtual void SetType(IEntity.type type)
    {
        this.type = type;
    }
    public void Release() => ObjectPoolManager.ReleaseObject(gameObject);
}

public class ProjectileParticle : Projectile
{
    [SerializeField] protected ParticleSystem particle;
    public void OnEnable()
    {
        particle.Play();
    }
}
//public class Projectile : MonoBehaviour
//{
//    [Space(3)]
//    [Header("Projectile Settings")]
//    [SerializeField] protected float projectileSpeed;
//    [SerializeField] protected float lifeTime;


//    Rigidbody2D rb;
//    float time;
//    public virtual void Awake()
//    {
//        rb = GetComponent<Rigidbody2D>();
//    }
//    public virtual void OnEnable()
//    {
//        Debug.Log("Start");
//        rb.velocity = direction * projectileSpeed;
//        time = Time.time + lifeTime;
//        //yield return new WaitForSeconds(lifeTime);
//    }
//    public virtual void Update()
//    {
//        if(time < Time.time)
//        {
//            Release();
//        }
//    }
//    public virtual void OnCollisionEnter2D(Collision2D col)
//    {
//        Release();
//    }
//    public void Release() => ObjectPoolManager.ReleaseObject(gameObject);
//}

//[RequireComponent(typeof(Rigidbody2D))]
//public class Projectile : MonoBehaviour
//{
//    //public Action<Projectile> OnRelease;

//    //public float speed;
//    //public float time;
//    //public Vector3 direction;
//    //Rigidbody2D rb;
//    //private void Awake()
//    //{
//    //    rb = GetComponent<Rigidbody2D>();
//    //}
//    //private IEnumerator Start()
//    //{
//    //    rb.velocity = direction * speed;
//    //    yield return new WaitForSeconds(time);
//    //    OnRelease?.Invoke(this);
//    //}
//    //void OnCollisionEnter2D(Collider2D col)
//    //{
//    //    OnRelease?.Invoke(this);
//    //}
//}
