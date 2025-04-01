using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace ParticleSystemOverride
{
    public class OverrideStartSize : IPSOverrideItem
    {
        public void Apply(ParticleSystem targetPS, ParticleSystem minPS, ParticleSystem maxPS, float t)
        {
            MainModule main = targetPS.main;
            main.startSize = Util.Lerp(minPS.main.startSize, maxPS.main.startSize, t);
        }
    }
}