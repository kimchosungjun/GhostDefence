using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTile : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRend;
    [SerializeField, Tooltip("0:CanSelect, 1:CannotSelect")] Material[] mats;
    private void Awake()
    {
        if (meshRend == null)
            meshRend = GetComponent<MeshRenderer>();
    }


    public void UpdateTile(Vector3 _pos, bool _canPlace)
    {
        transform.position = _pos;
        transform.position += Vector3.up;
        if (_canPlace)
            meshRend.material = mats[0];
        else
            meshRend.material = mats[1];
    }
}
