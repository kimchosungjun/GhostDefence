using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="TowerData_Scriptable", menuName ="Scriptable/Tower", order =int.MaxValue)]
public class STowerData : ScriptableObject
{
    const int maxUpgradeLevel =3;
    const float increaseUpgradeValue = 1f;
    
    public TowerAttackType attackType;
    public float attackFrequency;
    public float attackValue;
    public int costMoney;
    public float attackRange;
    public int upgradeLevel;

    public GameObject weaponObject;

    public STowerData(TowerAttackType _attackType , float _attackFrequency, float _attackValue, int _costMoney, float _attackRange)
    {
        upgradeLevel = 0;
        attackType = _attackType;
        attackFrequency = _attackFrequency;
        attackValue = _attackValue;
        costMoney = _costMoney;
        attackRange = _attackRange;
    }

    public STowerData(STowerData _data)
    {
        upgradeLevel = 0;
        attackType = _data.attackType;
        attackFrequency = _data.attackFrequency;
        attackValue = _data.attackValue;
        costMoney = _data.costMoney;
        attackRange = _data.attackRange;
    }
}
