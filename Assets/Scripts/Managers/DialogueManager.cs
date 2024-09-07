using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIEnums; 

public class DialogueManager : MonoBehaviour
{
    GameSceneUIController gameSceneUIController;
    public GameSceneUIController UIController { get { if (gameSceneUIController == null) gameSceneUIController = GameObject.FindWithTag("GameSceneUI").GetComponent<GameSceneUIController>(); return gameSceneUIController; } }

    [SerializeField] DialogueEvent dialogueEvent;
    public DialogueEvent PDialogueEvent { get { return dialogueEvent; } }

    Dialogue dialogue = null;

    LobbySetUI currentSettingUI = LobbySetUI.Middle; 
    public LobbySetUI CurrentSettingUI { get { return currentSettingUI; } set { currentSettingUI = value; DispatchDialogueSpeed(); } }

    public void StartDialogue(int _stroyID)
    {
        if (dialogue != null)
            return;
        dialogue = GameManager.Instance.Data.GetDialogueData(_stroyID);
        UIController.PDialogueUI.StartDialogue(dialogue);
    }

    public void EndDialogue()
    {
        dialogue = null;
    }

    public void DispatchDialogueSpeed()
    {
        if (LoadingManager.Instance.CurrentScene != SceneName.Game)
            return;
        UIController.PDialogueUI.SetDialogueSpeed(currentSettingUI);
    }
}
