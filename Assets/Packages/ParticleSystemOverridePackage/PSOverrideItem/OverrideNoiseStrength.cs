using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace ParticleSystemOverride
{
    public class OverrideNoiseStrength : IPSOverrideItem
    {
        public void Apply(ParticleSystem targetPS, ParticleSystem minPS, ParticleSystem maxPS, float t)
        {
            NoiseModule module = targetPS.noise;
            module.strength = Util.Lerp(minPS.noise.strength, maxPS.noise.strength, t);
        }
    }
}