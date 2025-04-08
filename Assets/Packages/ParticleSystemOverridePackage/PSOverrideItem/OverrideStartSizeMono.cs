using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace ParticleSystemOverride
{
    public class OverrideStartSizeMono : PSOverrideItemMono<OverrideStartSize> { }
    public class OverrideStartSize : IPSOverrideItem
    {
        public void Lerp(ParticleSystem targetPS, ParticleSystem minPS, ParticleSystem maxPS, float t)
        {
            MainModule main = targetPS.main;
            main.startSize = Util.Lerp(minPS.main.startSize, maxPS.main.startSize, t);
        }
    }
}