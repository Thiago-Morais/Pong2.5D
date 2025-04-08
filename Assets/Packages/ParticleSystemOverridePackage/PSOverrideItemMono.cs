using UnityEngine;

namespace ParticleSystemOverride
{
    public abstract class PSOverrideItemMono : MonoBehaviour, IPSOverrideItem
    {
        public abstract void Lerp(ParticleSystem targetPS, ParticleSystem minPS, ParticleSystem maxPS, float t);
    }
    public class PSOverrideItemMono<T> : PSOverrideItemMono, IPSOverrideItem where T : IPSOverrideItem, new()
    {
        [SerializeField] T model = new T();
        public override void Lerp(ParticleSystem targetPS, ParticleSystem minPS, ParticleSystem maxPS, float t)
        {
            model.Lerp(targetPS, minPS, maxPS, t);
        }
    }
}