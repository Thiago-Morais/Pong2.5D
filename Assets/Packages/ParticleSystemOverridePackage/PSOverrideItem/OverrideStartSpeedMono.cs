using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace ParticleSystemOverride
{
    public class OverrideStartSpeedMono : PSOverrideItemMono<OverrideStartSpeed> { }
    public class OverrideStartSpeed : IPSOverrideItem
    {
        public void Lerp(ParticleSystem targetPS, ParticleSystem minPS, ParticleSystem maxPS, float t)
        {
            MainModule main = targetPS.main;
            main.startSpeed = Util.Lerp(minPS.main.startSpeed, maxPS.main.startSpeed, t);
        }
    }
}