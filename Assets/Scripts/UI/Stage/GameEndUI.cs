using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndUI : MonoBehaviour
{
    [SerializeField] Button winBtn;
    [SerializeField, Tooltip("0:Lobby, 1:Restart")] Button[] loseBtn;
    public void Init()
    {
        winBtn.onClick.AddListener(() => ReturnLobby());
        loseBtn[0].onClick.AddListener(() => ReturnLobby());
        loseBtn[1].onClick.AddListener(() => RestartGame());
    }

    public void ReturnLobby()
    {
        // 데이터 리셋하는지 확인하기
        LoadingManager.Instance.LoadScene(SceneName.Lobby);
    }

    public void RestartGame()
    {
        // 데이터 리셋하는지 확인하기
        LoadingManager.Instance.LoadScene(SceneName.Game);
    }
}
