using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishListStorer", menuName = "Config/FishListStorer")]
public class FishListStorer : ScriptableObject
{
    [SerializeField] private List<FishConfig> _FishList = new();

    public int NumberOfFishes { get => _FishList.Count; }

    public FishConfig this[int index]
    {
        get
        {
            if(index < 0 || index >= NumberOfFishes)
            {
                Debug.LogWarning("An attempt to get fish config with invalid index was made. Returning null");
                return null;
            }
            return _FishList[index];
        }
    }
}