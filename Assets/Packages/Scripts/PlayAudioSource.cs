using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[ExecuteInEditMode]
public class PlayAudioSource : MonoBehaviour
{
    [SerializeField] bool play;
    public KeyCode playShortcut = KeyCode.P;
    AudioSource audioSource;

    [ContextMenu(nameof(Awake))]
    void Awake()
    {
        Reset();
    }
    void Update()
    {
        if (play)
        {
            play = false;
            Play();
        }
    }
    [ContextMenu(nameof(Play))] public void Play() => audioSource.Play();
    void Reset()
    {
        audioSource = GetComponent<AudioSource>();
    }
}