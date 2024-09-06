using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    Dictionary<int, List<EnemyMover>> enemyPoolGroup = new Dictionary<int, List<EnemyMover>>();
    Dictionary<int, GameObject> resourceEnemySet = new Dictionary<int, GameObject>();
    Dictionary<int, GameObject> poolParentSet = new Dictionary<int, GameObject>();

    [SerializeField] GameObject[] projectileParents;
    Dictionary<int, List<Projectile>> projectilePoolGroup = new Dictionary<int, List<Projectile>>();

    // 게임 씬 내에 있는 씬 스크립트에서 불러오기
    public void Init(List<EnemyData> _enemyDatas)
    {
        #region Enemy
        int _enemyCnt = _enemyDatas.Count;
        for(int idx=0; idx<_enemyCnt; idx++)
        {
            GameObject _enemyObject = Resources.Load<GameObject>($"Prefab/Enemy/{_enemyDatas[idx].EnemyName}");

            if (_enemyObject == null)
            {
                Debug.LogError("해당 적의 정보를 불러올 수 없습니다.");
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
        #endregion
    }

    public void SummonEnemy(int _enemyID)
    {
        if (!enemyPoolGroup.ContainsKey(_enemyID))
        {
            Debug.LogError("해당 적 정보는 해당 스테이지에 없습니다!");
            return;
        }

        int _enemyCnt = enemyPoolGroup[_enemyID].Count;
        List<EnemyMover> _enemyMoveSet = enemyPoolGroup[_enemyID];
        for (int idx= 0; idx<_enemyCnt; idx++)
        {
            if (!_enemyMoveSet[idx].gameObject.activeSelf)
            {
                _enemyMoveSet[idx].gameObject.SetActive(true);
                _enemyMoveSet[idx].SummonEnemy();
                return;
            }
        }
        GameObject _enemy = Instantiate(resourceEnemySet[_enemyID], poolParentSet[_enemyID].transform);
        EnemyMover _enemyMover = _enemy.GetComponent<EnemyMover>();
        enemyPoolGroup[_enemyID].Add(_enemyMover);
        _enemyMover.SummonEnemy();
    }

    public Projectile SummonProjectile(Projectile _projectile)
    {
        int _id = (int)_projectile.AttackType;
        if (!projectilePoolGroup.ContainsKey(_id))
        {
            Debug.LogError("해당 총알 정보는 해당 스테이지에 없습니다!");
            return null;
        }

        int _projectileCnt = projectilePoolGroup[_id].Count;
        List<Projectile> _projectileSet = projectilePoolGroup[_id];
        for (int idx = 0; idx < _projectileCnt; idx++)
        {
            if (!_projectileSet[idx].gameObject.activeSelf)
            {
                _projectileSet[idx].gameObject.SetActive(true);
                return _projectileSet[idx];
            }
        }
        GameObject _projectileObject = Instantiate(_projectile.gameObject);
        _projectileObject.transform.SetParent(projectileParents[(int)_projectile.AttackType].transform);
        Projectile _newProjectile = _projectileObject.GetComponent<Projectile>();
        projectilePoolGroup[_id].Add(_newProjectile);
        return _newProjectile;
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
        // 불 타 사라지는 셰이더 효과 주면서 비활성화 시키기

    }
}
