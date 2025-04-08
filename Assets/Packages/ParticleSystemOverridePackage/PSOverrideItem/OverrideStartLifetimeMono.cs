using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace ParticleSystemOverride
{
    public class OverrideStartLifetimeMono : PSOverrideItemMono<OverrideStartLifetime> { }
    public class OverrideStartLifetime : IPSOverrideItem
    {
        public void Lerp(ParticleSystem targetPS, ParticleSystem minPS, ParticleSystem maxPS, float t)
        {
            MainModule main = targetPS.main;
            main.startLifetime = Util.Lerp(minPS.main.startLifetime, maxPS.main.startLifetime, t);
        }
    }
}