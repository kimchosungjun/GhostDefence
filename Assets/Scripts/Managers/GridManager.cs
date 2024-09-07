using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager
{
    #region Use For PathFind
    AStarPathFinder pathFinder = new AStarPathFinder();
    public AStarPathFinder PathFinder { get { return pathFinder; } }

    Dictionary<Vector2Int, NodeData> gridMap = new Dictionary<Vector2Int, NodeData>();
    public Vector2Int StartCoordinate { get; set; }
    public Vector2Int EndCoordinate { get; set; }
    #endregion

    #region Use For GetTileNode
    Dictionary<string, TileNode> tileMap = new Dictionary<string, TileNode>();
    #endregion

    // 게임 씬 Awake에서 호출
    public void LoadStartEndPoint(StageData _stageData)
    {
        int _startXPos = 0;
        int _startYPos = 0;
        int _endXPos = _stageData.EndX;
        int _endYPos = _stageData.EndY;

        StartCoordinate = new Vector2Int(_startXPos, _startYPos);
        EndCoordinate = new Vector2Int(_endXPos, _endYPos);
    }        

    // Node Tile에서 호출 
    public void AddMap(NodeData _nodeData)
    {
        Vector2Int _coordinate = new Vector2Int(_nodeData.xPos, _nodeData.zPos);
        if (gridMap.ContainsKey(_coordinate))
            return;
        gridMap.Add(_coordinate, _nodeData);
    }
    
    public NodeData GetNodeData(Vector2Int _coordinate)
    {
        if (gridMap.ContainsKey(_coordinate))
            return gridMap[_coordinate];
        return null;
    }

    public void AddTile(string _name,TileNode _tileNode)
    {
        if (tileMap.ContainsKey(_name))
            return;
        tileMap.Add(_name, _tileNode);
    }

    public TileNode GetTileNode(string _name)
    {
        if (tileMap.ContainsKey(_name))
            return tileMap[_name];
        return null;
    }

    public void AllClearData()
    {
        StartCoordinate = Vector2Int.zero;
        EndCoordinate = Vector2Int.zero;
        if(gridMap.Count>0)
            gridMap.Clear();
        if(tileMap.Count>0)
            tileMap.Clear();
    }
}
