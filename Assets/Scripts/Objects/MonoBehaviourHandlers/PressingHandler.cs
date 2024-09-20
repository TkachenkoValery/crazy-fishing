using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PressingHandler : MonoBehaviour, IPointerClickHandler
{
    public event Action<PointerEventData, Image> OnClick;

    private Image ThisImage;

    private void Awake()
    {
        ThisImage = GetComponent<Image>();
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData pointerEventData)
    {
        OnClick?.Invoke(pointerEventData, ThisImage);
    }
}