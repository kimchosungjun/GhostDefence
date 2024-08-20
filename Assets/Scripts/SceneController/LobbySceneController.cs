using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySceneController : MonoBehaviour
{
    [SerializeField] LobbyStartUI startUI;
 
    private void Start()
    {
        GameManager.Instance.Data.ClearDataManager();
    }
}
