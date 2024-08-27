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
    }

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
    bool isPause = false;
    public void PauseGame()
    {
        isPause = true;
        pauseBtn.interactable = false;
        GameManager.Instance.GameSystem.StopGame();
        // pause ui 활성
    }

    public void ResumeGame()
    {
        isPause = false;
        pauseBtn.interactable = true;
        GameManager.Instance.GameSystem.ResumeGame();
        // pause ui 비활성
    }

    public void PreessEscape()
    {
        if (isPause)
            ResumeGame();
        else
            PauseGame();

        isPause = !isPause;
    }

    #endregion

    #region Game End
    
    public void WinGame()
    {

    }

    public void LoseGame()
    {

    }

    #endregion
}
