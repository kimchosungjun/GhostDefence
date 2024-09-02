using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField, Tooltip("0:Name, 1:Speaker")] TextMeshProUGUI[] dialogueTexts;
    [SerializeField, Tooltip("0:TextBox, 1:SpaceBar")] GameObject[] dialogueObjects;
    float dialogueSpeed;

    bool isEndLine = false;
    bool isTalking = false;
    int dialogueIndex = 0;
    Dialogue dialogue = null;

    public void Init()
    {
        switch(GameManager.Instance.PDialogue.currentSettingUI)
        {
            case UIEnums.LobbySetUI.Slow:
                dialogueSpeed = 0.2f;
                break;
            case UIEnums.LobbySetUI.Middle:
                dialogueSpeed = 0.1f;
                break;
            case UIEnums.LobbySetUI.Fast:
                dialogueSpeed = 0.05f;
                break;
        }    
    }

    public void StartDialogue(Dialogue _dialogue)
    {
        if (isTalking)
            return;
        isTalking = true;
        dialogueIndex = 0;
        dialogue = _dialogue;
        dialogueTexts[0].text = dialogue.speakerName;
        dialogueTexts[1].text = "";
        dialogueObjects[1].SetActive(false);
        dialogueObjects[0].SetActive(true);
        StartCoroutine(ShowDialogue(dialogue.storyLines[dialogueIndex]));
    }

    IEnumerator ShowDialogue(string _storyLine)
    {
        dialogueObjects[1].SetActive(false);
        dialogueTexts[1].text = "";
        isEndLine = false;

        int _lineCnt = _storyLine.Length;
        string _line = "";

        if (_storyLine[0].Equals('@'))
        {
            // 이벤트 발생
            GameManager.Instance.PDialogue.PDialogueEvent.DispatchDialogueEvent(_storyLine);
            dialogueIndex += 1;
            CheckNextDialogue();
            yield break;
        }
        else
        {
            for (int i = 0; i < _lineCnt; i++)
            {
                _line += _storyLine[i];
                dialogueTexts[1].text = _line;
                yield return new WaitForSeconds(dialogueSpeed);
            }
            dialogueIndex += 1;
            isEndLine = true;
            dialogueObjects[1].SetActive(true);
        }
    }

    #region Key Press Function
    public void DialogueUpdate()
    {
        if (!isTalking)
            return;

        if(isTalking && !isEndLine && Input.GetKeyDown(KeyCode.Space))
        {
            SkipType();
            return;
        }

        if (!isEndLine)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckNextDialogue();
        }
    }

    public void SkipType()
    {
        //StopCoroutine(ShowDialogue(dialogue.storyLines[dialogueIndex]));
        StopAllCoroutines();
        dialogueTexts[1].text = dialogue.storyLines[dialogueIndex];
        dialogueIndex += 1;
        isEndLine = true;
        dialogueObjects[1].SetActive(true);
    }

    public void CheckNextDialogue()
    {
        if (dialogueIndex >= dialogue.storyLines.Count)
        {
            isTalking = false;
            dialogueIndex = 0;
            dialogue = null;
            dialogueObjects[0].SetActive(false);
            GameManager.Instance.PDialogue.EndDialogue();
        }
        else
        {
            StartCoroutine(ShowDialogue(dialogue.storyLines[dialogueIndex]));
        }
    }
    #endregion
}
