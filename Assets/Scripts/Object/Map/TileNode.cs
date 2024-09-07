using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TileEnums;

public class TileNode : MonoBehaviour
{
    //[SerializeField] const int gridSize = 2;
    int senseLayer = 1 << (int)DefineLayer.Turret | 1<<(int)DefineLayer.Enemy;

    [SerializeField] TileFeatureType currentTileFeatureType;
    [SerializeField] TileKind currentTileKind;
    public Vector2Int Coordinate { get; set; } = Vector2Int.zero;
    public bool CanPlace { get; set; } = true;

    Vector3 senseHalfScale = new Vector3(1, 1, 1);

    public Vector3 BuildPosition { get; set; } = Vector3.zero;
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

        bool _isAccessible;
        if (currentTileFeatureType == TileFeatureType.Inaccessible)
            _isAccessible = false;
        else
            _isAccessible = true;

        Coordinate = new Vector2Int(_xPos, _yPos);
        NodeData _currentNodeData = new NodeData(_xPos, _yPos, _zPos, _isAccessible, currentTileKind);
        GameManager.Grid.AddMap(_currentNodeData);
        GameManager.Grid.AddTile(gameObject.name,this);
    }

    public bool SenseOnTile()
    {
        if (currentTileFeatureType != TileFeatureType.Accessible)
            return true;
        // 이곳에 설치했을 때, 루트가 존재하는지 확인
        Collider[]  _colls = Physics.OverlapBox(transform.position, senseHalfScale, Quaternion.identity, senseLayer);
        if (_colls.Length > 0)
            return true;
        return false;
    }
}
