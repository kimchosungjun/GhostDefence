using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    float yDelta = 18f;  // ī�޶��� ������ ����
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
        distance = mainCam.transform.position.y - mapBounds.min.y;  // ī�޶�� �� �ٴ� ������ �Ÿ�
        frustumHeight = 2.0f * distance * Mathf.Tan(mainCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        frustumWidth = frustumHeight * mainCam.aspect;
    }

    private void Update()
    {
        Vector3 newPosition = mainCam.transform.position;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += Vector3.forward * 0.05f;// �ӵ� ����
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

        // ī�޶� ��ġ�� ��� ���� ����
        newPosition.x = Mathf.Clamp(newPosition.x, mapBounds.min.x + frustumWidth / 2, mapBounds.max.x - frustumWidth / 2);
        newPosition.z = Mathf.Clamp(newPosition.z, mapBounds.min.z + frustumHeight / 2, mapBounds.max.z - frustumHeight / 2);

        // yDelta�� ����Ͽ� y ���� ����
        newPosition.y = yDelta;

        mainCam.transform.position = newPosition;
    }
}
