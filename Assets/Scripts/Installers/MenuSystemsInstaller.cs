using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[System.Serializable]
public struct ButtonsForLoadingNextLevelData
{
    [SerializeField] private int _LevelNumber;
    public int LevelNumber { get => _LevelNumber; }

    [SerializeField] private Button _Button;
    public Button Button { get => _Button; }
}

public class MenuSystemsInstaller : MonoInstaller
{
    [Header("Audio")]
    [SerializeField] private AudioSource _Music;

    [Header("UI")]
    [SerializeField] private Button _AudioSwitchingButton;
    [SerializeField] private GameObject _AudioEnabledSign;
    [SerializeField] private GameObject _AudioDisabledSign;
    [SerializeField] private Text _TextWithQuantityOfCoins;
    [SerializeField] private List<ButtonsForLoadingNextLevelData> _ButtonsForLoadingNextLevels = new();

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<AudioPlayer>().AsSingle().WithArguments(_Music);
        Container.BindInterfacesAndSelfTo<AudioSwitcher>().AsSingle();
        Container.BindInterfacesAndSelfTo<AudioSwitcherView>().AsSingle().WithArguments(_AudioEnabledSign, _AudioDisabledSign);
        Container.BindInterfacesAndSelfTo<AudioChangingButton>().AsCached().WithArguments(_AudioSwitchingButton);

        Container.BindInterfacesTo<CoinsHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<CoinsUIView>().AsSingle().WithArguments(new ObservableCollection<Text>() { _TextWithQuantityOfCoins });

        for (int i = 0; i < _ButtonsForLoadingNextLevels.Count; i++)
        {
            Container.BindInterfacesAndSelfTo<NextSceneLoaderWithSettingLevel>().AsCached().WithArguments(_ButtonsForLoadingNextLevels[i].LevelNumber);
        }
        Button[] AllButtonsForLoadingNextLevel = new Button[_ButtonsForLoadingNextLevels.Count];
        for (int i = 0; i < _ButtonsForLoadingNextLevels.Count; i++)
        {
            AllButtonsForLoadingNextLevel[i] = _ButtonsForLoadingNextLevels[i].Button;
        }
        Container.BindInterfacesAndSelfTo<AllNextSceneLoaderButtonsWithSettingLevelData>().AsSingle().WithArguments(AllButtonsForLoadingNextLevel);
        Container.BindInterfacesAndSelfTo<NextSceneLoaderButtonsWithSettingLevel>().AsSingle();
    }
}