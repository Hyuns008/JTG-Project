using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowReroadingUI : MonoBehaviour
{
    private Vector3 position;
    [Header("UI�� ���� ������Ʈ")]
    [SerializeField] private GameObject player;

    private void Update()
    {
        cameraMoving();
    }

    private void cameraMoving()
    {
        if (player == null)
        {
            return;
        }

        position = player.transform.position;
        position.y = player.transform.position.y + 0.8f;
        position.z = transform.position.z;
        transform.position = position;
    }
}