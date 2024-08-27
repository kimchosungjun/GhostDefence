using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySceneController : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.Data.CurrentPlayerData = null;
    }
}
