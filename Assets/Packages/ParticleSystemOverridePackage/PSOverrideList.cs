using System.Collections.Generic;
using UnityEngine;

namespace ParticleSystemOverride
{
    public class PSOverrideList
    {
        ParticleSystem targetPS;
        ParticleSystem minPS;
        ParticleSystem maxPS;
        float t;
        List<IPSOverrideItem> overrides = new List<IPSOverrideItem>();

        public PSOverrideList(ParticleSystem targetPS, ParticleSystem minPS, ParticleSystem maxPS, float t)
        {
            this.targetPS = targetPS;
            this.minPS = minPS;
            this.maxPS = maxPS;
            this.t = t;
        }
        public PSOverrideList Add(IPSOverrideItem overrideFirstBurstCount)
        {
            overrides.Add(overrideFirstBurstCount);
            return this;
        }
        public void Apply()
        {
            foreach (IPSOverrideItem overrideFirstBurstCount in overrides)
                overrideFirstBurstCount.Lerp(targetPS, minPS, maxPS, t);
        }
    }
}