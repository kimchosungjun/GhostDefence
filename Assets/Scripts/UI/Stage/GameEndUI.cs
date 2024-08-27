using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndUI : MonoBehaviour
{
    [SerializeField, Tooltip("0:Win, 1:Lose")] GameObject[] endUIObject;

    [SerializeField] Button winBtn;
    [SerializeField, Tooltip("0:Lobby, 1:Restart")] Button[] loseBtn;
    public void Init()
    {
        endUIObject[0].SetActive(false);
        endUIObject[1].SetActive(false);

        winBtn.onClick.AddListener(() => ReturnLobby());
        loseBtn[0].onClick.AddListener(() => ReturnLobby());
        loseBtn[1].onClick.AddListener(() => RestartGame());
    }

    public void ReturnLobby()
    {
        Time.timeScale = 1f;
        GameManager.Instance.Data.SavePlayerData(GameManager.Instance.Data.CurrentPlayerData);
        LoadingManager.Instance.CallStartLoading(SceneName.Stage);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        LoadingManager.Instance.CallStartLoading(SceneName.Game);
    }

    public void WinGame()
    {
        endUIObject[0].SetActive(true);
        GameManager.Instance.Data.CurrentPlayerData.WinStage(GameManager.Instance.Data.CurrentStageData.StageID);
    }

    public void LoseGame()
    {
        endUIObject[1].SetActive(true);
    }
}
