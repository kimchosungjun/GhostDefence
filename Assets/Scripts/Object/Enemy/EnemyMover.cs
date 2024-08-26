using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] int enemyID;
    bool isFollowPath = false;

    EnemyData enemyData = null;
    List<NodeData> path = new List<NodeData>();
    GridManager gridManager;

    #region Unity Life Cycle
    private void Awake()
    {
        if (enemyData == null)
            enemyData = GameManager.Instance.Data.GetEnemyData(enemyID);
        if (enemyData == null)
            Debug.LogError("�� �����͸� ã�� �� ����..");

        gridManager = GameManager.Grid;
    }

    private void Start()
    {
        CalculatePath();
    }

    #endregion

    public void CalculatePath()
    {
        if (isFollowPath)
            StopCoroutine(FollowPath());

        NodeData _startNode = gridManager.GetNodeData(gridManager.StartCoordinate);
        NodeData _endNode = gridManager.GetNodeData(gridManager.EndCoordinate);

        path = gridManager.PathFinder.GetPath(_startNode, _endNode);

        if(path!=null)
            StartCoroutine(FollowPath());
    }

    public IEnumerator FollowPath()
    {
        // �̵��ӵ� �̿��ϴ� ���
        isFollowPath = true;
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
                movePercent += Time.deltaTime * enemyData.Speed;
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
        gameObject.SetActive(false);
        isFollowPath = false;
        transform.position = new Vector3(GameManager.Grid.StartCoordinate.x, transform.position.y, GameManager.Grid.StartCoordinate.y);
    }
}
