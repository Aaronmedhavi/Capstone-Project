using UnityEngine;

public class EffectParticle : ProjectileParticle
{
    public void Awake()
    { 
        var main = particle.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }
    public override void ParticleLogic(GameObject other) { }
}
