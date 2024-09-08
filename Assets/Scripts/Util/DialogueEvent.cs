using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvent : MonoBehaviour
{
    DialogueUI dialogueUI = null;

    public void DispatchDialogueEvent(string _line, DialogueUI _dialogueUI)
    {
        if (dialogueUI == null)
            dialogueUI = _dialogueUI;

        int _lineCnt = _line.Length;
        string _functionName = "";
        List<string> _parameters = new List<string>();
        for(int i=1; i < _lineCnt; i++)
        {
            if (_line[i].Equals('<'))
            {
                _parameters = GetParameter(i+1, _line);
                break;
            }
            _functionName += _line[i];
        }

        OperateEvent(_functionName, _parameters);
    }

    public List<string> GetParameter(int _index, string _line)
    {
        int _maxCnt = _line.Length;
        List<string> _parameters = new List<string>();
        string _parameter = "";
        for(int i = _index; i<_maxCnt; i++)
        {
            if (_line[i].Equals(','))
            {
                _parameters.Add(_parameter);
                _parameter = "";
            }
            else if (_line[i].Equals('>'))
            {
                _parameters.Add(_parameter);
                break;
            }
            else
            {
                _parameter += _line[i];
            }
        }
        return _parameters;
    }

    public void OperateEvent(string _functionName, List<string> _parameters)
    {
        switch (_functionName)
        {
            case "Tutorial":
                GameManager.Instance.PDialogue.UIController.PTutorial.OperateTutorial();
                break;
            case "Character":
                int _index = int.Parse(_parameters[0]);
                dialogueUI.ChangeImage(_index);
                break;
        }
    }
}
