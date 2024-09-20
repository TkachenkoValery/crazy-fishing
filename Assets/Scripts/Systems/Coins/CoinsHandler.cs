using System;
using UnityEngine;
using Zenject;

public interface ICoinsHandler : IInitializable, IDisposable
{
    public int CurrentValue { get; set; }
    public event Action OnChangingValue;
}

public class CoinsHandler : ICoinsHandler
{
    private CoinsHandlerConfig _Config;
    
    private int _CurrentValue;
    public int CurrentValue
    {
        get => _CurrentValue;
        set
        {
            _CurrentValue = value >= 0 ? value : 0;
            OnChangingValue?.Invoke();
        }
    }

    public event Action OnChangingValue;

    public CoinsHandler(CoinsHandlerConfig config)
    {
        _Config = config;
    }

    private void SaveCoinsAmount()
    {
        PlayerPrefs.SetInt(_Config.KeyForPlayerPrefs, CurrentValue);
    }

    void IInitializable.Initialize()
    {
        CurrentValue = PlayerPrefs.GetInt(_Config.KeyForPlayerPrefs);
        OnChangingValue += SaveCoinsAmount;
    }

    void IDisposable.Dispose()
    {
        OnChangingValue -= SaveCoinsAmount;
    }
}