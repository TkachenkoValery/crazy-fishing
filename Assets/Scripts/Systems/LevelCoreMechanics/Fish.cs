using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Fish : IInitializable, IFixedTickable
{
    private RectTransform _FishRectTransform;
    private Image _FishImage;
    private RectTransform _Parent;
    private RectTransform _Bobber;
    private FishConfig _Config;
    private float _CurrentSpeed;

    public RectTransform CatchingZone { get; private set; }
    public int Reward { get => _Config.Reward; }
    public bool IsGoingRight { get; private set; } = true;
    public bool IsSpeedingUp { get; private set; }
    public bool IsSwimming { get; private set; } = true;
    public bool IsCaught { get; private set; } = false;

    public Fish(RectTransform fishRectTransform, Image fishImage, RectTransform parent, RectTransform catchingZone, RectTransform bobber, FishConfig config)
    {
        _FishRectTransform = fishRectTransform;
        _FishImage = fishImage;
        _Parent = parent;
        CatchingZone = catchingZone;
        _Bobber = bobber;
        _Config = config;
    }

    void IInitializable.Initialize()
    {
        _FishImage.sprite = _Config.Sprite;
        _FishRectTransform.SetParent(_Parent);
        Vector2 FishSpriteSize = _Config.Sprite.rect.size;
        _FishRectTransform.sizeDelta = new Vector2(_Config.Height * FishSpriteSize.x / FishSpriteSize.y, _Config.Height);
        _FishRectTransform.anchoredPosition = _Config.StartAnchoredPosition;
    }

    void IFixedTickable.FixedTick()
    {
        if (!IsSwimming) return;
        float CentralPositionOfFish = (_Config.MinimalPositionAlongMovingAxis + _Config.MaximalPositionAlongMovingAxis) / 2;
        IsSpeedingUp = (IsGoingRight && _FishRectTransform.anchoredPosition[(int)_Config.MovementAxis] < CentralPositionOfFish)
            || (!IsGoingRight && _FishRectTransform.anchoredPosition[(int)_Config.MovementAxis] > CentralPositionOfFish);
        _CurrentSpeed += _Config.Acceleration * (IsSpeedingUp ? 1 : -1) * (IsGoingRight ? 1 : -1);
        _FishRectTransform.localEulerAngles = _CurrentSpeed > 0 ? Vector3.zero : 180 * Vector3.up;
        _FishRectTransform.anchoredPosition += _CurrentSpeed * (Vector2)_Config.MovementAxis.ConvertToVector3();
        if (IsGoingRight && _FishRectTransform.anchoredPosition[(int)_Config.MovementAxis] > _Config.MaximalPositionAlongMovingAxis)
        {
            IsGoingRight = false;
        }
    }

    public void TouchBobber()
    {
        _FishRectTransform.SetParent(_Bobber);
        _FishRectTransform.anchoredPosition = -_FishRectTransform.sizeDelta.y / 2 * Vector2.up;
        _FishRectTransform.localEulerAngles = 90 * Vector3.forward;
        IsSwimming = false;
    }

    public void BeCaught()
    {
        _FishRectTransform.gameObject.SetActive(false);
        IsCaught = true;
    }
}