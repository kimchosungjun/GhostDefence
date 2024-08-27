using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyDataSlot : MonoBehaviour
{
    PlayerData playerData;
    [SerializeField] int buttonIndex;
    [SerializeField, Tooltip("0 : Slot, 1 : Create, 2 : Delete, 3 : Delete Yes, 4 : Delete No")] Button[] buttonGroup;
    [SerializeField, Tooltip("0 : No Data, 1 : Create Data, 2 : Have Data, 3 : DeleteButton")] GameObject[] slotObjectGroup;
    [SerializeField, Tooltip("0 : DeleteUI")] GameObject[] activeObject;
    [SerializeField, Tooltip("0,1 : Data Text, 2 : Input Text, 3 : Warn or Info Text")] TextMeshProUGUI[] textGroup;
    [SerializeField] TMP_InputField inputPlayerNameField;

    public LobbyStartUI StartUI { get; set; } = null;

    bool isDeleteDataState = false;
    bool isCreateDataState = false;

    public void Init(PlayerData playerData)
    {
        this.playerData = playerData;
        if (playerData == null)
        {
            slotObjectGroup[0].SetActive(true);
            for (int i=1; i<4; i++)
            {
                slotObjectGroup[i].SetActive(false);
            }
        }
        else
        {
            slotObjectGroup[0].SetActive(false);
            slotObjectGroup[1].SetActive(false);
            textGroup[0].text = playerData.playerName;
            if(playerData.isClearAll)
                textGroup[1].text = "100%";
            else
                textGroup[1].text = (100*(playerData.clearStage) / 6).ToString("0") + "%";
            slotObjectGroup[2].SetActive(true);
            slotObjectGroup[3].SetActive(true);
        }

        buttonGroup[0].onClick.AddListener(() => SlotClick());
        buttonGroup[1].onClick.AddListener(() => ClickCreateData());
        buttonGroup[2].onClick.AddListener(() => ActiveDeleteUI());
        buttonGroup[3].onClick.AddListener(() => ClickDelete());
        buttonGroup[4].onClick.AddListener(() => CancelDelete());
        inputPlayerNameField.onValueChanged.AddListener(AcceptValueChange);
    }

    public void SlotClick()
    {
        if (playerData == null)
        {
            slotObjectGroup[0].SetActive(false);
            slotObjectGroup[1].SetActive(true);
            buttonGroup[0].interactable = false;
            StartUI.SetInteractableSlotBtn(buttonIndex, false);
            isCreateDataState = true;
        }
        else
        {
            GameManager.Instance.Data.CurrentPlayerData = playerData;
            LoadingManager.Instance.CallStartLoading(SceneName.Stage);
        }
    }

    public void AcceptValueChange(string _value)
    {
        if (_value.Length <= 0)
            buttonGroup[1].gameObject.SetActive(false);
        else 
            buttonGroup[1].gameObject.SetActive(true);
        if (GameManager.Instance.Data.CanCreatePlayerData(_value))
            textGroup[3].text = "플레이어의 이름을 입력하세요.";
    }

    public void ClickCreateData()
    {
        string _name = textGroup[2].text;
        if (GameManager.Instance.Data.CanCreatePlayerData(_name))
        {
            playerData = GameManager.Instance.Data.CreatePlayerData(_name, buttonIndex);
            StartUI.SetInteractableSlotBtn(-1, true);
            isCreateDataState = false;
            UpdateData();
        }
        else
            textGroup[3].text = "<color=red>중복된 플레이어 이름입니다.";
    }

    public void UpdateData()
    {
        buttonGroup[0].interactable = true;
        slotObjectGroup[0].SetActive(false);
        slotObjectGroup[1].SetActive(false);
        textGroup[0].text = playerData.playerName;
        textGroup[1].text = ((playerData.clearStage + 1) / 6).ToString("0") + "%";
        slotObjectGroup[2].SetActive(true);
        slotObjectGroup[3].SetActive(true);
    }

    public void ActiveDeleteUI()
    {
        isDeleteDataState = true;
        activeObject[0].SetActive(true);
        SetActiveBtn(true, 3, 4);
        StartUI.SetInteractableSlotBtn(buttonIndex, false);
    }

    public void ClickDelete()
    {
        GameManager.Instance.Data.DeletePlayerData(playerData.nameIndex);
        activeObject[0].SetActive(false);
        slotObjectGroup[0].SetActive(true);
        for (int i = 1; i < 4; i++)
        {
            slotObjectGroup[i].SetActive(false);
        }
        StartUI.SetInteractableSlotBtn(-1, true);
        GameManager.Instance.Data.DeletePlayerData(playerData.playerName);
        playerData = null;
        inputPlayerNameField.text = "";
        isDeleteDataState = false;
    }

    public void CancelDelete()
    {
        activeObject[0].SetActive(false);
        SetActiveBtn(true, 0,1,2);
        StartUI.SetInteractableSlotBtn(buttonIndex, true);
        isDeleteDataState = false;
    }

    public void SetActiveBtn(bool _isActive, params int[] _index)
    {
        int _btnCnt = buttonGroup.Length;
        for (int i = 0; i < _btnCnt; i++)
        {
            buttonGroup[i].interactable = !_isActive;
        }

        int _indexLength = _index.Length;
        for (int k = 0; k < _indexLength;  k++)
        {
            if (_index[k] >= 0 && _index[k] < _btnCnt)
            {
                buttonGroup[_index[k]].interactable = _isActive;
            }
        }
    }

    public void SetAllBtn(bool _isActive)
    {
        int _btnCnt = buttonGroup.Length;
        for (int i = 0; i < _btnCnt; i++)
        {
            buttonGroup[i].interactable = _isActive;
        }
    }

    public bool CheckSlotInteractable()
    {
        if (buttonGroup[0].interactable == false)
            return false;
        return true;
    }

    public void ClickBack()
    {
        if (isDeleteDataState)
        {
            isDeleteDataState = false;
            CancelDelete();
        }
        if (isCreateDataState)
        {
            isCreateDataState = false;
            inputPlayerNameField.text = "";
            slotObjectGroup[0].SetActive(true);
            for (int i = 1; i < 4; i++)
            {
                slotObjectGroup[i].SetActive(false);
            }
        }
    }
}
