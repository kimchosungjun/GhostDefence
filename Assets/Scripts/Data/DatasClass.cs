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
    public int XSize;
    public int YSize;
    public float Time;
    public int HP;
    public int Money;
    public StageData(int _id, int _xSize, int _ySize, float _time, int _hp, int _money)
    {
        StageID = _id;
        XSize = _xSize;
        YSize = _ySize;
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

    public EnemyData(int _id, float _speed, float _hp, string _name, int _damage)
    {
        EnemyID = _id;
        Speed = _speed;
        HP = _hp;
        EnemyName = _name;
        Damage = _damage;
    }
}
#endregion

#region Node Data
[Serializable] 
public class NodeData
{
    #region Placeable
    public bool canPlace;
    public bool isAccessible;
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

    // ������
    public NodeData(int _xPos, int _yPos ,int _zPos, bool _isAccessible, TileKind _tileKind)
    {
        // Recieve Parameter 
        xPos = _xPos;
        zPos = _zPos;
        coordinates = new Vector3(_xPos, _yPos, _zPos);
        isAccessible = _isAccessible;
        tileKind = _tileKind;
        switch (_tileKind)
        {
            case TileKind.Grass:
                multiplyCost = 1;
                break;
            case TileKind.Stone:
                multiplyCost = 2;
                break;
            default:
                multiplyCost = 0;
                break;
        }

        // Not Receive Parameter
        gCost = 0;
        hCost = 0;
        parentNode = null;
        canPlace = true;
    }

    #region Function
    public bool CanAccessTile()
    {
        if (!isAccessible)
            return false;
        if (canPlace)
            return true;
        return false;
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

    public PlayerData()
    {
        clearStage = 0;
        playerName = "";
        dialogueIndex = -1;
        nameIndex = -1;
    }

    public PlayerData(string _playerName, int _nameIndex)
    {
        clearStage = 0;
        playerName = _playerName;
        dialogueIndex = -1;
        nameIndex = _nameIndex;
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