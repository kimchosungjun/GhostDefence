using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UIEnums;

public class TitleSelectIconButtonScale : OnButtonScale
{
    [SerializeField] LobbyUIController uiController;
    [SerializeField] LobbyUIBtn currentBtn;
    protected override void Awake()
    {
        base.Awake();
        if (uiController == null)
            uiController = GetComponentInParent<LobbyUIController>();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        uiController.OnMousePointSelectBtn(currentBtn);
    }

    public override void OnPointerExit(PointerEventData eventData) { }
}
