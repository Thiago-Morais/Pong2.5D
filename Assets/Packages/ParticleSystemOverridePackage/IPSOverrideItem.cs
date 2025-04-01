using UnityEngine;

namespace ParticleSystemOverride
{
    public interface IPSOverrideItem
    {
        void Apply(ParticleSystem targetPS, ParticleSystem minPS, ParticleSystem maxPS, float t);
    }
}