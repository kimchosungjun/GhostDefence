using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enums 
{
    public static string GetStringValue<T>(T _enumValue) where T : Enum
    {
        return Enum.GetName(typeof(T), _enumValue);
    }

    public static int GetIntValue<T>(T _enumValue) where T : Enum
    {
        return Convert.ToInt32(_enumValue);
    }

    public static int GetEnumLenth<T>() where T : Enum
    {
        return System.Enum.GetValues(typeof(T)).Length;
    }
}

public enum SceneName
{
    Lobby,
    Stage,
    Game,
}

public enum TowerAttackType
{
    Normal,
    Slow,
    Explosion
}

public enum TileType
{
    StartEndTile,
    NormalTile
}

public enum GameStageType
{
    Tutorial = 0,
    Stage1,
    Stage2,
    Stage3,
    Stage4,
    Stage5
}

namespace UIEnums
{
    public enum LobbyUIBtn
    {
        Start,
        Setting,
        Exit,
        SelectIcon
    }
}
