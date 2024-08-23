using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager
{
    AStarPathFinder pathFinder = new AStarPathFinder();
    public AStarPathFinder PathFinder { get { return pathFinder; } }

    Dictionary<Vector2Int, NodeData> gridMap = new Dictionary<Vector2Int, NodeData>();
    public Vector2Int StartCoordinate { get; set; }
    public Vector2Int EndCoordinate { get; set; }

    // �������� ������ ���Ӿ����� �ε��ϴ� ������ �ѹ� ȣ��
    public void ClearMap()
    {
        if (gridMap.Count > 0)
            gridMap.Clear();
    }

    // ���� �� Awake���� ȣ��
    public void LoadStartEndPoint(StageData _stageData)
    {
        int _startXPos = 0;
        int _startYPos = 0;
        int _endXPos = _stageData.XSize;
        int _endYPos = _stageData.YSize;

        StartCoordinate = new Vector2Int(_startXPos, _startYPos);
        EndCoordinate = new Vector2Int(_endXPos, _endYPos);
    }        

    // Node Tile���� ȣ�� 
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
}
