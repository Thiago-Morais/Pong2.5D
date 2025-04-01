using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace ParticleSystemOverride
{
    public class OverrideDrag : IPSOverrideItem
    {
        public void Apply(ParticleSystem targetPS, ParticleSystem minPS, ParticleSystem maxPS, float t)
        {
            LimitVelocityOverLifetimeModule module = targetPS.limitVelocityOverLifetime;
            module.drag = Util.Lerp(minPS.limitVelocityOverLifetime.drag, maxPS.limitVelocityOverLifetime.drag, t);
        }
    }
}