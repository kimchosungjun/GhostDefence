using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemManager : MonoBehaviour
{
    /******* 스테이지 데이터를 불러옴 : 시간, HP, 돈 ********/
    #region Stage Data
    StageData stageData = null;
    SummonData summonData;
    GameSceneController gameSceneController;
    GameSceneUIController uiController;

    int playerHP = 0;
    int playerMoney = 0;
    float playerTime = 0f;
    float playerTimer = 0f;

    int summonCnt = 0;
    float[] summonCycleTime; 
    float[] summonStartTime;

    float[] summonStartTimer;

    bool[] isSummonCommand;
    #endregion

    // 게임 씬 진입 시, 현재 스테이지의 정보를 불러옴
    public void InitGameStage(StageData _stageData, GameSceneController _gameSceneController)
    {
        gameSceneController = _gameSceneController;
        uiController = _gameSceneController.UIController;

        #region Stage Data
        stageData = _stageData;
        if (stageData == null)
        {
            Debug.LogError("데이터를 찾을 수 없습니다!");
            return;
        }

        playerHP = stageData.HP;
        playerMoney = stageData.Money;
        playerTime = stageData.Time;
        playerTimer = stageData.Time;

        uiController.UpdateHPValue(playerHP);
        uiController.UpdateTimeValue(playerTime);
        uiController.UpdateEnergyValue(playerMoney);
        #endregion

        #region Summon Data
        summonData = GameManager.Instance.Data.GetSummonData(_stageData.StageID);
        if (summonData == null)
        {
            Debug.LogError("소환 데이터를 찾을 수 없습니다!");
            return;
        }

        summonCnt = summonData.summonCycle.Count;
        summonCycleTime = new float[summonCnt];
        summonStartTime = new float[summonCnt];
        summonStartTimer = new float[summonCnt];
        isSummonCommand = new bool[summonCnt];

        for (int idx=0; idx< summonCnt; idx++)
        {
            summonCycleTime[idx] = summonData.summonCycle[idx];
            summonStartTime[idx] = summonData.summonStartTime[idx];
            summonStartTimer[idx] = summonData.summonStartTime[idx];
            isSummonCommand[idx] = false;
        }
        #endregion
    }

    /****************************** HP ***************************************/
    #region Handle HP
    public void HitByEnemy(int _damage)
    {
        if (playerHP - _damage <= 0)
        {
            GameOver();
            return;
        }
        playerHP -= _damage;
        uiController.UpdateHPValue(playerHP);
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
        uiController.UpdateEnergyValue(playerMoney);
    }

    public void EarnMoney(int _money)
    {
        playerMoney += _money;
        uiController.UpdateEnergyValue(playerMoney);
    }
    #endregion

    /****************************** Time ***************************************/
    #region Handle Time

    public void StartGame()
    {
        StartCoroutine(Timer());
    }

    public void StopGame()
    {
        //StopCoroutine(Timer());
        Time.timeScale = 0f;
    }

    public IEnumerator Timer()
    {
        float _startTime = Time.time;
        float _takeTime = 0f;
        
        playerTime = playerTimer;

        for(int idx=0; idx<summonCnt; idx++)
        {
            summonStartTime[idx] = summonStartTimer[idx];
        }

        while (playerTimer >= 0f)
        {
            _takeTime = Time.time - _startTime;
            playerTimer = playerTime- _takeTime;
            uiController.UpdateTimeValue(playerTimer);

            for (int idx=0; idx<summonCnt; idx++)
            {

                if (summonStartTimer[idx] > 0f)
                {
                    summonStartTimer[idx] = summonStartTime[idx] -_takeTime;
                    continue;
                }

                if (!isSummonCommand[idx])
                 {
                    isSummonCommand[idx] = true;
                    StartCoroutine(SummonCommandCor(idx, summonCycleTime[idx]));
                 }
            }
            yield return null;
        }
        WinGame();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        //StartGame();
    }

    public void WinGame()
    {
        Debug.Log("승리했습니다.");
        Time.timeScale = 0f;
    }
    public IEnumerator SummonCommandCor(int _index, float _waitTime)
    {
        gameSceneController.SummonEnemy(summonData.enemyID[_index]);
        yield return new WaitForSeconds(_waitTime);
        isSummonCommand[_index] = false;
    }
    #endregion
}
