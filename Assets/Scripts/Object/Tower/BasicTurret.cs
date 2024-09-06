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

    public void Update()
    {
        DetectTarget();
        RotateHead();
        Attack();
    }

    bool canAttack = true;
    protected override void Attack()
    {
        if (canAttack && Target!=null)
        {
            canAttack = false;
            LaunchProjectile();
            StartCoroutine(AttackCoolTimer());
        }
    }

    public void LaunchProjectile()
    {
         Projectile _projectile = GameManager.Instance.GameSystem.GameController.SummonProjectile(projectile);
        _projectile.Init(launchPoint.position, Target.position, currentTowerData);
    }

    IEnumerator AttackCoolTimer()
    {
        yield return new WaitForSeconds(currentTowerData.attackFrequency);
        canAttack = true;
    }

    protected override void DetectTarget()
    {
        if (Target != null)
        {
            if (Vector3.Distance(Target.position, transform.position) < currentTowerData.attackRange)
                Target = null;
            else
                return;
        }

        Collider[]  _colls = Physics.OverlapSphere(transform.position, currentTowerData.attackRange, enemyLayer);


        int _closestIndex = -1;
        float _closestDistance = -1f;
        int _collsCnt = _colls.Length;
        
        if (_collsCnt >= 1)
        {
            _closestIndex = 0;
            _closestDistance = Vector3.Distance(transform.position, _colls[0].transform.position);
        }

        for(int i=1; i<_collsCnt; i++)
        {
            float _distance = Vector3.Distance(transform.position, _colls[i].transform.position);
            if (_closestDistance > _distance)
            {
                _closestIndex = i;
                _closestDistance = _distance;
            }
        }

        if (_closestIndex != -1)
            Target = _colls[_closestIndex].transform;
    }

    protected override void RotateHead()
    {
        if (Target == null)
            return;
        headTf.LookAt(Target);
    }
}
