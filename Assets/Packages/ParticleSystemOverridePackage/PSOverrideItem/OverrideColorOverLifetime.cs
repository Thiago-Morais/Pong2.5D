using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace ParticleSystemOverride
{
    public class OverrideColorOverLifetime : IPSOverrideItem
    {
        public void Apply(ParticleSystem targetPS, ParticleSystem minPS, ParticleSystem maxPS, float t)
        {
            ColorOverLifetimeModule colorOverLifetime = targetPS.colorOverLifetime;
            colorOverLifetime.color = Util.Lerp(minPS.colorOverLifetime.color, maxPS.colorOverLifetime.color, t);
        }
    }
}