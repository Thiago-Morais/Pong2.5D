#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[ExecuteInEditMode]
[CustomEditor(typeof(PlayAudioSource))]
public class PlayAudioSourceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ProcessKey((PlayAudioSource)this.target);
    }
    void OnSceneGUI()
    {
        ProcessKey((PlayAudioSource)this.target);
    }
    void ProcessKey(PlayAudioSource target)
    {
        Event e = Event.current;
        if (e == null) return;
        if (e.isKey)
        {
            if (e.type == EventType.KeyUp)
            {
                if (e.keyCode == target.playShortcut)
                {
                    target.Play();
                }
            }
        }
    }
}
#endif