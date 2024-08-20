using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyExitUI : LobbyUI
{
    [SerializeField, Tooltip("0: Exit, 1: Back")] Button[] selectExitButtons;
    [SerializeField] GameObject uiObject;
    public override void Init()
    {
        selectExitButtons[0].onClick.AddListener(() => Application.Quit());
        selectExitButtons[1].onClick.AddListener(() => { UIController.ReleaseEnterKey(); });
    }

    public override void OnOffUI(bool _isActive)
    {
        uiObject.SetActive(_isActive);
    }
}
