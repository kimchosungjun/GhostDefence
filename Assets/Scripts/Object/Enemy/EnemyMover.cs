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
            Debug.LogError("�� �����͸� ã�� �� ����..");

        if (enemyFeature == null)
            enemyFeature = GetComponent<EnemyFeature>();
        if(enemyFeature!=null && enemyData!=null)
            enemyFeature.Init(enemyData,this);
        else
            Debug.LogError("�ʱ�ȭ ����!!!!");
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
        // ���������� ������ ���� ��, ��� ��Ž��
        NodeData _startNode = CurrentOnNodeData();
        if (_startNode == null)
        {
            Debug.LogError("�������� ã�� ���߽��ϴ�! : ���� �߻�!!");
            return;
        }
        NodeData _endNode = gridManager.GetNodeData(gridManager.EndCoordinate);
        // �̹� �������� ������ ��Ž�� ����
        if (_startNode == _endNode)
            return;
        if(followPathCoroutine!=null)
            StopCoroutine(followPathCoroutine);

        if (path.Count > 0)
            path.Clear();
        path = gridManager.PathFinder.GetPath(_startNode, _endNode);

        // ��� ���� �����̱�
        if (path == null || path.Count == 0)
            return;
        followPathCoroutine =StartCoroutine(FollowPath());
    }

    public IEnumerator FollowPath()
    {
        // �̵��ӵ� �̿��ϴ� ���
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
        // ȸ������ ���������� Lerp���� ���������� Slerp�� �� �� �ε巯��
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
