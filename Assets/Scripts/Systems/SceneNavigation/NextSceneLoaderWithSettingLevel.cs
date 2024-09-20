using UnityEngine;

public class NextSceneLoaderWithSettingLevel : OneActionPerformerWithParameter<int>
{
    private NextSceneLoader _NextSceneLoader;
    private LevelDataStorer _LevelDataStorer;

    public NextSceneLoaderWithSettingLevel(int levelToBeSet, NextSceneLoader nextSceneLoader, LevelDataStorer levelDataStorer) : base(levelToBeSet)
    {
        Parameter = levelToBeSet;
        _NextSceneLoader = nextSceneLoader;
        _LevelDataStorer = levelDataStorer;
    }

    public override void PerformAction()
    {
        Debug.Log(Parameter);
        _LevelDataStorer.Level = Parameter;
        _NextSceneLoader.PerformAction();
    }
}