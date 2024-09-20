using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AudioPlayer : IInitializable, IDisposable
{
    public enum SoundID
    {
        Music
    }

    private AudioSource _Music;

    public Dictionary<SoundID, AudioSource> BackgroundSounds { get; set; }

    public AudioEnabledStateHandler AudioEnabledStateHandler { get; private set; }

    public AudioPlayer(AudioEnabledStateHandler audioEnabledStateHandler, AudioSource music)
    {
        AudioEnabledStateHandler = audioEnabledStateHandler;
        _Music = music;
    }

    void IInitializable.Initialize()
    {
        BackgroundSounds = new()
        {
            { SoundID.Music, _Music }
        };
        AudioEnabledStateHandler.OnChangingAudioEnableState += SetStateToAllBackgroundSounds;
        SetStateToAllBackgroundSounds(AudioEnabledStateHandler.IsAudioEnabled);
    }

    void IDisposable.Dispose()
    {
        AudioEnabledStateHandler.OnChangingAudioEnableState -= SetStateToAllBackgroundSounds;
    }

    private void ApplyActionToEveryBackgroundSound(Action<AudioSource> actionToBeApplied)
    {
        foreach (KeyValuePair<SoundID, AudioSource> soundData in BackgroundSounds)
        {
            if (soundData.Value != null)
            {
                actionToBeApplied?.Invoke(soundData.Value);
            }
        }
    }

    private void PlayBackgroundSounds()
    {
        ApplyActionToEveryBackgroundSound(audio => audio.Play());
    }

    private void PauseBackgroundSounds()
    {
        ApplyActionToEveryBackgroundSound(audio => audio.Pause());
    }

    public void SetStateToAllBackgroundSounds(bool newState)
    {
        if(newState)
        {
            PlayBackgroundSounds();
        }
        else
        {
            PauseBackgroundSounds();
        }
    }
}