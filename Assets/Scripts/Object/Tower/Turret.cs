using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    protected int enemyLayer = 1<<(int)DefineLayer.Enemy;
    [SerializeField] protected Transform headTf;
    [SerializeField] protected STowerData towerData;

    protected abstract void DetectTarget();

    protected virtual void RotateHead() { }

    protected abstract void Attack();
}
