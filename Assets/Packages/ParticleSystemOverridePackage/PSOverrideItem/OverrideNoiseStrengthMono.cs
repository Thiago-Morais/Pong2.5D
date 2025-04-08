using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace ParticleSystemOverride
{
    public class OverrideNoiseStrengthMono : PSOverrideItemMono<OverrideNoiseStrength> { }
    public class OverrideNoiseStrength : IPSOverrideItem
    {
        public void Lerp(ParticleSystem targetPS, ParticleSystem minPS, ParticleSystem maxPS, float t)
        {
            NoiseModule module = targetPS.noise;
            module.strength = Util.Lerp(minPS.noise.strength, maxPS.noise.strength, t);
        }
    }
}