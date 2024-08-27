using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    Dictionary<int, List<EnemyMover>> enemyPoolGroup = new Dictionary<int, List<EnemyMover>>();
    Dictionary<int, GameObject> resourceEnemySet = new Dictionary<int, GameObject>();
    Dictionary<int, GameObject> poolParentSet = new Dictionary<int, GameObject>();

    // ���� �� ���� �ִ� �� ��ũ��Ʈ���� �ҷ�����
    public void Init(List<EnemyData> _enemyDatas)
    {
        int _enemyCnt = _enemyDatas.Count;
        for(int idx=0; idx<_enemyCnt; idx++)
        {
            GameObject _enemyObject = Resources.Load<GameObject>($"Prefab/Enemy/{_enemyDatas[idx].EnemyName}");

            if (_enemyObject == null)
            {
                Debug.LogError("�ش� ���� ������ �ҷ��� �� �����ϴ�.");
                continue;
            }

            GameObject _parentPool = new GameObject($"{_enemyDatas[idx].EnemyName}Pool");
            _parentPool.transform.SetParent(this.gameObject.transform);
            poolParentSet.Add(_enemyDatas[idx].EnemyID, _parentPool);
            resourceEnemySet.Add(_enemyDatas[idx].EnemyID, _enemyObject);

            List<EnemyMover> _enemyMoverSet = new List<EnemyMover>();
            for (int k=0; k<5; k++)
            {
                EnemyMover _enemyMover = Instantiate(_enemyObject, _parentPool.transform).GetComponent<EnemyMover>();
                _enemyMoverSet.Add(_enemyMover);
                _enemyMover.gameObject.SetActive(false);
            }
            enemyPoolGroup.Add(_enemyDatas[idx].EnemyID, _enemyMoverSet);
        }
    }

    public void SummonEnemy(int _enemyID)
    {
        if (!enemyPoolGroup.ContainsKey(_enemyID))
        {
            Debug.LogError("�ش� �� ������ �ش� ���������� �����ϴ�!");
            return;
        }

        int _enemyCnt = enemyPoolGroup[_enemyID].Count;
        List<EnemyMover> _enemyMoveSet = enemyPoolGroup[_enemyID];
        for (int idx= 0; idx<_enemyCnt; idx++)
        {
            if (!_enemyMoveSet[idx].gameObject.activeSelf)
            {
                _enemyMoveSet[idx].gameObject.SetActive(true);
                _enemyMoveSet[idx].CalculatePath();
                return;
            }
        }

        enemyPoolGroup[_enemyID].Add(Instantiate(resourceEnemySet[_enemyID], poolParentSet[_enemyID].transform).GetComponent<EnemyMover>());
    }

    public void AnnounceChangePath()
    {
        List<int> enemyKeys = new List<int>(enemyPoolGroup.Keys);
        int keysCnt = enemyKeys.Count;
        for(int idx= 0; idx< keysCnt; idx++)
        {
            int enemyMoveSetCnt = enemyPoolGroup[enemyKeys[idx]].Count;
            for(int k=0; k< enemyMoveSetCnt; k++)
            {
                if (!enemyPoolGroup[enemyKeys[idx]][k].gameObject.activeSelf)
                    return;
                enemyPoolGroup[enemyKeys[idx]][k].CalculatePath();
            }
        }
    }

    public void ClearAllEnemy()
    {
        // �� Ÿ ������� ���̴� ȿ�� �ָ鼭 ��Ȱ��ȭ ��Ű��

    }
}
