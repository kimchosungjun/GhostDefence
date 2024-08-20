using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemManager 
{
    /******* 스테이지 데이터를 불러옴 : 시간, HP, 돈 ********/
    #region Stage Data
    StageData stageData = null;

    int playerHP = 0;
    int playerMoney = 0;
    float playerTime = 0f;
    #endregion

    // 게임 씬 진입 시, 현재 스테이지의 정보를 불러옴
    public void InitGameStage(StageData _stageData)
    {
        stageData = _stageData;
        if (stageData==null)
        {
            Debug.LogError("데이터를 찾을 수 없습니다!");
            return;
        }

        playerHP = stageData.HP;
        playerMoney = stageData.Money;
        playerTime = stageData.Time;
        // UI 적용
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
        // UI 적용
    }

    public void GameOver()
    {
        // 게임 오버 UI 적용
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
        // UI 적용
    }

    public void EarnMoney(int _money)
    {
        playerMoney += _money;
        // UI 적용
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
