using System.Collections.Generic;
using ParticleSystemOverride;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;
public class BallParticles : MonoBehaviour
{
    [SerializeField] ParticleSystem hitParticlesPrefab;
    [SerializeField] PSOverrideCreatorMono psOverrideCreator;
    List<ParticleSystem> particleSystemLiveInstances = new();

    public void PlayHitParticlesAt(Vector3 position, float intensity)
    {
        DebugDrawPoint(position, Color.red, 2f);
        ParticleSystem instance = Instantiate(hitParticlesPrefab, position, Quaternion.identity);
        ParticleCallback particleCallback = instance.AddComponent<ParticleCallback>();
        particleCallback.e_OnParticleSystemStopped += () => OnParticleStopped(instance);

        psOverrideCreator.Model.CreateOverride(targetPS: instance, t: intensity)
                                        .Add(new OverrideFirstBurstCount())
                                        .Add(new OverrideStartSize())
                                        .Add(new OverrideDrag())
                                        .Add(new OverrideStartColor())
                                        .Add(new OverrideNoiseStrength())
                                        .Add(new OverrideSpeedModifier())
                                        .Add(new OverrideColorOverLifetime())
                                        .Apply();
        instance.Play();
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
