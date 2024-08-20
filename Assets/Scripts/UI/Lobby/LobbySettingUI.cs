using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySettingUI : LobbyUI
{
    [SerializeField] GameObject uiObject;
    public override void Init()
    {
        
    }

    public override void OnOffUI(bool _isActive)
    {
        uiObject.SetActive(_isActive);
    }
}
