using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameSceneController : BaseSceneController
{
    [SerializeField] SelectTile selectTile;
    [SerializeField] Transform turretGroup;

    int tileLayer = 1 << (int)DefineLayer.Tile;
    int turretLayer = 1 << (int)DefineLayer.Turret;
    [SerializeField] PoolManager poolManager;
    public PoolManager Pool { get { return poolManager; } }
    [SerializeField] CameraBounds camBounds;
    StageData stageData;
    SummonData summonData;
    public StageData Data { get => stageData; }

    [SerializeField] GameSceneUIController uiController;
    public GameSceneUIController UIController { get { return uiController; } }

    Camera mainCam = null;

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
    }
    #endregion

    #region 게임 시작 시 호출
    private void Start()
    {
        // 대화 시작  
        if (GameManager.Instance.Data.CurrentPlayerData.isClearAll)
            return;
        if (GameManager.Instance.Data.CurrentStageData.StageID >= GameManager.Instance.Data.CurrentPlayerData.clearStage)
            GameManager.Instance.PDialogue.StartDialogue(Data.StageID);
    }
    #endregion

    #region 게임 진행 중 계속 호출
    private void Update()
    {
        #region 터렛 배치
        // 땅 클릭하면 업그레이드 초기화
        if (isBuildState)
        {
            if (Input.GetMouseButtonDown(1))
            {
                // 취소
                BuildTurret = null;
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            bool rayCastHit = Physics.Raycast(ray, out rayHit, 100f, tileLayer);
            if (!rayCastHit)
                return;

            TileNode _tileNode = GameManager.Grid.GetTileNode(rayHit.collider.name);
            if (_tileNode == null)
                return;

            bool _isOnTile = _tileNode.SenseOnTile();
            selectTile.UpdateTile(_tileNode.BuildPosition, !_isOnTile);

            // 터렛을 지으면 => SelectTurret 초기화 => BuildTurret은 초기화할지 고민(한번에 여러개 만들 수 있게 할까 고민중)
            if (Input.GetMouseButtonDown(0))
            {
                if (!_isOnTile)
                {
                    CreateTurret(_tileNode.BuildPosition, _tileNode);
                    _tileNode.BuildTurretOnTile();
                    // 적들에게 다시 경로 탐색하도록 만듬
                    poolManager.AnnounceChangePath();
                }
            }
        }
        else
        {
            // 터렛을 누르면 옆의 업그레이드 창이 자동으로 초기화됨
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit rayHit;
                bool rayCastHit = Physics.Raycast(ray, out rayHit, 100f, turretLayer);

                if (rayCastHit)
                    SelectTurret = rayHit.collider.GetComponent<Turret>();
                else
                    SelectTurret = null;
            }
        }
        #endregion
    }

    public void CreateTurret(Vector3 _createPos, TileNode _tileNode)
    {
        GameObject _newTurretObject = Instantiate(BuildTurret.gameObject, _createPos, Quaternion.identity, turretGroup);
        Turret _newTurret = _newTurretObject.GetComponent<Turret>();
        _newTurret.UnderTurretTileNode = _tileNode;
        GameManager.Instance.GameSystem.UseMoney(BuildTurret.ScriptableTowerData.costMoney);

        // 타워 건설 후, 돈이 없다면 취소
        if (!GameManager.Instance.GameSystem.CanUse(BuildTurret.ScriptableTowerData.costMoney))
            BuildTurret = null;
    }

    // TowerUpgrade에 정보전달, 설치와 동시에 업그레이드 타워로 지정
    bool isBuildState = false;
    private Turret buildTurret = null;
    public Turret BuildTurret { get { return buildTurret; } set { buildTurret = value; ChangeBuildTurret(); } }

    public void ChangeBuildTurret()
    {
        if (buildTurret == null)
        {
            isBuildState = false;
            // 마우스 커서 이미지 변경
            GameManager.Instance.MouseCursor.IsBuildCursor = false;
            // 지을 수 있는지 확인시키는 UI OFF
            uiController.IndicateBuildCursor(false);
            selectTile.gameObject.SetActive(false);
        }
        else
        {
            isBuildState = true;
            // 마우스 커서 이미지 변경
            GameManager.Instance.MouseCursor.IsBuildCursor = true;
            // 터렛 지을 수 있는지 확인하게 해주는 UI가 따라다님
            uiController.IndicateBuildCursor(true);
            SelectTurret = null;
            selectTile.gameObject.SetActive(true);
        }
    }

    private Turret selectTurret = null;
    public Turret SelectTurret { get { return selectTurret; } set { ChangeSelectTurret(value); selectTurret = value; } }

    public void ChangeSelectTurret(Turret _selectTurret)
    {
        if (_selectTurret == null)
        {
            uiController.TowerUpgrade.InitialText();
        }
        else
        {
            if (_selectTurret == selectTurret)
                return;
            uiController.TowerUpgrade.UpdateTower(_selectTurret);
        }
    }
    #endregion

    #region 메서드
    public void SummonEnemy(int _enemyID)
    {
        poolManager.SummonEnemy(_enemyID);
    }

    public void EndGame(bool _isWin)
    {
        Time.timeScale = 0f;
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

    [Header("Skybox")]
    [SerializeField] Material nightSkyBox;

    public void ChangeNight()
    {
        if (nightSkyBox != null)
        {
            RenderSettings.skybox = nightSkyBox;
        }
    }
}
