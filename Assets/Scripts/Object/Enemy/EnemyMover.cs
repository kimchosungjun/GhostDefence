using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyFeature))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] int enemyID;
    [SerializeField] EnemyFeature enemyFeature;

    EnemyData enemyData = null;
    List<NodeData> path = new List<NodeData>();
    GridManager gridManager;

    public float Speed { get; set; } = 0;

    #region Unity Life Cycle
    private void Awake()
    {
        #region Enemy Data
        if (enemyData == null)
        {
            if(GameManager.Instance.Data.GetEnemyData(enemyID)!=null)
                enemyData = new EnemyData(GameManager.Instance.Data.GetEnemyData(enemyID));
        }
        if (enemyData == null)
            Debug.LogError("적 데이터를 찾을 수 없다..");

        if (enemyFeature == null)
            enemyFeature = GetComponent<EnemyFeature>();
        if(enemyFeature!=null && enemyData!=null)
            enemyFeature.Init(enemyData,this);
        else
            Debug.LogError("초기화 실패!!!!");
        #endregion

        gridManager = GameManager.Grid;
        Speed = enemyData.Speed;
    }
    #endregion

    public void SummonEnemy()
    {
        gameObject.SetActive(true);
        enemyFeature.ResetStat();
        CalculatePath();
    }

    public void CalculatePath()
    {
        // 시작지점과 목적지 설정 후, 경로 재탐색
        NodeData _startNode = CurrentOnNodeData();
        if (_startNode == null)
        {
            Debug.LogError("시작점을 찾지 못했습니다! : 에러 발생!!");
            return;
        }
        NodeData _endNode = gridManager.GetNodeData(gridManager.EndCoordinate);
        path = gridManager.PathFinder.GetPath(_startNode, _endNode);
        
        // 같지 않다면 이미 출발한 상태, 마지막 노드에 있는 중이라면 계속 이동하게 만듬
        Vector2Int _currentPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        Vector2Int _startPos = new Vector2Int((int)_startNode.coordinates.x, (int)_startNode.coordinates.z);
        if (_currentPos != gridManager.StartCoordinate && _startPos != gridManager.EndCoordinate)
            StopCoroutine(FollowPath());

        // 경로 따라 움직이기
        if (path != null)
            StartCoroutine(FollowPath());
    }

    public IEnumerator FollowPath()
    {
        // 이동속도 이용하는 방법
        int pathCnt = path.Count;
        for (int idx=0; idx< pathCnt; idx++)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = path[idx].coordinates;
            float movePercent = 0f;

            #region Immediately Rotate
            Vector3 direction = path[idx].coordinates - transform.position;
            transform.rotation = Quaternion.LookRotation(direction);
            #endregion

            while (movePercent < 1f)
            {
                movePercent += Time.deltaTime * Speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, movePercent);
                #region Smooth Rotate
                // 회전에선 선형보간인 Lerp보단 구형보간인 Slerp가 좀 더 부드러움
                //Vector3 _direction = path[idx].coordinates - transform.position;
                //Quaternion _rotateValue = Quaternion.LookRotation(_direction);
                //transform.rotation = Quaternion.Slerp(transform.rotation, _rotateValue, movePercent);
                #endregion
                yield return null;
            }
        }
        ArriveDestination();
    }

    public void ArriveDestination()
    {
        GameManager.Instance.GameSystem.HitByEnemy(enemyData.Damage);
        enemyFeature.Dissolve();
    }


    #region Check Current Under Tile : Recalculate Path
    [Header("Tile RayCast")]
    [SerializeField, Range(3f, 5f)] float detectTileDistance =4f;
    [SerializeField] LayerMask tileLayer;
    public NodeData CurrentOnNodeData()
    {
        RaycastHit _hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f , Vector3.down, out _hit, detectTileDistance, tileLayer))
        {
            TileNode _tileNode = _hit.collider.GetComponentInParent<TileNode>();
             if (_tileNode == null)
                return null;
            return GameManager.Grid.GetNodeData(_tileNode.Coordinate);
        }
        return null;
    }
    #endregion
}
