using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using UIEnums;

public class LobbyUIController : MonoBehaviour
{
    int lobbyUIBtnCnt = 0;
    LobbyUIBtn currentSelect;
    [SerializeField] TitleUI titleUI;

    #region UnityAction
    UnityAction<LobbyUIBtn> selectTextAction;
    UnityAction enterSelectBtnAction;
    UnityAction releaseSelectBtnAction;
    public UnityAction EnterSelectBtnAction { get { return enterSelectBtnAction; } }
    public UnityAction ReleaseSelectBtnAction { get { return releaseSelectBtnAction; } }
    #endregion

    bool isPressSelectBtn;
    [SerializeField] GameObject darkEffectPanel;

    // LobbyUI Group
    [SerializeField, Tooltip("Start, Setting, Exit")] LobbyUI[] lobbyUIGroup;
    [SerializeField] LobbyStartUI startUI =null;

    #region Start
    private void Start()
    {
        Link();
        Init();
    }

    public void Link()
    {   
        if (titleUI == null)
            titleUI = GetComponentInChildren<TitleUI>();
    }

    public void Init()
    {
        lobbyUIBtnCnt = Enums.GetEnumLenth<LobbyUIBtn>();
        currentSelect = LobbyUIBtn.Start;
        isPressSelectBtn = false;

        int lobbyUICnt = lobbyUIGroup.Length;
        for(int idx=0; idx< lobbyUICnt; idx++)
        {
            lobbyUIGroup[idx].UIController = this;
            lobbyUIGroup[idx].Init();
        }

        titleUI.UIController = this;
        titleUI.Init();

        selectTextAction = null;
        selectTextAction += titleUI.LocationSelectIcon;
        selectTextAction += titleUI.ResizeButtonScale;
        selectTextAction(currentSelect);

        enterSelectBtnAction = null;
        enterSelectBtnAction += PressEnterKey;
        enterSelectBtnAction += titleUI.DisableSelectIcon;

        releaseSelectBtnAction = null;
        releaseSelectBtnAction += ReleaseEnterKey;
        releaseSelectBtnAction += titleUI.EnableSelectIcon;
    }
    #endregion

    #region Update
    private void Update()
    {
        if (!isPressSelectBtn)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ControlDirectionKey(-1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ControlDirectionKey(1);
            }
        }
        

        if ((Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) && !isPressSelectBtn)
        {
            enterSelectBtnAction();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isPressSelectBtn)
        {
            releaseSelectBtnAction();
        }
    }

    /// <summary>
    /// 방향키로 키 선택
    /// </summary>
    /// <param name="_value"></param>
    public void ControlDirectionKey(int _value)
    {
        int currentIndex = ((int)currentSelect + _value);
        if (currentIndex < 0)
            currentIndex = lobbyUIBtnCnt - 2;
        else if (currentIndex > 2)
            currentIndex = 0;
        currentSelect = (LobbyUIBtn)currentIndex;
        selectTextAction(currentSelect);
    }

    /// <summary>
    /// 선택한 키를 엔터 키를 누르거나 버튼을 눌렀을 때 호출 : 매개변수는 사용 안함
    /// </summary>
    public void PressEnterKey()
    {
        isPressSelectBtn = true;
        darkEffectPanel.SetActive(true);
        lobbyUIGroup[(int)currentSelect].OnOffUI(true);
    }

    /// <summary>
    /// 버튼을 선택한 뒤에 뒤로가기 버튼을 눌렀을 때 호출 : 매개변수 사용 안함
    /// </summary>
    public void ReleaseEnterKey()
    {
        #region StartUI
        if (currentSelect == LobbyUIBtn.Start)
        {
            if (lobbyUIGroup[0].CanCloseWindow())
            {
                if(startUI==null)
                    startUI =  lobbyUIGroup[0].gameObject.GetComponent<LobbyStartUI>();
                startUI.ClickBackBtn();
                CloseStartUI();
            }
            else
            {
                if (startUI == null)
                    startUI = lobbyUIGroup[0].gameObject.GetComponent<LobbyStartUI>();
                startUI.ClickBackBtn();
            }
            return;
        }
        #endregion

        #region Setting, Exit UI
        isPressSelectBtn = false;
        darkEffectPanel.SetActive(false);
        lobbyUIGroup[(int)currentSelect].OnOffUI(false);
        #endregion
    }

    /// <summary>
    /// 버튼 위에 마우스를 올렸을 때 호출
    /// </summary>
    /// <param name="_selectBtnType"></param>
    public void OnMousePointSelectBtn(LobbyUIBtn _selectBtnType)
    {
        currentSelect = _selectBtnType;
        selectTextAction(currentSelect);
    }
    #endregion

    public void CloseStartUI()
    {
        isPressSelectBtn = false;
        darkEffectPanel.SetActive(false);
        lobbyUIGroup[(int)currentSelect].OnOffUI(false);
    }
}
