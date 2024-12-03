using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected float Damage;
    [SerializeField] protected float EnemyInvisDuration = -1; //ini sengaja biar kalo mau ada yang bredet, ini bredet
    protected GameObject thisObject;
    protected GameObject ToRelease
    {
        get
        {
            if (thisObject == null) return gameObject;
            return thisObject;
        }
    }
    protected LayerMask layer;
    public abstract void SetLayer(LayerMask layer, GameObject obj);
    public abstract void ParticleLogic(GameObject other);
    public void Release() => ObjectPoolManager.ReleaseObject(ToRelease);
}
public abstract class ProjectileObject : Projectile
{
    [SerializeField] protected Collider2D col;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float lifeTime;

    private float time;
    public override void SetLayer(LayerMask layer, GameObject obj)
    {
        thisObject = obj;
        this.layer = layer;
        col.isTrigger = true;
        col.includeLayers = layer;
        col.excludeLayers = ~layer;
        col.contactCaptureLayers = layer;
        col.callbackLayers = layer;
    }
    public virtual void OnEnable()
    {
        time = Time.time + lifeTime;
    }
    public virtual void Update()
    {
        if (time < Time.time) Release();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        ParticleLogic(collision.gameObject);
    }
}
public class DamagePerParticle : ProjectileParticle
{
    public override void ParticleLogic(GameObject other)
    {
        if(other.TryGetComponent(out IEntity entity))
        {
            entity.OnReceiveDamage(Damage, EnemyInvisDuration);
        }
    }
}
public abstract class ProjectileParticle : Projectile
{
    [SerializeField] protected ParticleSystem particle;
    public void OnEnable()
    {
        particle.Play();
    }
    public override void SetLayer(LayerMask layer, GameObject obj)
    {
        thisObject = obj;
        this.layer = layer;
        var collider = particle.collision;
        collider.collidesWith = ~layer;
        collider.sendCollisionMessages = true;
        var main = particle.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }
    public void Stop()
    {
        particle.Stop();
        Release();
    }
    private void OnParticleSystemStopped()
    {
        Release();
    }
    private void OnParticleCollision(GameObject other)
    {
        ParticleLogic(other);
    }
}