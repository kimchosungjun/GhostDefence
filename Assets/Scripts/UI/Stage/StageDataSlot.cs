using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageDataSlot : MonoBehaviour
{
    bool isLock = false;
    [SerializeField] Button stageButton;
    public Button StageButton { get { return stageButton; } }
    [SerializeField, Tooltip("0:Text, 1:LockImage")]GameObject[] stageObject;

    private void Awake()
    {
        if (stageButton == null)
            stageButton = GetComponent<Button>();
    }

    public void Setup(bool _isLock)
    {
        if (_isLock)
        {
            stageObject[0].gameObject.SetActive(false);
            stageObject[1].gameObject.SetActive(true);
        }
        else
        {
            stageObject[0].gameObject.SetActive(true);
            stageObject[1].gameObject.SetActive(false);
        }

        isLock = _isLock;
        SetLockState();
    }

    public void SetLockState()
    {
        if (!isLock)
            return;
        stageButton.interactable = false;
    }
}
