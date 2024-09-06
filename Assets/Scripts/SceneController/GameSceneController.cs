using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneController : BaseSceneController
{
    [SerializeField] LayerMask tileLayer;
    [SerializeField] PoolManager poolManager;
    [SerializeField] CameraBounds camBounds;
    StageData stageData;
    SummonData summonData;
    public StageData Data { get => stageData; }

    [SerializeField] GameSceneUIController uiController;
    public GameSceneUIController UIController { get { return uiController; } }

    Camera mainCam =null;

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

        // Ÿ�� ���� �޾ƿ���
        LinkNodes();
    }
    #endregion

    #region ���� ���� �� ȣ��
    private void Start()
    {
        // ��ȭ ����  
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

    #region ���� ���� �� ��� ȣ��
    private void Update()
    {
        //#region Ÿ�Ͽ� ���콺 Ŭ��
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit rayHit;
        //    bool rayCastHit = Physics.Raycast(ray, out rayHit, 100f, tileLayer);

        //    if (rayHit.collider != null)
        //        GetNode(rayHit.collider);
        //    else
        //        Debug.Log("�浹ü�� �������� ����");
        //}
        //#endregion

        //#region Ÿ�� ����
        //if (CurrentNode != null)
        //{

        //}
        //#endregion
    }
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
    #region Ÿ�� �˻�
    
    #endregion

    #region �޼���
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
    #endregion


}
