using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneController : BaseSceneController
{
    [SerializeField] PoolManager poolManager;
    [SerializeField] CameraBounds camBounds;
    StageData stageData;
    SummonData summonData;
    public StageData Data { get => stageData; }

    [SerializeField] GameSceneUIController uiController;
    public GameSceneUIController UIController { get { return uiController; } }
    private void Awake()
    {
        camBounds.Init();
        InitScene();
    }

    public override void InitScene()
    {
        stageData = GameManager.Instance.Data.CurrentStageData;
        GameManager.Instance.GameSystem.InitGameStage(stageData, this);
        summonData = GameManager.Instance.Data.GetSummonData(stageData.StageID);

        List<EnemyData> _enemyDataList = new List<EnemyData>();
        int _summonEnemyCnt = summonData.enemyID.Length;
        for(int idx= 0; idx<_summonEnemyCnt; idx++)
        {
            EnemyData _enemyData = GameManager.Instance.Data.GetEnemyData(summonData.enemyID[idx]);
            _enemyDataList.Add(_enemyData);
        }
        poolManager.Init(_enemyDataList);
    }

    public void SummonEnemy(int _enemyID)
    {
        poolManager.SummonEnemy(_enemyID);
    }

    public void EndGame(bool _isWin)
    {
        if (_isWin)
            uiController.EndUI.WinGame();
        else
            uiController.EndUI.LoseGame();
        poolManager.ClearAllEnemy();
    }
}
