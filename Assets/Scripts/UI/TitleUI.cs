using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UIEnums;

public class TitleUI : UI
{
    [SerializeField, Tooltip("Start, Setting, Exit, Icon")] RectTransform[] textRects;
    [SerializeField, Tooltip("Start, Setting, Exit")] Button[] textBtns;
    [SerializeField, Tooltip("Start, Setting, Exit")] TitleSelectIconButtonScale[] selectButtonScaleGroup;
    Image selectIconImage;
    Animator selectIconAnim;

    public LobbyUIController UIController { get; set; } = null;

    public override void Init()
    {
        selectIconImage = textRects[(int)LobbyUIBtn.SelectIcon].GetComponent<Image>();
        selectIconAnim = textRects[(int)LobbyUIBtn.SelectIcon].GetComponent<Animator>();

        textBtns[(int)LobbyUIBtn.Start].onClick.AddListener(() => { UIController.EnterSelectBtnAction(); });
        textBtns[(int)LobbyUIBtn.Setting].onClick.AddListener(() => { UIController.EnterSelectBtnAction(); });
        textBtns[(int)LobbyUIBtn.Exit].onClick.AddListener(() => { UIController.EnterSelectBtnAction(); });
    }

    public void LocationSelectIcon(LobbyUIBtn _currentSelect)
    {
        Vector2 iconPosition = textRects[(int)_currentSelect].anchoredPosition;
        //iconPosition.x -= textRects[(int)LobbyUIBtn.SelectIcon].rect.width;
        iconPosition.x -= 90;
        textRects[(int)LobbyUIBtn.SelectIcon].anchoredPosition = iconPosition;
    }

    public void DisableSelectIcon()
    {
        selectIconAnim.enabled = false;
        Color defaultColor = selectIconImage.color;
        defaultColor.a = 1f;
        selectIconImage.color = defaultColor;
    }

    public void EnableSelectIcon()
    {
        selectIconAnim.enabled = true; 
    }

    public void ResizeButtonScale(LobbyUIBtn _currentState)
    {
        int _currentStateIndex = (int)_currentState;
        int _cnt = (int)LobbyUIBtn.SelectIcon;
        for (int idx=0; idx< _cnt; idx++)
        {
            if(idx==_currentStateIndex)
                selectButtonScaleGroup[idx].ResizeButtonScale(true);
            else
                selectButtonScaleGroup[idx].ResizeButtonScale(false);
        }
    }
}
