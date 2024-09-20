using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameSystemsInstaller : MonoInstaller
{
    [Header("Audio")]
    [SerializeField] private AudioSource _Music;

    [Header("Fish")]
    [SerializeField] private GameObject _FishPrefab;
    [SerializeField] private RectTransform _FishParent;

    [Header("BobberThrower")]
    [SerializeField] private CanvasScaler _CanvasScaler;
    [SerializeField] private RectTransform _Bobber;
    [SerializeField] private RectTransform _String;
    [SerializeField] private RectTransform _StringStartPoint;
    [SerializeField] private RectTransform _MainCharacter;
    [SerializeField] private PressingHandler _PressingHandler;

    [Header("UI")]
    [SerializeField] private GameObject _VictoryUI;
    [SerializeField] private List<Button> _HomeButtons = new();
    [SerializeField] private Text _TextWithQuantityOfCoins;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameCycleEventHandler>().AsSingle();

        Container.BindInterfacesAndSelfTo<AudioPlayer>().AsSingle().WithArguments(_Music);
        
        Container.BindInterfacesTo<CoinsHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<CoinsUIView>().AsSingle().WithArguments(new ObservableCollection<Text>() { _TextWithQuantityOfCoins });
        Container.BindInterfacesAndSelfTo<GameFinishView>().AsSingle().WithArguments(_VictoryUI);
        _HomeButtons.ForEach(button => Container.BindInterfacesAndSelfTo<BackButton>().AsCached().WithArguments(button));
        
        Container.BindInterfacesAndSelfTo<BobberThrower>().AsSingle().WithArguments(new object[] { _CanvasScaler, _Bobber, _String, _StringStartPoint, _MainCharacter, _PressingHandler }).NonLazy();

        FishListStorer FishListStorer = Container.Resolve<LevelDataStorer>().CurrentFishList;
        for (int i = 0; i < FishListStorer.NumberOfFishes; i++)
        {
            GameObject NewFish = Instantiate(_FishPrefab);
            RectTransform NewFishRectTransform = (RectTransform)NewFish.transform;
            Container.BindInterfacesAndSelfTo<Fish>().AsCached().WithArguments(NewFishRectTransform, NewFishRectTransform.GetComponent<Image>(), _FishParent, (RectTransform)NewFishRectTransform.GetComponentInChildren<CatchingZoneMarker>().transform, _Bobber, FishListStorer[i]);
        }
    }
}