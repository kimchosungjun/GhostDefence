using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    enum TutorialUIEnum
    {
        PlayerInfo,
        Timer,
        Tower,
        End
    }

    [SerializeField] TextMeshProUGUI informationText;
    TutorialUIEnum currentTutorial = TutorialUIEnum.PlayerInfo;

    [SerializeField] GameObject tutorialObject;
    [SerializeField, Tooltip("0:플레이어, 1:타이머, 2:타워")] GameObject[] maskObject;
    [SerializeField] Button[] Buttons;

    string playerInfoStr = "성의 내구도, 플레이이어의 재화 그리고 게임을 시작하는 버튼을 확인할 수 있습니다.";
    string timerInfoStr = "해가 뜰 때까지의 시간을 확인할 수 있습니다.";
    string towerInfoStr = "타워 아이콘을 눌러 원하는 곳에 건설할 수 있습니다. \n<color=red>(물과 적이 있는 타일은 제외)";

    public void Init()
    {
        Buttons[0].onClick.AddListener(() => ClickPlayerBtn());
        Buttons[1].onClick.AddListener(() => ClickTimerBtn());
        Buttons[2].onClick.AddListener(() => ClickTowerBtn());
        informationText.text = "";
        tutorialObject.SetActive(false);
    }

    public void OperateTutorial()
    {
        switch (currentTutorial)
        {
            case TutorialUIEnum.PlayerInfo:
                UnityAction _action = null;
                _action += () => tutorialObject.SetActive(true);
                ShowTutorial(playerInfoStr, _action);
                break;
            case TutorialUIEnum.Timer:
                ShowTutorial(timerInfoStr);
                break;
            case TutorialUIEnum.Tower:
                ShowTutorial(towerInfoStr);
                break;
        }
        if (currentTutorial != TutorialUIEnum.End)
            currentTutorial += 1;
    }

    public void ShowTutorial(string _informationText, UnityAction _action = null)
    {
        if(_action!=null)
            _action();
        
        int _curIndex = (int)currentTutorial;
        int _cnt = maskObject.Length;
        for(int i=0; i<_cnt; i++)
        {
            if (i == _curIndex)
            {
                maskObject[i].SetActive(true);
                Buttons[i].gameObject.SetActive(true);
            }
            else
            {
                maskObject[i].SetActive(false);
                Buttons[i].gameObject.SetActive(false);
            }
        }
        informationText.text =_informationText;
    }

    public void ClickPlayerBtn()
    {
        Buttons[0].interactable = false;
        OperateTutorial();
    }

    public void ClickTimerBtn()
    {
        Buttons[1].interactable = false;
        OperateTutorial();
    }

    public void ClickTowerBtn()
    {
        Buttons[2].interactable = false;
        tutorialObject.SetActive(false);
    }
}
