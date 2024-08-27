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
        // ������ �����ϴ��� Ȯ���ϱ�
        LoadingManager.Instance.LoadScene(SceneName.Lobby);
    }

    public void RestartGame()
    {
        // ������ �����ϴ��� Ȯ���ϱ�
        LoadingManager.Instance.LoadScene(SceneName.Game);
    }
}
