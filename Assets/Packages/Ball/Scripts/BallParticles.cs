using System.Collections.Generic;
using ParticleSystemOverride;
using Unity.VisualScripting;
using UnityEngine;

public class BallParticles : MonoBehaviour
{
    [SerializeField] ParticleSystem hitParticlesPrefab;
    [SerializeField] PSOverrideCreatorMono psHit;
    [SerializeField] List<ParticleSystem> flames;
    [SerializeField] PSOverrideContainerMono psContinuous;
    List<ParticleSystem> psHitLiveInstances = new();

    public void PlayHitParticlesAt(Vector3 position, float intensity)
    {
        DebugDrawPoint(position, Color.red, 2f);
        ParticleSystem instance = Instantiate(hitParticlesPrefab, position, Quaternion.identity);
        ParticleCallback particleCallback = instance.AddComponent<ParticleCallback>();
        particleCallback.e_OnParticleSystemStopped += () => OnParticleStopped(instance);

        psHit.Model.CreateOverride(targetPS: instance, t: intensity)
                                        .Add(new OverrideFirstBurstCount())
                                        .Add(new OverrideStartSize())
                                        .Add(new OverrideDrag())
                                        .Add(new OverrideStartColor())
                                        .Add(new OverrideNoiseStrength())
                                        .Add(new OverrideSpeedModifier())
                                        .Add(new OverrideColorOverLifetime())
                                        .Apply();
        instance.Play();
        psHitLiveInstances.Add(instance);
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
    void OnParticleStopped(ParticleSystem ps)
    {
        if (psHitLiveInstances.Contains(ps))
            psHitLiveInstances.Remove(ps);
        ps.Stop();
        Destroy(ps.gameObject);
    }
    public void KillAllParticles()
    {
        foreach (var ps in psHitLiveInstances)
        {
            ps.Stop();
            Destroy(ps.gameObject);
        }
        psHitLiveInstances.Clear();
    }
    public void SetContinuousParticlesIntensity(float intensity)
    {
        psContinuous.Model.ApplyLerpTo(flames, intensity);
    }
}
