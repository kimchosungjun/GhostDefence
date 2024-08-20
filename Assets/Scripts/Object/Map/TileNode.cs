using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNode : MonoBehaviour
{
    [SerializeField] const int gridSize = 2;
    [SerializeField] TileType currentTileType;

    Vector2Int coordinate;
    public bool CanPlace { get; set; } = true;

    private void Start()
    {
        InitNodeData();
    }

    public void InitNodeData()
    {
        //int _divideSize = (int)UnityEditor.EditorSnapSettings.gridSize.z;
        int _xPos = Mathf.RoundToInt(transform.position.x / gridSize);
        int _zPos = Mathf.RoundToInt(transform.position.z / gridSize);
        int _yPos = (int)transform.position.y;
        coordinate = new Vector2Int(_xPos, _zPos);
        NodeData _currentNodeData = new NodeData(_xPos, _yPos, _zPos);

        GameManager.Grid.AddMap(_currentNodeData);
    }
}
