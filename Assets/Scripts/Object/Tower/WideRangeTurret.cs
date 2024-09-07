using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WideRangeTurret : Turret
{
    bool isNearEnemy = false;

    private void Awake()
    {
        currentTowerData = new PTowerData(stowerData);
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
        if (canAttack && isNearEnemy)
        {
            canAttack = false;
            LaunchProjectile();
            StartCoroutine(AttackCoolTimer());
        }
    }

    public void LaunchProjectile()
    {
        float _angle = 0f;
        for (int i = 0; i < 8; i++)
        {
            Projectile _projectile = GameManager.Instance.GameSystem.GameController.SummonProjectile(projectile);
            Vector3 _dir = Vector3.zero;
            _dir.x = Mathf.Cos(Mathf.Deg2Rad * _angle);
            _dir.z = Mathf.Sin(Mathf.Deg2Rad * _angle);
            _angle += 45f;
            _projectile.WideInit(currentTowerData, launchPoint.position,_dir);
        }
    }

    IEnumerator AttackCoolTimer()
    {
        yield return new WaitForSeconds(currentTowerData.attackFrequency);
        canAttack = true;
    }

    protected override void DetectTarget()
    {
        Collider[] _colls = Physics.OverlapSphere(transform.position, currentTowerData.attackRange, enemyLayer);
        int _collsCnt = _colls.Length;

        if (_collsCnt == 0)
            isNearEnemy = false;
        else
            isNearEnemy = true;
    }
}
