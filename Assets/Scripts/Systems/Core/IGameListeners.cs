public interface IGameListener { }

public interface IGameFinishListener : IGameListener
{
    void OnFinishGame();
}