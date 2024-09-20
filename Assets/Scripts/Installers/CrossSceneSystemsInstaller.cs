using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "CrossSceneSystemsInstaller", menuName = "Installers/ScriptableObjectInstallers/CrossSceneSystemsInstaller")]
public class CrossSceneSystemsInstaller : ScriptableObjectInstaller
{
    [SerializeField] private AudioEnabledStateHandler _AudioEnabledStateHandler;
    [SerializeField] private LevelDataStorer _LevelDataStorer;
    [SerializeField] private CoinsHandlerConfig _CoinsHandlerConfig;

    public override void InstallBindings()
    {
        Container.Bind<AudioEnabledStateHandler>().FromInstance(_AudioEnabledStateHandler).AsSingle();
        Container.Bind<LevelDataStorer>().FromInstance(_LevelDataStorer).AsSingle();
        Container.Bind<CoinsHandlerConfig>().FromInstance(_CoinsHandlerConfig).AsSingle();
        Container.BindInterfacesAndSelfTo<PreviousSceneLoader>().AsSingle();
        Container.BindInterfacesAndSelfTo<NextSceneLoader>().AsSingle();
    }
}