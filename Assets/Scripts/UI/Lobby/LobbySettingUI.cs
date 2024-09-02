using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbySettingUI : LobbyUI
{
    [SerializeField] GameObject uiObject;
    [SerializeField] Button exitBtn;
    public override void Init()
    {
        exitBtn.onClick.AddListener(() => OnOffUI(false));
    }

    public override void OnOffUI(bool _isActive)
    {
        uiObject.SetActive(_isActive);
    }
}
