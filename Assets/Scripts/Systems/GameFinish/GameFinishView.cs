using UnityEngine;

public class GameFinishView : IGameFinishListener
{
    private GameObject _VictoryUI;

    public GameFinishView(GameObject victoryUI)
    {
        _VictoryUI = victoryUI;
    }

    void IGameFinishListener.OnFinishGame()
    {
        EnableWinUI();
    }

    public void EnableWinUI()
    {
        _VictoryUI.SetActive(true);
    }
}