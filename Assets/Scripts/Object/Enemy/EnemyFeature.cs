using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFeature : MonoBehaviour
{
    protected EnemyData enemyData = null;

    public virtual void Init(EnemyData _enemyData)
    {
        if(_enemyData!=null)
            enemyData = _enemyData;
    }

    public virtual void Hit(float _damage, TowerAttackType _attackType)
    {
        enemyData.HP -= _damage;
        if (enemyData.HP <= 0)
            Death();
        else
            HitEffect(_attackType);
    }

    public virtual void Death()
    {

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

    public virtual void ArriveDestination()
    {

    }
}
