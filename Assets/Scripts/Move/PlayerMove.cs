using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f; // ĳ���� �̵� �ӵ�
    public float rotationSpeed = 720f; // ĳ���� ȸ�� �ӵ� (1�ʿ� 720��)

    private void Update()
    {
        // ����Ű �Է� �ޱ�
        float moveX = Input.GetAxis("Horizontal"); // ����(-1) �Ǵ� ������(+1)���� �̵�
        float moveZ = Input.GetAxis("Vertical");   // ������(+1) �Ǵ� �ڷ�(-1) �̵�

        // �Է� ���� ���� ������� �̵� ���� ���
        Vector3 move = new Vector3(-moveX, 0f, -moveZ);

        // �̵� ������ ũ�Ⱑ 0���� Ŭ ���� �̵� �� ȸ�� ó��
        if (move.magnitude > 0.1f)
        {
            // ĳ���� ȸ�� ó��
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // ĳ���� �̵�
            transform.Translate(move.normalized * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
