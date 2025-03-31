using System;
using UnityEngine;

public class ParticleCallback : MonoBehaviour
{
    public event Action e_OnParticleTrigger;
    public event Action<GameObject> e_OnParticleCollision;
    public event Action e_OnParticleSystemStopped;
    void OnParticleTrigger() => e_OnParticleTrigger?.Invoke();
    void OnParticleCollision(GameObject other) => e_OnParticleCollision?.Invoke(other);
    void OnParticleSystemStopped() => e_OnParticleSystemStopped?.Invoke();
}