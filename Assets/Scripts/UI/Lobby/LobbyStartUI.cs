using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyStartUI : LobbyUI
{
    [SerializeField] Button backButton;
    [SerializeField] GameObject uiObject;
    [SerializeField] LobbyDataSlot[] lobbyDataSlotGroup;
    PlayerData[] playerDataGroup = new PlayerData[3];
    public override void Init()
    {
        if (uiObject.activeSelf)
            uiObject.SetActive(false);
        backButton.onClick.AddListener(() => ClickBackBtn());
    }

    private void Start()
    {
        playerDataGroup = GameManager.Instance.Data.LoadPlayerData();
        for(int i=0; i<3; i++)
        {
            lobbyDataSlotGroup[i].Init(playerDataGroup[i]);
            lobbyDataSlotGroup[i].StartUI = this;
        }
    }

    public override void OnOffUI(bool _isActive)
    {
        uiObject.SetActive(_isActive);
    }

    public void SetInteractableSlotBtn(int _index, bool _isActive)
    {
        int cnt = lobbyDataSlotGroup.Length;
        for(int idx=0; idx<cnt; idx++)
        {
            if (idx == _index)
                continue;
            lobbyDataSlotGroup[idx].SetAllBtn(_isActive);
        }
    }

    public void ClickBackBtn()
    {
        bool _canSlotInteractable = true;
        for(int i=0; i<3; i++)
        {
            if(!lobbyDataSlotGroup[i].CheckSlotInteractable())
            {
                _canSlotInteractable = false;
                break;
            }
        }

        if (!_canSlotInteractable)
        {
            // 데이터 생성중이거나 삭제중
            SetInteractableSlotBtn(-1, true);
            for (int i = 0; i < 3; i++)
            {
                lobbyDataSlotGroup[i].ClickBack();
            }
        }
        else
        {
            // 창 닫기
            UIController.CloseStartUI();
        }
    }

    public override bool CanCloseWindow()
    {
        for (int i = 0; i < 3; i++)
        {
            if (!lobbyDataSlotGroup[i].CheckSlotInteractable())
                return false;
        }
        return true;
    }
}
