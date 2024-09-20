using UnityEngine;

[CreateAssetMenu(fileName = "CoinsHandlerConfig", menuName = "Config/CoinsHandlerConfig")]
public class CoinsHandlerConfig : ScriptableObject
{
    [SerializeField] private string _KeyForPlayerPrefs = "Money";
    public string KeyForPlayerPrefs { get => _KeyForPlayerPrefs; }
}