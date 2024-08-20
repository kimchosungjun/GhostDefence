using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemManager 
{
    /******* �������� �����͸� �ҷ��� : �ð�, HP, �� ********/
    #region Stage Data
    StageData stageData = null;

    int playerHP = 0;
    int playerMoney = 0;
    float playerTime = 0f;
    #endregion

    // ���� �� ���� ��, ���� ���������� ������ �ҷ���
    public void InitGameStage(StageData _stageData)
    {
        stageData = _stageData;
        if (stageData==null)
        {
            Debug.LogError("�����͸� ã�� �� �����ϴ�!");
            return;
        }

        playerHP = stageData.HP;
        playerMoney = stageData.Money;
        playerTime = stageData.Time;
        // UI ����
    }

    /****************************** HP ***************************************/
    #region Handle HP
    public void HitByEnemy(int _damage)
    {
        if(playerHP-_damage<=0)
        {
            GameOver();
            return;
        }
        playerHP -= _damage;
        // UI ����
    }

    public void GameOver()
    {
        // ���� ���� UI ����
        Time.timeScale = 0f;
    }
    #endregion

    /****************************** Money ***************************************/
    #region Handle Money
    public bool CanUse(int _cost)
    {
        if (playerMoney - _cost < 0)
            return false;
        return true;
    }

    public void UseMoney(int _cost)
    {
        if (!CanUse(_cost))
            return;
        playerMoney -= _cost; 
        // UI ����
    }

    public void EarnMoney(int _money)
    {
        playerMoney += _money;
        // UI ����
    }
    #endregion

    /****************************** Time ***************************************/
    #region Handle Time
    public void StartGame()
    {

    }

    public void StopGame()
    {

    }

    public void ResumeGame()
    {

    }

    public void WinGame()
    {

    }
    #endregion
}
