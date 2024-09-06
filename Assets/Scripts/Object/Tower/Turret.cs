using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    protected int enemyLayer = 1<<(int)DefineLayer.Enemy;
    [SerializeField] protected Transform launchPoint; 
    [SerializeField] protected Transform headTf;
    [SerializeField] protected STowerData towerData;
    [SerializeField] protected Projectile projectile;

    public STowerData TowerData { get { return towerData; } }

    protected abstract void DetectTarget();

    protected virtual void RotateHead() { }

    protected abstract void Attack();
}
