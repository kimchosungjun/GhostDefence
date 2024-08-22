using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneController : BaseSceneController
{
    [SerializeField] GameStageType currentStage;
    [SerializeField] PoolManager poolManager;
    [SerializeField] CameraBounds camBounds;
    StageData stageData;
    SummonData summonData;
    public StageData Data { get => stageData; }

    GridManager gridManager;
    private void Awake()
    {
        gridManager = GameManager.Grid;
        camBounds.Init();
        InitScene();


    }

    bool once = true;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && once)
        {
            InitScene();
            once = false;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            poolManager.SummonEnemy(0);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            List<NodeData> nodePath = gridManager.PathFinder.GetPath(gridManager.GetNodeData(gridManager.StartCoordinate), gridManager.GetNodeData(gridManager.EndCoordinate));
            DebugPath(nodePath);
        }
    }

    public override void InitScene()
    {
        int _currentStageID = Enums.GetIntValue<GameStageType>(currentStage);
        stageData =  GameManager.Instance.Data.GetStageData(_currentStageID);
        GameManager.GameSystem.InitGameStage(stageData);
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

    public void DebugPath(List<NodeData> nodePath)
    {
        for(int i=0; i<nodePath.Count; i++)
        {
            Debug.Log(nodePath[i].coordinates);
        }
    }
}
