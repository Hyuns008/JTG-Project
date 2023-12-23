using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FollowCamera : MonoBehaviour
{
    private Vector3 position;
    [Header("ī�޶� ���� ������Ʈ")]
    [SerializeField] private GameObject player;

    private void Update()
    {
        cameraMoving();
    }

    private void cameraMoving()
    {
        position = player.transform.position;
        position.y = player.transform.position.y + 3f;
        position.z = transform.position.z;
        transform.position = position;
    }
}
