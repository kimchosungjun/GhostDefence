using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    public bool canPlace;

    public int xPos;
    public int zPos;
    public Vector3 coordinates;

    public int gCost; 
    public int hCost; 
    public int fCost => gCost + hCost; 

    public NodeData parentNode; 

    public NodeData(int _xPos, int _yPos ,int _zPos)
    {
        // Recieve Parameter 
        xPos = _xPos;
        zPos = _zPos;
        coordinates = new Vector3(_xPos, _yPos, _zPos);

        // Not Receive Parameter
        gCost = 0;
        hCost = 0;
        parentNode = null;
        canPlace = true;
    }
}
#endregion

#region Summon EnemyData
[Serializable]
public class SummonData
{
    public int stageID;
    public int[] enemyID;
    public float[] summonStartTime;
    public float[] summonCycle;
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
}

#endregion