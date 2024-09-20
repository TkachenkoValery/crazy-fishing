using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameListenerComposite : MonoBehaviour, IGameFinishListener
{
    [Inject]
    private GameCycleEventHandler _GameCycleEventHandler;
    
    [InjectLocal]
    private readonly List<IGameListener> _GameListeners = new();

    private void Start()
    {
        _GameCycleEventHandler.AddListener(this);
    }

    private void OnDestroy()
    {
        _GameCycleEventHandler.AddListener(this);
    }

    void IGameFinishListener.OnFinishGame()
    {
        foreach (IGameListener gameListener in _GameListeners)
        {
            if (gameListener is IGameFinishListener gameFinishListener)
            {
                gameFinishListener.OnFinishGame();
            }
        }
    }
}