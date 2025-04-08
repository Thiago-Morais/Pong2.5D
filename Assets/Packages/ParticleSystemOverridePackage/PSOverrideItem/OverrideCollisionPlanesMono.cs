using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace ParticleSystemOverride
{
    public class OverrideCollisionPlanesMono : PSOverrideItemMono<OverrideCollisionPlanes>
    {
        [SerializeField] MinMaxCurve test;
    }
    [Serializable]
    public class OverrideCollisionPlanes : IPSOverrideItem
    {
        [SerializeField] List<Transform> planes = new();
        public OverrideCollisionPlanes() { }
        public OverrideCollisionPlanes(List<Transform> planes)
        {
            this.planes = planes;
        }
        public void Lerp(ParticleSystem targetPS, ParticleSystem minPS, ParticleSystem maxPS, float t)
        {
            CollisionModule collision = targetPS.collision;
            collision.enabled = true;
            for (int i = collision.planeCount - 1; i >= 0; i--)
                collision.RemovePlane(i);

            foreach (Transform plane in planes)
                collision.AddPlane(plane);
        }
    }
}