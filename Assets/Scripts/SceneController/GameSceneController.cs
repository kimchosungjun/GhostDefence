using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneController : BaseSceneController
{
    [SerializeField] GameStageType currentStage; // 없어도 되는 변수
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
        int _currentStageID = Enums.GetIntValue<GameStageType>(currentStage);
        stageData =  GameManager.Instance.Data.GetStageData(_currentStageID);
        GameManager.Instance.GameSystem.InitGameStage(stageData, this);
        summonData = GameManager.Instance.Data.GetSummonData(_currentStageID);

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


}
