using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    int enemyLayer = 1 << (int)DefineLayer.Enemy;
    [SerializeField] float maintainTime;
    [SerializeField] float explosionRange;
    [SerializeField] float explosionDamage;

    ExplosionProjectile projectile = null;
    public void DoExplosion(ExplosionProjectile _projectile, Vector3 _spawnPoint)
    {
        transform.position = _spawnPoint;

        projectile = _projectile;

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRange, enemyLayer);
        int _collCnt = colliders.Length;
        for(int i=0; i<_collCnt; i++)
        {
            colliders[i].gameObject.GetComponent<EnemyFeature>().Hit(explosionDamage, TowerAttackType.Explosion);
        }


        StartCoroutine(MaintainExplosion());
    }

    IEnumerator MaintainExplosion()
    {
        yield return new WaitForSeconds(maintainTime);
        projectile.InActive();
        gameObject.SetActive(false);
    }
}
