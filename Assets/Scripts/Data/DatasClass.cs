using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TileEnums;
public class DatasClass { }

#region Stage Data
[Serializable]
public class StageData
{
    public int StageID;
    public int EndX;
    public int EndY;
    public float Time;
    public int HP;
    public int Money;
    public StageData(int _id, int _endX, int _endY, float _time, int _hp, int _money)
    {
        StageID = _id;
        EndX = _endX;
        EndY = _endY;
        Time = _time;
        HP = _hp;
        Money = _money ;
    }
}
#endregion

#region Enemy Data
[Serializable]
public class EnemyData
{
    public int EnemyID;
    public float Speed;
    public float HP;
    public string EnemyName;
    public int Damage;
    public int Money;
    public EnemyData(EnemyData _enemyData) 
    {
        EnemyID = _enemyData.EnemyID;
        Speed = _enemyData.Speed;
        HP = _enemyData.HP;
        EnemyName = _enemyData.EnemyName;
        Damage = _enemyData.Damage;
        Money = _enemyData.Money;
    }

    public EnemyData(int _id, float _speed, float _hp, string _name, int _damage, int _money)
    {
        EnemyID = _id;
        Speed = _speed;
        HP = _hp;
        EnemyName = _name;
        Damage = _damage;
        Money = _money;
    }
}
#endregion

#region Node Data
[Serializable] 
public class NodeData
{
    #region Placeable
    public bool canPlace;
    public TileKind tileKind;
    #endregion

    #region Cooridinate
    public int xPos;
    public int zPos;
    public Vector3 coordinates;
    #endregion

    #region PathFinder
    public int multiplyCost;
    public int gCost; 
    public int hCost; 
    public int fCost => gCost + hCost; 
    public NodeData parentNode;
    #endregion

    // »ý¼ºÀÚ
    public NodeData(int _xPos, int _yPos ,int _zPos, TileKind _tileKind)
    {
        // Recieve Parameter 
        xPos = _xPos;
        zPos = _zPos;
        coordinates = new Vector3(_xPos, _yPos, _zPos);
        tileKind = _tileKind;
        canPlace = true;
        switch (_tileKind)
        {
            case TileKind.Grass:
                multiplyCost = 1;
                break;
            case TileKind.Stone:
                multiplyCost = 2;
                break;
            case TileKind.Water:
                canPlace = false;
                break;
            case TileKind.StartEnd:
                canPlace = false;
                break;
        }

        // Not Receive Parameter
        gCost = 0;
        hCost = 0;
        parentNode = null;
    }

    #region Function
    public bool CanAccessTile()
    {
        if (canPlace)
            return true;
        else
        {
            if(tileKind==TileKind.StartEnd)
                return true;
            return false;
        }
    }
    #endregion
}
#endregion

#region Summon EnemyData
[Serializable]
public class SummonData
{
    public int stageID;
    public int[] enemyID;
    public List<float> summonStartTime;
    public List<float> summonCycle;
}

public class SummonEnemyData
{
    public List<SummonData> summonEnemyData;
}
#endregion

#region Player Data
public class PlayerData
{
    public int clearStage;
    public string playerName;
    public int dialogueIndex;
    public int nameIndex;

    public bool isClearAll = false;
    public const int maxClearStage = 6;
    public bool[] isSeenDialogue;
    public PlayerData()
    {
        clearStage = 0;
        playerName = "";
        dialogueIndex = -1;
        nameIndex = -1;
        isSeenDialogue = new bool[maxClearStage];
        for(int i=0; i<maxClearStage; i++)
        {
            isSeenDialogue[i] = false;
        }
    }

    public PlayerData(string _playerName, int _nameIndex)
    {
        clearStage = 0;
        playerName = _playerName;
        dialogueIndex = -1;
        nameIndex = _nameIndex;
        isSeenDialogue = new bool[maxClearStage];
        for (int i = 0; i < maxClearStage; i++)
        {
            isSeenDialogue[i] = false;
        }
    }

    public void WinStage(int _index)
    {
        if (_index == clearStage)
        {
            if(_index+1 == maxClearStage)
            {
                isClearAll = true;
            }
            else
            {
                clearStage += 1;
            }
        }
    }
}

#endregion

#region Dialogue Data

[Serializable]
public class Dialogue
{
    public int stageID;
    public string speakerName;
    public List<string> storyLines;
}

[Serializable]
public class DialogueData
{
    public List<Dialogue> dialogues;
}
#endregion