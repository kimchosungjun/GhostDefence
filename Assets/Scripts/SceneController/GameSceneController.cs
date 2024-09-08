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

    #region �ʱ�ȭ
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

    #region ���� ���� �� ȣ��
    private void Start()
    {
        // ��ȭ ����  
        if (GameManager.Instance.Data.CurrentPlayerData.isClearAll)
            return;
        if (GameManager.Instance.Data.CurrentStageData.StageID >= GameManager.Instance.Data.CurrentPlayerData.clearStage)
            GameManager.Instance.PDialogue.StartDialogue(Data.StageID);
    }
    #endregion

    #region ���� ���� �� ��� ȣ��
    private void Update()
    {
        #region �ͷ� ��ġ
        // �� Ŭ���ϸ� ���׷��̵� �ʱ�ȭ
        if (isBuildState)
        {
            if (Input.GetMouseButtonDown(1))
            {
                // ���
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

            // �ͷ��� ������ => SelectTurret �ʱ�ȭ => BuildTurret�� �ʱ�ȭ���� ���(�ѹ��� ������ ���� �� �ְ� �ұ� �����)
            if (Input.GetMouseButtonDown(0))
            {
                if (!_isOnTile)
                {
                    CreateTurret(_tileNode.BuildPosition, _tileNode);
                    _tileNode.BuildTurretOnTile();
                    // ���鿡�� �ٽ� ��� Ž���ϵ��� ����
                    poolManager.AnnounceChangePath();
                }
            }
        }
        else
        {
            // �ͷ��� ������ ���� ���׷��̵� â�� �ڵ����� �ʱ�ȭ��
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

        // Ÿ�� �Ǽ� ��, ���� ���ٸ� ���
        if (!GameManager.Instance.GameSystem.CanUse(BuildTurret.ScriptableTowerData.costMoney))
            BuildTurret = null;
    }

    // TowerUpgrade�� ��������, ��ġ�� ���ÿ� ���׷��̵� Ÿ���� ����
    bool isBuildState = false;
    private Turret buildTurret = null;
    public Turret BuildTurret { get { return buildTurret; } set { buildTurret = value; ChangeBuildTurret(); } }

    public void ChangeBuildTurret()
    {
        if (buildTurret == null)
        {
            isBuildState = false;
            // ���콺 Ŀ�� �̹��� ����
            GameManager.Instance.MouseCursor.IsBuildCursor = false;
            // ���� �� �ִ��� Ȯ�ν�Ű�� UI OFF
            uiController.IndicateBuildCursor(false);
            selectTile.gameObject.SetActive(false);
        }
        else
        {
            isBuildState = true;
            // ���콺 Ŀ�� �̹��� ����
            GameManager.Instance.MouseCursor.IsBuildCursor = true;
            // �ͷ� ���� �� �ִ��� Ȯ���ϰ� ���ִ� UI�� ����ٴ�
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

    #region �޼���
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
