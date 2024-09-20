using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class BobberThrower : IInitializable, IFixedTickable, IDisposable
{
    private GameCycleEventHandler _GameCycleEventHandler;
    private CanvasScaler _CanvasScaler;
    private RectTransform _Bobber;
    private RectTransform _StringRectTransform; 
    private RectTransform _StringStartPoint; 
    private RectTransform _MainCharacter;
    private PressingHandler _PressingHandler;
    private BobberThrowerConfig _BobberThrowerConfig;
    private ICoinsHandler _CoinsHandler;
    private Fish[] _Fish;
    private List<Fish> _FishOnBobber = new();
    private List<Fish> _CaughtFish = new();

    private Vector2 _StringStartPointPosition;
    private bool _IsAnimatedNow = false;
    private float _TargetStringYScale;
    private bool _IsWideningString = false;
    private float _FishingCurrentSpeed;
    private bool _IsWaitingNow = false;
    private float _WaitedTime = 0;

    public BobberThrower(CanvasScaler canvasScaler, RectTransform bobber, RectTransform stringRectTransform, RectTransform stringStartPoint, RectTransform mainCharacter, PressingHandler pressingHandler, BobberThrowerConfig bobberThrowerConfig, ICoinsHandler coinsHandler, Fish[] fish, GameCycleEventHandler gameCycleEventHandler)
    {
        _CanvasScaler = canvasScaler;
        _Bobber = bobber;
        _StringRectTransform = stringRectTransform;
        _StringStartPoint = stringStartPoint;
        _MainCharacter = mainCharacter;
        _PressingHandler = pressingHandler;
        _BobberThrowerConfig = bobberThrowerConfig;
        _CoinsHandler = coinsHandler;
        _Fish = fish;
        _GameCycleEventHandler = gameCycleEventHandler;
    }

    void IInitializable.Initialize()
    {
        Vector2 MainCharacterStartPosition = new(_CanvasScaler.referenceResolution.x + _MainCharacter.anchoredPosition.x, _CanvasScaler.referenceResolution.x / Screen.width * Screen.height / 2 + _MainCharacter.anchoredPosition.y);
        float AngleInRadians = Mathf.Deg2Rad * Mathf.Abs(Mathf.Atan(_StringStartPoint.anchoredPosition.y / _StringStartPoint.anchoredPosition.x) * Mathf.Rad2Deg + _MainCharacter.transform.localEulerAngles.z - 360);
        _StringStartPointPosition = MainCharacterStartPosition + Mathf.Abs(_StringStartPoint.anchoredPosition.magnitude) * new Vector2(-Mathf.Cos(AngleInRadians), Mathf.Sin(AngleInRadians));
        _PressingHandler.OnClick += ThrowBobber;
    }

    void IDisposable.Dispose()
    {
        _PressingHandler.OnClick -= ThrowBobber;
    }

    void IFixedTickable.FixedTick()
    {
        if (_IsAnimatedNow || _IsWaitingNow)
        {
            foreach (Fish fish in _Fish)
            {
                if(!fish.IsSwimming)
                {
                    continue;
                }
                Vector3[] CatchingZoneInWorldSpace = new Vector3[4];
                fish.CatchingZone.GetWorldCorners(CatchingZoneInWorldSpace);
                Collider2D OverlappingCollider = Physics2D.OverlapArea(CatchingZoneInWorldSpace[0], CatchingZoneInWorldSpace[2]);
                if(OverlappingCollider != null && ((RectTransform)OverlappingCollider.transform).Equals(_Bobber))
                {
                    fish.TouchBobber();
                    _FishOnBobber.Add(fish);
                    break;
                }
            }
        }
        if (_IsAnimatedNow)
        {
            float DeltaYScale = _IsWideningString ? _FishingCurrentSpeed : -_FishingCurrentSpeed;
            float NewYScale = _StringRectTransform.sizeDelta.y + DeltaYScale;
            if ((_IsWideningString && NewYScale > _TargetStringYScale) || (!_IsWideningString && NewYScale < _TargetStringYScale))
            {
                NewYScale = _TargetStringYScale;
                _IsAnimatedNow = false;
                if (_IsWideningString)
                {
                    _IsWaitingNow = true;
                }
                else
                {
                    bool IsAtLeastOneNewFish = false;
                    foreach(Fish fish in _FishOnBobber)
                    {
                        _CoinsHandler.CurrentValue += fish.Reward;
                        _CaughtFish.Add(fish);
                        fish.BeCaught();
                        IsAtLeastOneNewFish = true;
                    }
                    if (!IsAtLeastOneNewFish)
                    {
                        _CoinsHandler.CurrentValue += Mathf.Abs(_BobberThrowerConfig.CoinsDecrease) * (-1);
                    }
                    if (_Fish.Count(fish => !fish.IsCaught) == 0)
                    {
                        _GameCycleEventHandler.FinishGame();
                    }
                }
            }
            SetStringSize(NewYScale);
            _FishingCurrentSpeed = Mathf.Max(_FishingCurrentSpeed + _BobberThrowerConfig.ThrowingAcceleration, _BobberThrowerConfig.ThrowingMinimalSpeed);
        }
        else if (_IsWaitingNow)
        {
            _WaitedTime += Time.fixedDeltaTime;
            if (_WaitedTime >= _BobberThrowerConfig.WaitingInWaterDuration)
            {
                _IsWaitingNow = false;
                _WaitedTime = 0;
                ChangeStringSizeGradually(_Bobber.sizeDelta.y / 2);
                _BobberThrowerConfig.ChangeDirection();
            }
        }
    }

    private void ThrowBobber(PointerEventData pointerEventData, Image image)
    {
        if (_IsAnimatedNow || _IsWaitingNow) return;

        Vector2 AnchoredPositionOfTextureCenter = image.rectTransform.anchoredPosition + new Vector2(_CanvasScaler.referenceResolution.x / 2, _CanvasScaler.referenceResolution.x / Screen.width * Screen.height / 2);
        Vector2 LeftBottomCornerPosition = AnchoredPositionOfTextureCenter - image.rectTransform.sizeDelta / 2;
        Vector2 RightTopCornerPosition = AnchoredPositionOfTextureCenter + image.rectTransform.sizeDelta / 2;

        bool IsPositionOfClickValid(Vector2 clickAnchoredPosition)
        {
            Vector2 LocalPoint = new((clickAnchoredPosition.x - LeftBottomCornerPosition.x) / (RightTopCornerPosition.x - LeftBottomCornerPosition.x),
                (clickAnchoredPosition.y - LeftBottomCornerPosition.y) / (RightTopCornerPosition.y - LeftBottomCornerPosition.y));
            Texture2D texture = image.sprite.texture;
            Color PixelColor = texture.GetPixel(
                Mathf.FloorToInt(LocalPoint.x * texture.width),
                Mathf.FloorToInt(LocalPoint.y * texture.height)
            );
            return PixelColor.a != 0;
        }

        Vector2 AnchoredPositionOfPressPoint =
            new(pointerEventData.pressPosition.x / Screen.width * _CanvasScaler.referenceResolution.x,
            pointerEventData.pressPosition.y * _CanvasScaler.referenceResolution.x / Screen.width);
        Vector2 NewFishingFloatPosition = AnchoredPositionOfPressPoint + (_Bobber.sizeDelta.y / 2) * (_StringStartPointPosition - AnchoredPositionOfPressPoint).normalized;
        if (IsPositionOfClickValid(AnchoredPositionOfPressPoint) && IsPositionOfClickValid(NewFishingFloatPosition))
        {
            ReplaceBobber(NewFishingFloatPosition);
        }
    }

    private void ReplaceBobber(Vector2 TargetFishingFloatPosition)
    {
        float Angle = -_MainCharacter.transform.localEulerAngles.z - (Mathf.Rad2Deg * Mathf.Atan((TargetFishingFloatPosition.x - _StringStartPointPosition.x) / (TargetFishingFloatPosition.y - _StringStartPointPosition.y)));
        _StringStartPoint.transform.localEulerAngles = new(_StringStartPoint.transform.localEulerAngles.x, _StringStartPoint.transform.localEulerAngles.y, Angle);
        float NewYScale = _Bobber.sizeDelta.y / 2;
        _BobberThrowerConfig.ChangeDirection();
        _FishingCurrentSpeed = _BobberThrowerConfig.ThrowingMaximalSpeed;
        SetStringSize(NewYScale);
        ChangeStringSizeGradually((TargetFishingFloatPosition - _StringStartPointPosition).magnitude);
    }

    private void SetStringSize(float NewYScale)
    {
        _StringRectTransform.sizeDelta = new(_StringRectTransform.sizeDelta.x, NewYScale);
        _StringRectTransform.anchoredPosition = new(_StringRectTransform.anchoredPosition.x, -NewYScale / 2);
    }

    private void ChangeStringSizeGradually(float targetStringYScale)
    {
        _TargetStringYScale = targetStringYScale;
        _IsWideningString = _TargetStringYScale > _StringRectTransform.sizeDelta.y;
        _IsAnimatedNow = true;
    }
}