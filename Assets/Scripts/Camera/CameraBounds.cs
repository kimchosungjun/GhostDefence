using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    float yDelta = 18f;  // 카메라의 고정된 높이
    Camera mainCam;
    Bounds mapBounds;
    [SerializeField] BoxCollider boxColl;

    public void Init()
    {
        if (mainCam == null)
            mainCam = Camera.main;
        if (boxColl == null)
            boxColl = GetComponent<BoxCollider>();
        mapBounds = boxColl.bounds;
        SetCameraBounds();
    }

    float distance;
    float frustumHeight;
    float frustumWidth;

    private void SetCameraBounds()
    {
        distance = mainCam.transform.position.y - mapBounds.min.y;  // 카메라와 맵 바닥 사이의 거리
        frustumHeight = 2.0f * distance * Mathf.Tan(mainCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        frustumWidth = frustumHeight * mainCam.aspect;
    }

    private void Update()
    {
        Vector3 newPosition = mainCam.transform.position;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += Vector3.forward * 0.05f;// 속도 조정
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += Vector3.left * 0.05f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += Vector3.right * 0.05f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += Vector3.back *0.05f;
        }

        // 카메라 위치를 경계 내로 제한
        newPosition.x = Mathf.Clamp(newPosition.x, mapBounds.min.x + frustumWidth / 2, mapBounds.max.x - frustumWidth / 2);
        newPosition.z = Mathf.Clamp(newPosition.z, mapBounds.min.z + frustumHeight / 2, mapBounds.max.z - frustumHeight / 2);

        // yDelta를 사용하여 y 값을 고정
        newPosition.y = yDelta;

        mainCam.transform.position = newPosition;
    }
}
