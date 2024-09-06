using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneController : BaseSceneController
{
    int tileLayer = 1<<(int)DefineLayer.Tile;
    [SerializeField] PoolManager poolManager;
    [SerializeField] CameraBounds camBounds;
    StageData stageData;
    SummonData summonData;
    public StageData Data { get => stageData; }

    [SerializeField] GameSceneUIController uiController;
    public GameSceneUIController UIController { get { return uiController; } }

    Camera mainCam =null;

    #region 초기화
    private void Awake()
    {
        mainCam = Camera.main;
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
        for (int idx = 0; idx < _summonEnemyCnt; idx++)
        {
            EnemyData _enemyData = GameManager.Instance.Data.GetEnemyData(summonData.enemyID[idx]);
            _enemyDataList.Add(_enemyData);
        }
        poolManager.Init(_enemyDataList);

        // 타일 정보 받아오기
        LinkNodes();
    }
    #endregion

    #region 게임 시작 시 호출
    private void Start()
    {
        // 대화 시작  
        GameManager.Instance.PDialogue.StartDialogue(Data.StageID);
    }

    public void LinkNodes()
    {
        GameObject _map = GameObject.FindWithTag("Map");
        TileNode[] _tileNodes = _map.GetComponentsInChildren<TileNode>();

        int _tileCnt = _tileNodes.Length;
        for (int i = 0; i < _tileCnt; i++)
        {
            tileGroup.Add(_tileNodes[i].name, _tileNodes[i]);
        }
    }
    #endregion

    #region 게임 진행 중 계속 호출
    private void Update()
    {
        #region 터렛 배치
        if (CurrentTurret != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            bool rayCastHit = Physics.Raycast(ray, out rayHit, 100f, tileLayer);
            if (rayCastHit)
                GetNode(rayHit.collider);
        }
        #endregion
    }

    public Turret CurrentTurret { get; set; } = null;

    #endregion

    public bool IsSelectTower { get; set; } = false;
    public TileNode CurrentNode { get; set; } = null;
    Dictionary<string, TileNode> tileGroup = new Dictionary<string, TileNode>();

    public void GetNode(Collider _collider)
    {
        if (tileGroup.ContainsKey(_collider.name))
        {
            TileNode _selectNode = tileGroup[_collider.name];
            if (_selectNode == CurrentNode)
                CurrentNode = null;
            else
                CurrentNode = _selectNode;
        }
        else
        {
            CurrentNode = null;
        }
    }
    #region 타일 검색
    
    #endregion

    #region 메서드
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

    public Projectile SummonProjectile(Projectile _projectile)
    {
        return poolManager.SummonProjectile(_projectile);
    }
    #endregion


}
