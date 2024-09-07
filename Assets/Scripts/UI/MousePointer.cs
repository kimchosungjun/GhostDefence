using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour
{
    [SerializeField, Tooltip("0 : idle, 1 : press, 2: Build")] List<Texture2D> mouseCursor;

    bool isBuildCursor = false;
    public bool IsBuildCursor { get { return isBuildCursor; }set { isBuildCursor = value; ChangeCursorImage(value); } }

    private void Awake()
    {
        Cursor.SetCursor(mouseCursor[0], new Vector2(mouseCursor[0].width * 0.5f, mouseCursor[0].height * 0.5f), CursorMode.Auto);
    }

    void Update()
    {
        if (!IsBuildCursor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.SetCursor(mouseCursor[1], new Vector2(mouseCursor[1].width * 0.5f, mouseCursor[1].height * 0.5f), CursorMode.Auto);
            }
            else if(Input.GetMouseButtonUp(0))
            {
                Cursor.SetCursor(mouseCursor[0], new Vector2(mouseCursor[0].width * 0.5f, mouseCursor[0].height * 0.5f), CursorMode.Auto);
            }
        }
    }

    public void ChangeCursorImage(bool _isBuildCursor)
    {
        if (_isBuildCursor)
        {
            Cursor.SetCursor(mouseCursor[2], new Vector2(mouseCursor[2].width * 0.5f, mouseCursor[2].height * 0.5f), CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(mouseCursor[0], new Vector2(mouseCursor[0].width * 0.5f, mouseCursor[0].height * 0.5f), CursorMode.Auto);
        }
    }
}
