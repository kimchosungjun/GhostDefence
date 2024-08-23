using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinder 
{
    int deltaCnt = 4;
    int[] deltaX = { 0, 0, 2, -2};
    int[] deltaZ = { 2, -2, 0, 0 };
    public List<NodeData> GetNearNodeDatas(NodeData _currrentNodeData)
    {
        int _startX = GameManager.Grid.StartCoordinate.x;
        int _startZ = GameManager.Grid.StartCoordinate.y;
        int _endX = GameManager.Grid.EndCoordinate.x;
        int _endZ = GameManager.Grid.EndCoordinate.y;

        List<NodeData> _nearNodeDatas = new List<NodeData>();

        for(int idx =0; idx<deltaCnt; idx++)
        {
            int _nextX = _currrentNodeData.xPos + deltaX[idx];
            int _nextZ = _currrentNodeData.zPos + deltaZ[idx];

            if((_nextX>=_startX && _nextX<=_endX) && (_nextZ>=_startZ && _nextZ <= _endZ))
            {
                NodeData _nearNode = GameManager.Grid.GetNodeData(new Vector2Int(_nextX, _nextZ));
                if (!_nearNode.CanAccessTile())
                    continue;
                _nearNodeDatas.Add(GameManager.Grid.GetNodeData(new Vector2Int(_nextX, _nextZ)));
            }
        }
        return _nearNodeDatas;
    }  

    // H 값은 거리만 고려, 비용은 G 값만 고려한다.
    public int GetHDistance(NodeData _startData, NodeData _endData)
    {
        return Mathf.Abs(_startData.xPos - _endData.xPos) + Mathf.Abs(_startData.zPos - _endData.zPos);
    }

    public int GetGDistance(NodeData _startData, NodeData _endData)
    {
        return Mathf.Abs(_startData.xPos - _endData.xPos) + Mathf.Abs(_startData.zPos - _endData.zPos) * _endData.multiplyCost;
    }

    public List<NodeData> TracePath(NodeData _startNode, NodeData _endNode)
    {
        List<NodeData> _path = new List<NodeData>();
        NodeData _currentNode = _endNode;

        while (_currentNode != _startNode)
        {
            _path.Add(_currentNode);
            _currentNode = _currentNode.parentNode;
        }
        _path.Reverse();

        return _path;
    }

    public List<NodeData> GetPath(NodeData _startNode, NodeData _endNode)
    {
        // HashSet을 사용해도 됨
        List<NodeData> openNodeSet= new List<NodeData>();
        List<NodeData> closeNodeSet= new List<NodeData>();

        openNodeSet.Add(_startNode);

        while (openNodeSet.Count > 0)
        {
            NodeData currentNode = openNodeSet[0];

            int openNodeSetCnt = openNodeSet.Count;
            for (int idx = 1; idx < openNodeSetCnt; idx++)
            {
                if ((openNodeSet[idx].fCost < currentNode.fCost) || ((openNodeSet[idx].fCost == currentNode.fCost) && openNodeSet[idx].hCost < currentNode.hCost))
                {
                    currentNode = openNodeSet[idx];
                }
            }

            closeNodeSet.Add(currentNode);
            openNodeSet.Remove(currentNode);

            if ((currentNode.xPos == _endNode.xPos) && (currentNode.zPos == _endNode.zPos))
            {
                return TracePath(_startNode, currentNode);
            }

            List<NodeData> nearNodeSet = GetNearNodeDatas(currentNode);
            int nearNodeCnt = nearNodeSet.Count;
            for(int idx =0; idx<nearNodeCnt; idx++)
            {
                NodeData _nearNode = nearNodeSet[idx];
                int _nearGCost = currentNode.gCost + GetGDistance(currentNode, _nearNode);

                if (closeNodeSet.Contains(_nearNode))
                    continue;

                if(currentNode.gCost > _nearGCost || !openNodeSet.Contains(_nearNode))
                {
                    _nearNode.gCost = _nearGCost;
                    _nearNode.hCost = GetHDistance(_nearNode, _endNode);
                    _nearNode.parentNode = currentNode;

                    if (!openNodeSet.Contains(_nearNode))
                        openNodeSet.Add(_nearNode);
                }
            }
        }
        return null;
    }
}
