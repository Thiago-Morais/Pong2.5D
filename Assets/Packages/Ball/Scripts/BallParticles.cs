using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallParticles : MonoBehaviour
{
    [SerializeField] ParticleSystem hitParticlesPrefab;
    [SerializeField] OverrideParticleSystemProperties overrideParticleSystemProperties;
    List<ParticleSystem> particleSystemLiveInstances = new();
    public void PlayHitParticles(BallMono from, Collider other)
    {
        Vector3 contactPoint = other.ClosestPointOnBounds(from.transform.position);
        DebugDrawPoint(contactPoint, Color.red, 2f);
        ParticleSystem instance = Instantiate(hitParticlesPrefab, contactPoint, Quaternion.identity);
        instance.Play();
        ParticleCallback particleCallback = instance.AddComponent<ParticleCallback>();
        particleCallback.e_OnParticleSystemStopped += () => OnParticleStopped(instance);
        overrideParticleSystemProperties.OverridePlanesIn(new[] { instance });
        particleSystemLiveInstances.Add(instance);
    }
    void OnParticleStopped(ParticleSystem ps)
    {
        if (particleSystemLiveInstances.Contains(ps))
            particleSystemLiveInstances.Remove(ps);
        ps.Stop();
        Destroy(ps.gameObject);
    }
    public void KillAllParticles()
    {
        foreach (var ps in particleSystemLiveInstances)
        {
            ps.Stop();
            Destroy(ps.gameObject);
        }
        particleSystemLiveInstances.Clear();
    }
    void DebugDrawPoint(Vector3 contactPoint, Color color, float duration)
    {
        Debug.DrawRay(contactPoint, Vector3.up * .5f, color, duration);
        Debug.DrawRay(contactPoint, Vector3.down * .5f, color, duration);
        Debug.DrawRay(contactPoint, Vector3.right * .5f, color, duration);
        Debug.DrawRay(contactPoint, Vector3.left * .5f, color, duration);
        Debug.DrawRay(contactPoint, Vector3.forward * .5f, color, duration);
        Debug.DrawRay(contactPoint, Vector3.back * .5f, color, duration);
    }
}
