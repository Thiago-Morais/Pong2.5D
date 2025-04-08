using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace ParticleSystemOverride
{
    public class OverrideStartColorMono : PSOverrideItemMono<OverrideStartColor> { }
    public class OverrideStartColor : IPSOverrideItem
    {
        public void Lerp(ParticleSystem targetPS, ParticleSystem minPS, ParticleSystem maxPS, float t)
        {
            MainModule main = targetPS.main;
            main.startColor = Util.Lerp(minPS.main.startColor, maxPS.main.startColor, t);
        }
    }
}