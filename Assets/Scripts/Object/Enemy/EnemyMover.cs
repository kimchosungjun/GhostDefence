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
            Debug.LogError("적 데이터를 찾을 수 없다..");

        gridManager = GameManager.Grid;
    }

    private void Start()
    {
        CalculatePath();
    }

    private void OnDisable()
    {
        isFollowPath = false;
        transform.position = new Vector3(GameManager.Grid.StartCoordinate.x, transform.position.y, GameManager.Grid.StartCoordinate.y);
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
        // 이동속도 이용하는 방법
        isFollowPath = true;
        int pathCnt = path.Count;
        for (int idx=0; idx< pathCnt; idx++)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = path[idx].coordinates;
            
            float movePercent = 0f;
            while (movePercent < 1f)
            {
                movePercent += Time.deltaTime * enemyData.Speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, movePercent);
                yield return null;
            }
        }

        ArriveDestination();
    }

    public void ArriveDestination()
    {
        GameManager.GameSystem.HitByEnemy(enemyData.Damage);
        gameObject.SetActive(false);
    }
}
