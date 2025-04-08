using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParticleSystemOverride
{
    public class PSOverrideContainerMono : MonoBehaviour
    {
        [SerializeField] PSOverrideContainer model;
        public PSOverrideContainer Model => model;
        void Awake()
        {
            IPSOverrideItem[] overrideItems = GetComponents<IPSOverrideItem>();
            foreach (var oi in overrideItems)
                model.OverrideItems.Add(oi);
        }
        [ContextMenu(nameof(AddOverrideCollisionPlanes))] void AddOverrideCollisionPlanes() => gameObject.AddComponent<OverrideCollisionPlanesMono>();
        [ContextMenu(nameof(AddOverrideColorOverLifetime))] void AddOverrideColorOverLifetime() => gameObject.AddComponent<OverrideColorOverLifetimeMono>();
        [ContextMenu(nameof(AddOverrideDrag))] void AddOverrideDrag() => gameObject.AddComponent<OverrideDragMono>();
        [ContextMenu(nameof(AddOverrideFirstBurstCount))] void AddOverrideFirstBurstCount() => gameObject.AddComponent<OverrideFirstBurstCountMono>();
        [ContextMenu(nameof(AddOverrideNoiseStrength))] void AddOverrideNoiseStrength() => gameObject.AddComponent<OverrideNoiseStrengthMono>();
        [ContextMenu(nameof(AddOverrideSpeedModifier))] void AddOverrideSpeedModifier() => gameObject.AddComponent<OverrideSpeedModifierMono>();
        [ContextMenu(nameof(AddOverrideStartColor))] void AddOverrideStartColor() => gameObject.AddComponent<OverrideStartColorMono>();
        [ContextMenu(nameof(AddOverrideStartLifetime))] void AddOverrideStartLifetime() => gameObject.AddComponent<OverrideStartLifetimeMono>();
        [ContextMenu(nameof(AddOverrideStartSize))] void AddOverrideStartSize() => gameObject.AddComponent<OverrideStartSizeMono>();
        [ContextMenu(nameof(AddOverrideStartSpeed))] void AddOverrideStartSpeed() => gameObject.AddComponent<OverrideStartSpeedMono>();
    }
    [Serializable]
    public class PSOverrideContainer
    {
        [Header("Minimum")]
        [SerializeField] List<ParticleSystem> minParticleProperties = new();
        [Header("Maximum")]
        [SerializeField] List<ParticleSystem> maxParticleProperties = new();
        readonly List<IPSOverrideItem> overrideItems = new();
        public List<IPSOverrideItem> OverrideItems => overrideItems;

        public PSOverrideContainer() { }
        public PSOverrideContainer(List<ParticleSystem> minParticleProperties, List<ParticleSystem> maxParticleProperties)
        {
            this.minParticleProperties = minParticleProperties;
            this.maxParticleProperties = maxParticleProperties;
        }
        public void ApplyLerpTo(List<ParticleSystem> targetPS, float t)
        {
            foreach (IPSOverrideItem oi in overrideItems)
                for (int i = 0; i < targetPS.Count; i++)
                    oi.Lerp(targetPS[i], minParticleProperties[i], maxParticleProperties[i], t);
        }
    }
}