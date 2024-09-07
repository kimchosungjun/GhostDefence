using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFeature : MonoBehaviour
{
    [SerializeField] protected SkinnedMeshRenderer skmRenderer;
    [SerializeField] protected MeshRenderer mRenderer;
    [SerializeField] protected Collider coll;
    [SerializeField, Range(1f,2f)] protected float dissolveTime;
    protected EnemyData enemyData = null;
    protected EnemyMover enemyMover = null;
    protected List<Material> rendererMatGroup = new List<Material>();

    float enemyHP = 0;
    public virtual void Init(EnemyData _enemyData, EnemyMover _mover)
    {
        if (enemyMover==null)
            enemyMover = _mover;

        if (mRenderer != null)
        {
            Material[] mrendererMats = mRenderer.materials;
            int _matCnt = mrendererMats.Length;
            for (int k = 0; k < _matCnt; k++)
            {
                rendererMatGroup.Add(mrendererMats[k]);
            }
        }

        if (skmRenderer != null)
        {
            Material[] skmrendererMats = skmRenderer.materials;
            int _skmatCnt = skmrendererMats.Length;
            for (int i = 0; i < _skmatCnt; i++)
            {
                rendererMatGroup.Add(skmrendererMats[i]);
            }
        }

        if (_enemyData != null)
        {
            enemyData = _enemyData;
            enemyHP = enemyData.HP;
        }
        if (coll == null)
            coll = GetComponentInChildren<Collider>();
        
        isDead = false;
    }

    public virtual void Hit(float _damage, TowerAttackType _attackType)
    {
        if (isDead)
            return;
        enemyHP -= _damage;
        if (enemyHP <= 0)
            Death();
        else
            HitEffect(_attackType);
    }

    bool isDead = false;
    public void Death()
    {
        isDead = true;
        GameManager.Instance.GameSystem.EarnMoney(enemyData.Money);
        Dissolve();
    }

    public virtual void Dissolve()
    {
        coll.enabled = false;
        isSlow = false;
        enemyMover.Speed = enemyData.Speed;
        StartCoroutine(DissolveEffect());   
    }

    IEnumerator DissolveEffect()
    {
        float _timer = 0f;
        int _matCnt = rendererMatGroup.Count;
        float _dissolveValue = 0f;
        while (_timer < dissolveTime)
        {
            _timer += Time.deltaTime;
            _dissolveValue = Mathf.Lerp(1, 0, _timer / dissolveTime);
            for (int idx = 0; idx < _matCnt; idx++)
            {
                rendererMatGroup[idx].SetFloat("_Split", _dissolveValue);
            }
            yield return null;
        }
        gameObject.SetActive(false);
        coll.enabled = true;
        for (int idx = 0; idx < _matCnt; idx++)
        {
            rendererMatGroup[idx].SetFloat("_Split", 1);
        }
        transform.position = new Vector3(GameManager.Grid.StartCoordinate.x, transform.position.y, GameManager.Grid.StartCoordinate.y);
    }

    public virtual void HitEffect(TowerAttackType _attackType)
    {
        switch (_attackType)
        {
            case TowerAttackType.Slow:
                SlowFeature();
                break;
            case TowerAttackType.Explosion:
                ExplosionFeature();
                break;
        }
    }

    #region Slow

    bool isSlow = false;
    float slowTimer = 0f;
    public void SlowFeature()
    {
        if (isSlow)
            slowTimer = 0f;
        else
        {
            enemyMover.Speed = enemyData.Speed / 2;
            StartCoroutine(SlowTimer());
        }
    }

    IEnumerator SlowTimer()
    {
        isSlow = true;
        while (slowTimer<2f)
        {
            if (!isSlow)
                yield break;
            slowTimer += Time.deltaTime;
            yield return null;
        }
        isSlow = false;
        enemyMover.Speed = enemyData.Speed;
    }
    #endregion

    #region Explosion
    WaitForSeconds oneSec = new WaitForSeconds(1f);
    public void ExplosionFeature()
    {
        if(!isDead)
            StartCoroutine(ExplosionEffect());
    }

    IEnumerator ExplosionEffect()
    {
        if (isDead)
            yield break;
        yield return oneSec;
        if (isDead)
            yield break;
        Hit(1, TowerAttackType.Normal);
        if (isDead)
            yield break;
        yield return oneSec;
        if (isDead)
            yield break;
        Hit(1, TowerAttackType.Normal);
        if (isDead)
            yield break;
        yield return oneSec;
        if (isDead)
            yield break;
        Hit(1, TowerAttackType.Normal);
    }
    #endregion

    public virtual void ResetStat()
    {
        
        EnemyData originalData = GameManager.Instance.Data.GetEnemyData(enemyData.EnemyID);
        enemyData.Damage = originalData.Damage;
        enemyData.HP = originalData.HP;
        enemyData.Speed = originalData.Speed;
        enemyHP = enemyData.HP;
        isDead = false;
    }
}
