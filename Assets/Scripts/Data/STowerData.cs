using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="TowerData_Scriptable", menuName ="Scriptable/Tower", order =int.MaxValue)]
public class STowerData : ScriptableObject
{
    const float increaseUpgradeValue = 1f;

    [Header("Stat")]
    public TowerAttackType attackType;
    public float attackFrequency;
    public float attackValue;
    public float attackRange;

    [Header("Cost")]
    public int costMoney;
    public int sellMoney;
    public int upgradeLevel;
    public int upgradeCost;

    [Header("Info")]
    public string towerName;
    public string towerInformation;
}


public class PTowerData
{
    const float increaseUpgradeValue = 1f;

    [Header("Stat")]
    public TowerAttackType attackType;
    public float attackFrequency;
    public float attackValue;
    public float attackRange;

    [Header("Cost")]
    public int costMoney;
    public int sellMoney;
    public int upgradeLevel;
    public int upgradeCost;

    [Header("Info")]
    public string towerName;
    public string towerInformation;

    public PTowerData(STowerData _data)
    {
        upgradeLevel = 0;
        attackType = _data.attackType;
        attackFrequency = _data.attackFrequency;
        attackValue = _data.attackValue;
        costMoney = _data.costMoney;
        sellMoney = _data.sellMoney;
        attackRange = _data.attackRange;
        towerName = _data.towerName;
        towerInformation = _data.towerInformation;
        upgradeCost = _data.upgradeCost;
    }

    #region Method
    public float GetUpgradeValue() { sellMoney += 5; upgradeLevel += 1; return increaseUpgradeValue; }
    #endregion
}