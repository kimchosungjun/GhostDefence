using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UIEnums;

public class PauseUI : MonoBehaviour
{
    GameSceneUIController controller = null;

    [SerializeField] GameObject frameObject;
    [SerializeField] GameObject pauseObject;
    [SerializeField] GameObject settingObject;
    [SerializeField] GameObject decideObject;

    [Header("Pause")]
    [SerializeField] Button resumeBtn;
    [SerializeField] Button settingBtn;
    [SerializeField] Button LobbyBtn;

    [Header("Select Return Lobby")]
    [SerializeField, Tooltip("0:yes, 1:no")] Button[] decideBtns;

    [Header("Setting")]
    [SerializeField] TMP_Dropdown dropDown;
    [SerializeField] Button backBtn;
    public void Init(GameSceneUIController _controller)
    {
        controller = _controller;
        
        // Pause
        resumeBtn.onClick.AddListener(() => _controller.ResumeGame());
        settingBtn.onClick.AddListener(() => ActiveSetting());
        LobbyBtn.onClick.AddListener(() => DecideReturnLobby());

        decideBtns[0].onClick.AddListener(() => DecideYes());
        decideBtns[1].onClick.AddListener(() =>DecideNo());

        // Setting
        dropDown.value = (int)GameManager.Instance.PDialogue.CurrentSettingUI;
        dropDown.onValueChanged.AddListener(SettingOptionChange);
        backBtn.onClick.AddListener(() => ActivePause());
    }

    public void ActivePause()
    {
        controller.BlockUI(true);
        frameObject.SetActive(true);
        pauseObject.SetActive(true);
        settingObject.SetActive(false);
    }

    public void ActiveSetting()
    {
        pauseObject.SetActive(false);
        settingObject.SetActive(true);
    }

    #region Pause
    public void ResumeGame()
    {
        controller.BlockUI(false);
        frameObject.SetActive(false);
        pauseObject.SetActive(false);
        pauseObject.SetActive(false);
    }

    public void DecideReturnLobby()
    {
        decideObject.SetActive(true);
        resumeBtn.interactable = false;
        settingBtn.interactable = false;
        LobbyBtn.interactable = false;
    }

    public void DecideYes()
    {
        // 적 오브젝트 전부 파괴
        Time.timeScale = 1f;
        GameManager.Instance.Data.SavePlayerData(GameManager.Instance.Data.CurrentPlayerData);
        LoadingManager.Instance.CallStartLoading(SceneName.Stage);
    }

    public void DecideNo()
    {
        decideObject.SetActive(false);
        resumeBtn.interactable = true;
        settingBtn.interactable = true;
        LobbyBtn.interactable = true;
    }
    #endregion

    #region Setting
    public void SettingOptionChange(int _value)
    {
        LobbySetUI _changeType = (LobbySetUI)_value;
        GameManager.Instance.PDialogue.CurrentSettingUI = _changeType;
    }
    #endregion
}
