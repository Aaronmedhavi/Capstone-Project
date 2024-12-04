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
    protected LayerMask notInLayer;
    public abstract void SetLayer(LayerMask layer, GameObject obj);
    public abstract void ProjectileLogic(GameObject other);
    public void Release() => ObjectPoolManager.ReleaseObject(ToRelease);
}
public abstract class ProjectileObject : Projectile
{
    [SerializeField] protected Collider2D col;
    [SerializeField] protected float lifeTime;

    private float time;
    public override void SetLayer(LayerMask layer, GameObject obj)
    {
        thisObject = obj;
        this.notInLayer = layer;
        col.isTrigger = true;
        col.includeLayers = ~notInLayer;
        col.excludeLayers = notInLayer;
        col.contactCaptureLayers = ~notInLayer;
        col.callbackLayers = ~notInLayer;
    }
    public virtual void OnEnable()
    {
        time = Time.time + lifeTime;
    }
    public virtual void Update()
    {
        if (time <= Time.time) Release();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        ProjectileLogic(collision.gameObject);
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
        this.notInLayer = layer;
        var collider = particle.collision;
        collider.collidesWith = ~notInLayer;
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
        ProjectileLogic(other);
    }
}