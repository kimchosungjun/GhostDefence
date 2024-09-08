using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSceneUIController : MonoBehaviour
{
    void Awake()
    {
        #region Link Button
        startBtn.onClick.AddListener(() => StartGame());
        fastBtn.onClick.AddListener(() => FastGame());
        startBtn.interactable = true;
        fastBtn.interactable = false; 
        startImgae.color = activeColor;
        fastImage.color = inactiveColor;
        pauseBtn.onClick.AddListener(() => PauseGame());
        pauseExitBtn.onClick.AddListener(() => ResumeGame());
        #endregion

        #region Init UI
        endUI.Init();
        dialogueUI.Init();
        PTutorial.Init();
        pauseUI.Init(this);
        towerSetUI.Init();
        towerUpgradeUI.Init();
        gameEndingUI.Init();
        #endregion
    }

    void Update()
    {
        if(!isPause)
            dialogueUI.DialogueUpdate();

        if (isFollowCursor && !isPause)
        {
            Vector2 mousePosition = Input.mousePosition;
            indicateBuildTransform.position = mousePosition;
        }
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
    bool isStartGame = false;
    [Header("게임 시작")]
    [SerializeField] Color activeColor;
    [SerializeField] Color inactiveColor;
    [SerializeField] TextMeshProUGUI fastText;
    [SerializeField] Button startBtn;
    [SerializeField] Button fastBtn;
    [SerializeField] Image startImgae;
    [SerializeField] Image fastImage;
    public void StartGame()
    {
        if (isStartGame == false)
        {
            isStartGame = true;
            GameManager.Instance.GameSystem.StartGame();
        }
        else
            Time.timeScale = 1f;
        fastText.text = "X1";
        fastBtn.interactable = true;
        startBtn.interactable = false;
        startImgae.color = inactiveColor;
        fastImage.color = activeColor;
    }

    public void FastGame()
    {
        Time.timeScale = 2f;
        fastText.text = "X2";
        fastBtn.interactable = false;
        startBtn.interactable = true;
        startImgae.color = activeColor;
        fastImage.color = inactiveColor;
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

    #region TowerMouse
    bool isFollowCursor =false;
    [SerializeField] RectTransform indicateBuildTransform;

    public void IndicateBuildCursor(bool _isOn)
    {
        isFollowCursor = _isOn;
        indicateBuildTransform.gameObject.SetActive(_isOn);
    }
    #endregion

    #region GameEnding
    [SerializeField] GameEndingUI gameEndingUI;
    public GameEndingUI GameEnding { get { return gameEndingUI; } }
    #endregion
}
