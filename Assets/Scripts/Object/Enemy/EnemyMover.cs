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

    private Coroutine followPathCoroutine = null;
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
        // 이미 도착지에 있으면 재탐색 안함
        if (_startNode == _endNode)
            return;
        if(followPathCoroutine!=null)
            StopCoroutine(followPathCoroutine);

        if (path.Count > 0)
            path.Clear();
        path = gridManager.PathFinder.GetPath(_startNode, _endNode);

        // 경로 따라 움직이기
        if (path == null || path.Count == 0)
            return;
        followPathCoroutine =StartCoroutine(FollowPath());
    }

    public IEnumerator FollowPath()
    {
        // 이동속도 이용하는 방법
        int pathCnt = path.Count;
        for (int idx=0; idx< pathCnt; idx++)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = path[idx].coordinates;

            #region Immediately Rotate
            Vector3 direction = path[idx].coordinates - transform.position;
            transform.rotation = Quaternion.LookRotation(direction);
            #endregion

            direction.y = 0;
            direction = direction.normalized;
            while (Vector3.Distance(transform.position,endPosition) > 0.1f)
            {
                transform.position += direction * Time.deltaTime * Speed;
                yield return null;
            }
        }
        ArriveDestination();
    }
        #region Smooth Rotate (Not Use)
        // 회전에선 선형보간인 Lerp보단 구형보간인 Slerp가 좀 더 부드러움
        //Vector3 _direction = path[idx].coordinates - transform.position;
        //Quaternion _rotateValue = Quaternion.LookRotation(_direction);
        //transform.rotation = Quaternion.Slerp(transform.rotation, _rotateValue, movePercent);
        #endregion

    public void ArriveDestination()
    {
        GameManager.Instance.GameSystem.HitByEnemy(enemyData.Damage);
        enemyFeature.Dissolve();
    }

    public void DispatchGameEnd()
    {
        StopAllCoroutines();
        enemyFeature.DisPatchGameEnd();
        Destroy(gameObject);
    }

    #region Check Current Under Tile : Recalculate Path
    [Header("Tile Detect")]
    [SerializeField] LayerMask tileLayer;
    Vector3 detectBoxVec = new Vector3(0.5f, 0.5f, 0.5f);
    public NodeData CurrentOnNodeData()
    {
        Collider[] _colls = Physics.OverlapBox(transform.position - Vector3.up * 0.5f, detectBoxVec, Quaternion.identity, tileLayer);
        int _collCnt = _colls.Length;
        if (_collCnt == 0)
            return null;

        int _nearestIndex = 0;
        float _nearestDistance = Vector3.Distance(transform.position, _colls[0].transform.position);
        for(int i=1; i<_collCnt; i++)
        {
            float _distance = Vector3.Distance(transform.position, _colls[i].transform.position);
            if (_distance < _nearestDistance)
            {
                _nearestDistance = _distance;
                _nearestIndex = i;
            }
        }
        return _colls[_nearestIndex].GetComponent<TileNode>().TileNodeData;
    }
    #endregion
}
