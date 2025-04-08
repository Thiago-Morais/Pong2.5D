using UnityEngine;

namespace ParticleSystemOverride
{
    public interface IPSOverrideItem
    {
        void Lerp(ParticleSystem targetPS, ParticleSystem minPS, ParticleSystem maxPS, float t);
    }
}