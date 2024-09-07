using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Rigidbody rigid;
    protected bool isCollideEnemy = false;

    protected PTowerData towerData = null;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] TowerAttackType attackType;
    public TowerAttackType AttackType { get { return attackType; } }
    public virtual void Init(Vector3 _launchPoint, Vector3 _destPoint, PTowerData _towerData)
    {
        transform.position = _launchPoint;
        towerData = _towerData;
        isCollideEnemy = false;

        if (rigid == null)
            rigid = GetComponent<Rigidbody>();

        Vector3 _velocity = Vector3.zero;
        Vector3 _dir = _destPoint - _launchPoint;
        _dir.y = 0f;
        _dir=_dir.normalized;

        _velocity = _dir * projectileSpeed;

        rigid.AddForce(_velocity, ForceMode.Impulse);
    }

    /// <summary>
    /// Use For Wide Range Turret
    /// </summary>
    /// <param name="_towerData"></param>
    /// <param name="_launchPoint"></param>
    /// <param name="_direction"></param>
    public void WideInit(PTowerData _towerData, Vector3 _launchPoint, Vector3 _direction)
    {
        transform.position = _launchPoint;

        towerData = _towerData;
        isCollideEnemy = false;

        if (rigid == null)
            rigid = GetComponent<Rigidbody>();

        Vector3 _velocity = Vector3.zero;
        _velocity = _direction * projectileSpeed;
        rigid.AddForce(_velocity, ForceMode.Impulse);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !isCollideEnemy)
        {
            isCollideEnemy = true;
            EnemyFeature _feature = other.GetComponent<EnemyFeature>();
            _feature.Hit(towerData.attackValue, towerData.attackType);
            rigid.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }

    protected virtual void OnBecameInvisible()
    {
        rigid.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
