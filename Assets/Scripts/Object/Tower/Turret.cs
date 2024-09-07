using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    protected int enemyLayer = 1<<(int)DefineLayer.Enemy;
    [SerializeField] protected Transform launchPoint; 
    [SerializeField] protected Transform headTf;
    [SerializeField] protected Projectile projectile;
    [SerializeField] protected STowerData stowerData;
    public STowerData ScriptableTowerData { get { return stowerData; } }

    protected PTowerData currentTowerData;
    public PTowerData TowerData { get { return currentTowerData; } }

    protected abstract void DetectTarget();

    protected virtual void RotateHead() { }

    protected abstract void Attack();
}
