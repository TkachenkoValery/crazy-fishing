using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using UnityEngine.UI;
using Zenject;

public class CoinsUIView : IInitializable, IDisposable
{
    private ICoinsHandler CoinsHandler;
    public ObservableCollection<Text> TextsWithCoinsAmount { get; private set; }

    private NotifyCollectionChangedEventHandler OnChangingTextsWithCoinsAmount;

    public CoinsUIView(ICoinsHandler coinsHandler, ObservableCollection<Text> textsWithCoinsAmount)
    {
        CoinsHandler = coinsHandler;
        TextsWithCoinsAmount = textsWithCoinsAmount;
    }

    public void Initialize()
    {
        CoinsHandler.OnChangingValue += VisualizeCoins;
        VisualizeCoins();
        OnChangingTextsWithCoinsAmount += (sender, e) => VisualizeCoins();
        TextsWithCoinsAmount.CollectionChanged += OnChangingTextsWithCoinsAmount;
    }

    public void Dispose()
    {
        CoinsHandler.OnChangingValue -= VisualizeCoins;
        TextsWithCoinsAmount.CollectionChanged -= OnChangingTextsWithCoinsAmount;
        OnChangingTextsWithCoinsAmount = null;
    }

    private void VisualizeCoins()
    {
        string CoinsAmountAsString = CoinsHandler.CurrentValue.ToString();
        foreach (Text text in TextsWithCoinsAmount)
        {
            text.text = CoinsAmountAsString;
        }
    }
}