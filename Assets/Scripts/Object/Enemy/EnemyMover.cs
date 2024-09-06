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
        path = gridManager.PathFinder.GetPath(_startNode, _endNode);
        
        // ���� �ʴٸ� �̹� ����� ����, ������ ��忡 �ִ� ���̶�� ��� �̵��ϰ� ����
        Vector2Int _currentPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        Vector2Int _startPos = new Vector2Int((int)_startNode.coordinates.x, (int)_startNode.coordinates.z);
        if (_currentPos != gridManager.StartCoordinate && _startPos != gridManager.EndCoordinate)
            StopCoroutine(FollowPath());

        // ��� ���� �����̱�
        if (path != null)
            StartCoroutine(FollowPath());
    }

    public IEnumerator FollowPath()
    {
        // �̵��ӵ� �̿��ϴ� ���
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
                // ȸ������ ���������� Lerp���� ���������� Slerp�� �� �� �ε巯��
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
