using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TileEnums;

public class TileNode : MonoBehaviour
{
    [SerializeField] const int gridSize = 2;
    [SerializeField] TileFeatureType currentTileFeatureType;
    [SerializeField] TileKind currentTileKind;
 
    public bool CanPlace { get; set; } = true;

    private void Start()
    {
        InitNodeData();
    }

    public void InitNodeData()
    {
        if (currentTileKind == TileKind.Water)
            currentTileFeatureType = TileFeatureType.Inaccessible;

        //int _divideSize = (int)UnityEditor.EditorSnapSettings.gridSize.z;
        int _xPos = Mathf.RoundToInt(transform.position.x / gridSize);
        int _zPos = Mathf.RoundToInt(transform.position.z / gridSize);
        int _yPos = (int)transform.position.y;
        //Vector2Int _coordinate = new Vector2Int(_xPos, _zPos);

        bool _isAccessible;
        if (currentTileFeatureType == TileFeatureType.Inaccessible)
            _isAccessible = false;
        else
            _isAccessible = true;

        NodeData _currentNodeData = new NodeData(_xPos, _yPos, _zPos, _isAccessible, currentTileKind);
        GameManager.Grid.AddMap(_currentNodeData);
    }
}
