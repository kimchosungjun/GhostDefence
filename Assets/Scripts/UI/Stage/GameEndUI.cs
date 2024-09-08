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
        GameManager.Instance.Data.CurrentPlayerData.WinStage(GameManager.Instance.Data.CurrentStageData.StageID);
        if (GameManager.Instance.Data.CurrentPlayerData.isClearAll)
        {
            // 전부 클리어했을 때, 엔딩
            if (GameManager.Instance.Data.CurrentPlayerData.isShowEnding)
            {
                // 이미 엔딩을 본 상태
                endUIObject[0].SetActive(true);
            }
            else
            {
                // 엔딩을 보지 못한 상태
                GameManager.Instance.GameSystem.GameController.UIController.GameEnding.EndingStart();  
            }
        }
        else
        {
            // 전부 클리어하지 못함
            endUIObject[0].SetActive(true);
        }
    }

    public void LoseGame()
    {
        endUIObject[1].SetActive(true);
    }
}
