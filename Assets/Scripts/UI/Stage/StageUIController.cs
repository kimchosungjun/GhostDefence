using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUIController : MonoBehaviour
{
    PlayerData playerData;
    [SerializeField] Button backButton;
    [SerializeField] StageDataSlot[] stageDataSlots;

    private void Awake()
    {
        if (stageDataSlots.Length == 0)
            stageDataSlots = GetComponentsInChildren<StageDataSlot>();
    }

    private void Start()
    {
        if (backButton != null)
            backButton.onClick.AddListener(() => { SceneLoadManager.Instance.LoadScene(SceneName.Lobby); 
                GameManager.Instance.Data.CurrentPlayerData = null; } );
        playerData = GameManager.Instance.Data.CurrentPlayerData;

        int _clearStage = playerData.clearStage;
        int _stageDataSlotCnt = stageDataSlots.Length;
        for(int idx= 0; idx<_stageDataSlotCnt; idx++)
        {
            if (_clearStage < idx)
                stageDataSlots[idx].Setup(true);
            else
                stageDataSlots[idx].Setup(false);
            stageDataSlots[idx].StageButton.onClick.AddListener(() => { SetStageData(idx); SceneLoadManager.Instance.LoadScene(SceneName.Game); } );
        }
    }

    public void SetStageData(int _idx)
    {
        if (GameManager.Instance.Data.GetStageData(_idx) == null)
            return;
        GameManager.Instance.Data.CurrentStageData = GameManager.Instance.Data.GetStageData(_idx);
    }
}
