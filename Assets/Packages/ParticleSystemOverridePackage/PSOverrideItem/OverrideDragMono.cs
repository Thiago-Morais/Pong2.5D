using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace ParticleSystemOverride
{
    public class OverrideDragMono : PSOverrideItemMono<OverrideDrag> { }
    public class OverrideDrag : IPSOverrideItem
    {
        public void Lerp(ParticleSystem targetPS, ParticleSystem minPS, ParticleSystem maxPS, float t)
        {
            LimitVelocityOverLifetimeModule module = targetPS.limitVelocityOverLifetime;
            module.drag = Util.Lerp(minPS.limitVelocityOverLifetime.drag, maxPS.limitVelocityOverLifetime.drag, t);
        }
    }
}