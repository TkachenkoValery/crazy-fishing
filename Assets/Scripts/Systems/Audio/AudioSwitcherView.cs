using System;
using UnityEngine;
using Zenject;

public class AudioSwitcherView : IInitializable, IDisposable
{
    private AudioPlayer _AudioPlayer;
    private GameObject _EnabledSign;
    private GameObject _DisabledSign;

    public AudioSwitcherView(AudioPlayer audioPlayer, GameObject enabledSign, GameObject disabledSign)
    {
        _AudioPlayer = audioPlayer;
        _EnabledSign = enabledSign;
        _DisabledSign = disabledSign;
    }

    public void Initialize()
    {
        VisualizeAudioEnableState(_AudioPlayer.AudioEnabledStateHandler.IsAudioEnabled);
        _AudioPlayer.AudioEnabledStateHandler.OnChangingAudioEnableState += VisualizeAudioEnableState;
    }

    public void Dispose()
    {
        _AudioPlayer.AudioEnabledStateHandler.OnChangingAudioEnableState -= VisualizeAudioEnableState;
    }

    private void VisualizeAudioEnableState(bool newEnableState)
    {
        _EnabledSign.SetActive(newEnableState);
        _DisabledSign.SetActive(!newEnableState);
    }
}
