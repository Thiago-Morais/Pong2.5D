using System.Collections.Generic;
using UnityEngine;


public class OverrideParticleSystemProperties : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> particleSystemList;
    [SerializeField] List<Transform> planes;
    void Start()
    {
        OverridePlanes();
    }
    [ContextMenu(nameof(OverridePlanes))]
    public void OverridePlanes() => OverridePlanesIn(particleSystemList);
    public void OverridePlanesIn(IEnumerable<ParticleSystem> particleSystemList)
    {
        OverridePlanesIn(particleSystemList, planes);
    }
    public static void OverridePlanesIn(IEnumerable<ParticleSystem> particleSystemList, List<Transform> planes)
    {
        foreach (ParticleSystem ps in particleSystemList)
        {
            ParticleSystem.CollisionModule collision = ps.collision;
            collision.enabled = true;
            for (int i = collision.planeCount - 1; i >= 0; i--)
                collision.RemovePlane(i);

            foreach (Transform plane in planes)
                collision.AddPlane(plane);
        }
    }
}
