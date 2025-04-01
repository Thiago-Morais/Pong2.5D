using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace ParticleSystemOverride
{
    public class OverrideSpeedModifier : IPSOverrideItem
    {
        public void Apply(ParticleSystem targetPS, ParticleSystem minPS, ParticleSystem maxPS, float t)
        {
            VelocityOverLifetimeModule velocityOverLifetime = targetPS.velocityOverLifetime;
            velocityOverLifetime.speedModifier = Util.Lerp(minPS.velocityOverLifetime.speedModifier, maxPS.velocityOverLifetime.speedModifier, t);
        }
    }
}