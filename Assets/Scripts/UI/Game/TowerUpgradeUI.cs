using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerUpgradeUI : MonoBehaviour
{
    [SerializeField, Tooltip("0:upgrade, 1:sell")] Button[] towerDecideBtns;
    [SerializeField, Tooltip("0:name, 1: info, 2:upgrade, 3:sell")] TextMeshProUGUI[] texts;

    Turret turret =null;
    public void Init()
    {
        InitialText();
        towerDecideBtns[0].onClick.AddListener(() => PressUpgradeBtn());
        towerDecideBtns[1].onClick.AddListener(() =>PressSellBtn());
    }

    public void InitialText()
    {
        int _cnt = texts.Length;
        for (int i=0; i< _cnt; i++)
        {
            texts[i].text = "";
        }
        towerDecideBtns[0].interactable = false;
        towerDecideBtns[1].interactable = false;
    }

    public void UpdateTower(Turret _turret)
    {
        if (_turret == null)
            InitialText();
        else
        {
            turret = _turret;
            towerDecideBtns[1].interactable = true;

            // 공통 업데이트 부분
            texts[0].text = turret.TowerData.towerName;
            texts[1].text = turret.TowerData.towerInformation + $"\n 공격력: {turret.TowerData.attackValue}, 업그레이드 : {turret.TowerData.upgradeLevel}";
            texts[3].text = turret.TowerData.sellMoney.ToString();

            if (turret.TowerData.CanUpgradeLevel())
            {
                texts[2].text = turret.TowerData.costMoney.ToString();
                towerDecideBtns[0].interactable = true;
            }
            else
            {
                texts[2].text = "X";
                towerDecideBtns[0].interactable = false;
            }
        }
    }

    #region Press Button
    public void PressUpgradeBtn()
    {
        if (GameManager.Instance.GameSystem.CanUse(turret.TowerData.upgradeCost))
        {
            GameManager.Instance.GameSystem.UseMoney(turret.TowerData.upgradeCost);
            turret.TowerData.attackValue += turret.TowerData.GetUpgradeValue();
        }
        else
        {
            Debug.Log("경고 메시지 출력 : 업그레이드 돈이 부족");
        }
    }

    public void PressSellBtn()
    {
        GameManager.Instance.GameSystem.EarnMoney(turret.TowerData.sellMoney);
        Destroy(turret.gameObject);
    }
    #endregion
}
