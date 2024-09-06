using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurret : Turret
{
    STowerData currentTowerData ;
    public STowerData CurrentTowerData { get { return currentTowerData; } }

    public Transform Target { get; set; } = null;

    private void Awake()
    {
        currentTowerData = new STowerData(towerData);
    }

    bool canAttack = true;
    protected override void Attack()
    {
        if (canAttack)
        {
            canAttack = false;
            // 공격 기능 추가
            StartCoroutine(AttackCoolTimer());
        }
    }

    IEnumerator AttackCoolTimer()
    {
        yield return new WaitForSeconds(currentTowerData.attackFrequency);
        canAttack = true;
    }

    protected override void DetectTarget()
    {
        // 타워 범위 내에 이전 타겟이 존재하는지 확인
        if (Target != null)
        {
            if (Vector3.Distance(Target.position, transform.position) < currentTowerData.attackRange)
                Target = null;
            else
                return;
        }

        Collider[]  _colls = Physics.OverlapSphere(transform.position, currentTowerData.attackRange, enemyLayer);

        int _collsCnt = _colls.Length;
        for(int i=0; i<_collsCnt; i++)
        {

        }

    }

    
}
