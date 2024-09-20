using UnityEngine.UI;

public class AllNextSceneLoaderButtonsWithSettingLevelData : AllButtonsData<int>
{
    public AllNextSceneLoaderButtonsWithSettingLevelData(Button[] buttons, NextSceneLoaderWithSettingLevel[] performers) : base(buttons, performers)
    {

    }
}

public class NextSceneLoaderButtonsWithSettingLevel : OneActionButtonsWithParameter<int>
{
    public NextSceneLoaderButtonsWithSettingLevel(AllNextSceneLoaderButtonsWithSettingLevelData buttonsData) : base(buttonsData)
    {

    }
}