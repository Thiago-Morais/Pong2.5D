using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace ParticleSystemOverride
{
    using static Util;
    public class OverrideFirstBurstCount : IPSOverrideItem
    {
        public void Apply(ParticleSystem targetPS, ParticleSystem minPS, ParticleSystem maxPS, float t)
        {
            EmissionModule emission = targetPS.emission;
            Burst burst = emission.GetBurst(0);
            burst.count = Lerp(minPS.emission.GetBurst(0).count, maxPS.emission.GetBurst(0).count, t);
            emission.SetBurst(0, burst);
        }
    }
}