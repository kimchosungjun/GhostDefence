using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageDataSlot : MonoBehaviour
{
    bool isLock = false;
    [SerializeField, Tooltip("StageID")] int stageIndex;
    [SerializeField] Button stageButton;
    [SerializeField, Tooltip("0:Text, 1:LockImage")]GameObject[] stageObject;

    private void Awake()
    {
        if (stageButton == null)
            stageButton = GetComponent<Button>();
        stageButton.onClick.AddListener(() => { SetStageData(); LoadingManager.Instance.CallStartLoading(SceneName.Game); });
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

    public void SetStageData()
    {
        if (GameManager.Instance.Data.GetStageData(stageIndex) == null)
        {
            Debug.Log("없습니다.");
            return;
        }
        GameManager.Instance.Data.CurrentStageData = GameManager.Instance.Data.GetStageData(stageIndex);
        GameManager.Grid.LoadStartEndPoint(GameManager.Instance.Data.CurrentStageData);
    }
}
