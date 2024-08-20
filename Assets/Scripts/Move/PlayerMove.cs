using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f; // 캐릭터 이동 속도
    public float rotationSpeed = 720f; // 캐릭터 회전 속도 (1초에 720도)

    private void Update()
    {
        // 방향키 입력 받기
        float moveX = Input.GetAxis("Horizontal"); // 왼쪽(-1) 또는 오른쪽(+1)으로 이동
        float moveZ = Input.GetAxis("Vertical");   // 앞으로(+1) 또는 뒤로(-1) 이동

        // 입력 받은 값을 기반으로 이동 벡터 계산
        Vector3 move = new Vector3(-moveX, 0f, -moveZ);

        // 이동 벡터의 크기가 0보다 클 때만 이동 및 회전 처리
        if (move.magnitude > 0.1f)
        {
            // 캐릭터 회전 처리
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 캐릭터 이동
            transform.Translate(move.normalized * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
