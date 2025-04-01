using UnityEngine;

namespace ParticleSystemOverride
{
    public abstract class PSOverrideItemMono : MonoBehaviour, IPSOverrideItem
    {
        public abstract void Apply(ParticleSystem targetPS, ParticleSystem minPS, ParticleSystem maxPS, float t);
    }
    public class PSOverrideItemMono<T> : PSOverrideItemMono, IPSOverrideItem where T : IPSOverrideItem
    {
        [SerializeField] T model;
        public override void Apply(ParticleSystem targetPS, ParticleSystem minPS, ParticleSystem maxPS, float t)
        {
            model.Apply(targetPS, minPS, maxPS, t);
        }
    }
}