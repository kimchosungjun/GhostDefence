using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSceneUIController : MonoBehaviour
{
    void Awake()
    {
        startBtn.onClick.AddListener(() => StartGame());
        pauseBtn.onClick.AddListener(() => PauseGame());
        pauseExitBtn.onClick.AddListener(() => ResumeGame());
        endUI.Init();
        dialogueUI.Init();
        PTutorial.Init();
        pauseUI.Init(this);
        towerSetUI.Init();
    }

    void Update()
    {
        if(!isPause)
            dialogueUI.DialogueUpdate();
    }

    #region Block UI
    [SerializeField] GameObject blockObject;
    public void BlockUI(bool _isBlock)
    {
        blockObject.SetActive(_isBlock);
    }
    #endregion

    #region HP
    [SerializeField] TextMeshProUGUI hpText;
    public void UpdateHPValue(int _hp)
    {
        if (_hp < 0)
            _hp = 0;
        hpText.text = _hp.ToString("00");
    }
    #endregion

    #region Energy
    [SerializeField] TextMeshProUGUI energyText;
    public void UpdateEnergyValue(int _energy)
    {
        energyText.text = _energy.ToString();
    }
    #endregion

    #region Timer
    [SerializeField] TextMeshProUGUI timerText;
    public void UpdateTimeValue(float _timer)
    {
        int _min = (int)_timer / 60;
        int _sec = (int)_timer % 60;

        string _minText = (_min <= 0) ? "00" : "0" + _min.ToString();
        string _secText = (_sec < 10) ? "0" + _sec.ToString() : _sec.ToString();
        timerText.text = _minText + " : " + _secText;
    }
    #endregion

    #region Game Control
    [Header("게임 시작")]
    [SerializeField] Button startBtn;
    public void StartGame()
    {
        GameManager.Instance.GameSystem.StartGame();
        startBtn.interactable = false;
    }

    [Header("게임 정지")]
    [SerializeField] Button pauseBtn;
    [SerializeField] Button pauseExitBtn;
    [SerializeField] PauseUI pauseUI;
    bool isPause = false;
    public void PauseGame()
    {
        isPause = true;
        pauseBtn.interactable = false;
        GameManager.Instance.GameSystem.StopGame();
        pauseUI.ActivePause();
    }

    public void ResumeGame()
    {
        isPause = false;
        pauseBtn.interactable = true;
        GameManager.Instance.GameSystem.ResumeGame();
        pauseUI.ResumeGame();
    }

    #endregion

    #region Game End
    [SerializeField] GameEndUI endUI;
    public GameEndUI EndUI { get { return endUI; } }
    public void WinGame()
    {
        endUI.WinGame();
    }

    public void LoseGame()
    {
        endUI.LoseGame();
    }

    #endregion

    #region Dialogue
    [SerializeField] DialogueUI dialogueUI;
    public DialogueUI PDialogueUI { get { if (dialogueUI == null) dialogueUI.GetComponentInChildren<DialogueUI>(); return dialogueUI; } }
    #endregion

    #region Tutorial
    [SerializeField] TutorialUI tutorial;
    public TutorialUI PTutorial { get { if (tutorial == null) tutorial = GetComponentInChildren<TutorialUI>(); return tutorial; } }
    #endregion

    #region TowerSet UI
    [SerializeField] TowerSetUI towerSetUI;
    public TowerSetUI TowerSet { get { return towerSetUI; } }
    #endregion

    #region TowerUpgradeUI
    [SerializeField] TowerUpgradeUI towerUpgradeUI;
    public TowerUpgradeUI TowerUpgrade { get { return towerUpgradeUI; } }
    #endregion
}
