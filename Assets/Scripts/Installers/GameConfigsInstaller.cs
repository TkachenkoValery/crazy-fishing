using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameConfigsInstaller", menuName = "Installers/GameConfigsInstaller")]
public class GameConfigsInstaller : ScriptableObjectInstaller
{
    [SerializeField] private BobberThrowerConfig _BobberThrowerConfig;

    public override void InstallBindings()
    {
        Container.Bind<BobberThrowerConfig>().FromInstance(_BobberThrowerConfig);
    }
}