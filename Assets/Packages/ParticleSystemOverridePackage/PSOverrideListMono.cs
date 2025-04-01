using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParticleSystemOverride
{
    public class PSOverrideCreatorMono : MonoBehaviour
    {
        [SerializeField] PSOverrideCreator model;
        public PSOverrideCreator Model => model;
    }
    [Serializable]
    public class PSOverrideCreator
    {
        [SerializeField] List<PSOverrideItemMono> fixedOverrides;
        [Header("Minimum")]
        [SerializeField] ParticleSystem minParticleProperties;
        [Header("Maximum")]
        [SerializeField] ParticleSystem maxParticleProperties;

        public PSOverrideCreator() { }
        public PSOverrideCreator(ParticleSystem minParticleProperties, ParticleSystem maxParticleProperties, List<PSOverrideItemMono> fixedOverrides) : this()
        {
            this.minParticleProperties = minParticleProperties;
            this.maxParticleProperties = maxParticleProperties;
            this.fixedOverrides = fixedOverrides;
        }

        public PSOverrideList CreateOverride(ParticleSystem targetPS, float t)
        {
            PSOverrideList psOverrideList = new(targetPS, minPS: minParticleProperties, maxPS: maxParticleProperties, t: t);
            foreach (var o in fixedOverrides)
                psOverrideList.Add(o);
            return psOverrideList;
        }
    }
}