using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionProjectile : Projectile
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Collider coll;

    [SerializeField] Explosion explosion;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !isCollideEnemy)
        {
            isCollideEnemy = true;
            EnemyFeature _feature = other.GetComponent<EnemyFeature>();
            _feature.Hit(towerData.attackValue, TowerAttackType.Normal);
            rigid.velocity = Vector3.zero;
            explosion.gameObject.SetActive(true);
            explosion.DoExplosion(this,this.transform.position);
            meshRenderer.enabled = false;
            coll.enabled = false;
        }
    }

    public void InActive()
    {
        gameObject.SetActive(false);
        meshRenderer.enabled = true;
        coll.enabled = true;
    }

    protected override void OnBecameInvisible()
    {
        if (!isCollideEnemy)
        {
            rigid.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}
