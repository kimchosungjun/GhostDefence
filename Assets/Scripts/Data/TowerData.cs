using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="TowerData_Scriptable", menuName ="Scriptable/Tower", order =int.MaxValue)]
public class TowerData : ScriptableObject
{
    public TowerAttackType attackType;
    public float attackFrequency;
    public float attackValue;
    public int costMoney;

    public TowerData(TowerAttackType _attackType , float _attackFrequency, float _attackValue, int _costMoney)
    {
        attackType = _attackType;
        attackFrequency = _attackFrequency;
        attackValue = _attackValue;
        costMoney = _costMoney;
    }
}
