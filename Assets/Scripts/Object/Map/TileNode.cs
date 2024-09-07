using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TileEnums;

public class TileNode : MonoBehaviour
{
    //[SerializeField] const int gridSize = 2;
    //int senseLayer = 1 << (int)DefineLayer.Turret | 1<<(int)DefineLayer.Enemy;
    int enemyLayer = 1 << (int)DefineLayer.Enemy;

    [SerializeField] TileKind currentTileKind;
    public Vector2Int Coordinate { get; set; } = Vector2Int.zero;
    public bool CanPlace { get; set; } = true;

    Vector3 senseHalfScale = new Vector3(1, 1, 1);

    public Vector3 BuildPosition { get; set; } = Vector3.zero;
    public NodeData TileNodeData { get; set; } = null;
    private void Awake()
    {
        InitNodeData();
    }

    public void InitNodeData()
    {
        BuildPosition = transform.position;
        //int _divideSize = (int)UnityEditor.EditorSnapSettings.gridSize.z;
        int _xPos = Mathf.RoundToInt(transform.position.x); // % gridSize
        int _zPos = Mathf.RoundToInt(transform.position.z);
        int _yPos = (int)transform.position.y;
        //Vector2Int _coordinate = new Vector2Int(_xPos, _zPos);

        Coordinate = new Vector2Int(_xPos, _yPos);
        NodeData _currentNodeData = new NodeData(_xPos, _yPos, _zPos, currentTileKind);
        TileNodeData = _currentNodeData;
        GameManager.Grid.AddMap(_currentNodeData);
        GameManager.Grid.AddTile(gameObject.name,this);
    }

    public bool SenseOnTile()
    {
        if (TileNodeData != null)
        {
            // Ÿ���� ��ġ�� �� �ִ� ������
            if (!TileNodeData.canPlace)
                return true;
        }

        // ���� ���̳� Ÿ���� �����ϴ��� Ȯ��
        Collider[] _colls = Physics.OverlapBox(transform.position, senseHalfScale, Quaternion.identity, enemyLayer);
        if (_colls.Length > 0)
            return true;

        // �̰��� ��ġ���� ��, ��Ʈ�� �����ϴ��� Ȯ��
        TileNodeData.canPlace = false;
        NodeData _startNode = GameManager.Grid.GetNodeData(GameManager.Grid.StartCoordinate);
        NodeData _endNode = GameManager.Grid.GetNodeData(GameManager.Grid.EndCoordinate);

        List<NodeData> path = GameManager.Grid.PathFinder.GetPath(_startNode, _endNode);
        TileNodeData.canPlace = true;
        if (path == null)
            return true;
        else
            return false;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(transform.position, senseHalfScale*2);
    //}

    public void BuildTurretOnTile() { TileNodeData.canPlace = false; }
    public void DestroyTurretOnTile() { TileNodeData.canPlace = true; }
}
