using System.Collections.Generic;

public enum GameState
{
    NotStarted,
    Ongoing,
    Finished
}

public class GameCycleEventHandler
{
    private readonly List<IGameListener> GameListeners = new();

    public GameState CurrentGameState { get; private set; } = GameState.Ongoing;

    public void AddListener(IGameListener gameListener)
    {
        GameListeners.Add(gameListener);
    }
    
    public void RemoveListener(IGameListener gameListener)
    {
        GameListeners.Remove(gameListener);
    }

    public void FinishGame()
    {
        CurrentGameState = GameState.Finished;
        foreach (IGameListener gameListener in GameListeners)
        {
            if(gameListener is IGameFinishListener gameFinishListener)
            {
                gameFinishListener.OnFinishGame();
            }
        }
    }
}