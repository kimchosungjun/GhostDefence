using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFeature : MonoBehaviour
{
    [SerializeField] protected SkinnedMeshRenderer meshRenderer;
    [SerializeField] protected Collider coll;
    [SerializeField, Range(1f,2f)] protected float dissolveTime;
    protected EnemyData enemyData = null;
    protected Material[] dissolveMats;

    public virtual void Init(EnemyData _enemyData)
    {
        if (meshRenderer == null)
            meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        Material[] _skinMats = meshRenderer.materials;
        int _matCnt = _skinMats.Length;
        dissolveMats = new Material[_matCnt];
        for (int idx=0; idx<_matCnt; idx++)
        {
            Material _newMat = new Material(_skinMats[idx]);
            dissolveMats[idx] = _newMat;
            meshRenderer.materials[idx] = dissolveMats[idx];
        }

        if (_enemyData!=null)
            enemyData = _enemyData;
        if (coll == null)
            coll = GetComponentInChildren<Collider>();
    }

    public virtual void Hit(float _damage, TowerAttackType _attackType)
    {
        enemyData.HP -= _damage;
        if (enemyData.HP <= 0)
            Dissolve();
        else
            HitEffect(_attackType);
    }

    public virtual void Dissolve()
    {
        coll.enabled = false;
        StartCoroutine(DissolveEffect());
    }

    IEnumerator DissolveEffect()
    {
        float _timer = 0f;
        int _matCnt = dissolveMats.Length;
        float _dissolveValue = 0f;
        while (_timer < dissolveTime)
        {
            _timer += Time.deltaTime;
            _dissolveValue = Mathf.Lerp(1, 0, _timer / dissolveTime);
            for (int idx = 0; idx < _matCnt; idx++)
            {
                dissolveMats[idx].SetFloat("Split Size", _dissolveValue);
            }
            yield return null;
        }
        gameObject.SetActive(false);
        coll.enabled = true;
        for (int idx = 0; idx < _matCnt; idx++)
        {
            dissolveMats[idx].SetFloat("Split Size", 1);
        }
        transform.position = new Vector3(GameManager.Grid.StartCoordinate.x, transform.position.y, GameManager.Grid.StartCoordinate.y);
    }

    public virtual void HitEffect(TowerAttackType _attackType)
    {
        switch (_attackType)
        {
            case TowerAttackType.Normal:
                break;
            case TowerAttackType.Slow:
                break;
            case TowerAttackType.Explosion:
                break;
        }
    }

    public virtual void ResetStat()
    {
        EnemyData originalData = GameManager.Instance.Data.GetEnemyData(enemyData.EnemyID);
        enemyData.Damage = originalData.Damage;
        enemyData.HP = originalData.HP;
        enemyData.Speed = originalData.Speed;
    }
}
