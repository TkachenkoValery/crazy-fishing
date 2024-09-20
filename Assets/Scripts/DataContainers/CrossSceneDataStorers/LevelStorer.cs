using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataStorer", menuName = "CrossSceneDataStorers/LevelDataStorer")]
public class LevelDataStorer : ScriptableObject
{
    [SerializeField] private List<FishListStorer> _FishAtAllLevels;

    [SerializeField] private int _Level = 0;
    public int Level
    {
        get => _Level;
        set
        {
            if(value >= 0 && value < _FishAtAllLevels.Count)
            {
                _Level = value;
            }
        }
    }

    public FishListStorer CurrentFishList { get => _FishAtAllLevels[Level]; }
}