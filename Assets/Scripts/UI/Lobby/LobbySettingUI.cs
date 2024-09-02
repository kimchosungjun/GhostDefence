using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UIEnums;

public class LobbySettingUI : LobbyUI
{
    [SerializeField] GameObject uiObject;
    [SerializeField] Button exitBtn;
    [SerializeField] TMP_Dropdown dropDown;
    public override void Init()
    {
        dropDown.value = (int)GameManager.Instance.PDialogue.currentSettingUI;
        dropDown.onValueChanged.AddListener(ChangeDropDownValue);
        exitBtn.onClick.AddListener(() => UIController.ReleaseEnterKey());
    }

    public override void OnOffUI(bool _isActive)
    {
        uiObject.SetActive(_isActive);
    }

    public void ChangeDropDownValue(int _value)
    {
        LobbySetUI _changeType = (LobbySetUI)_value;
        GameManager.Instance.PDialogue.currentSettingUI = _changeType;
    }
}
