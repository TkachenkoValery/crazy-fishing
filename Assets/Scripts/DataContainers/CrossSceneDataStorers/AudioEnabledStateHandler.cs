using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioEnabledStateHandler", menuName = "CrossSceneDataStorers/AudioEnabledStateHandler")]
public class AudioEnabledStateHandler : ScriptableObject
{
    [SerializeField] private bool _IsAudioEnabled = true;
    public bool IsAudioEnabled
    {
        get => _IsAudioEnabled;
        set
        {
            _IsAudioEnabled = value;
            OnChangingAudioEnableState?.Invoke(value);
        }
    }

    public event Action<bool> OnChangingAudioEnableState;

    public void SwitchEnableState()
    {
        IsAudioEnabled = !IsAudioEnabled;
    }
}